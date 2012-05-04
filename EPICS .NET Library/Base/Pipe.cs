// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Pipe.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base
{
	#region Using Directives

	using System;
	using System.Threading;

	#endregion

	/// <summary>
	/// The pipe.
	/// </summary>
	public class Pipe : IDisposable
	{
		/// <summary>
		///   The data semaphore.
		/// </summary>
		private readonly Semaphore dataSemaphore = new Semaphore(1, 1);

		/// <summary>
		///   The wait for new data.
		/// </summary>
		private readonly AutoResetEvent waitForNewData = new AutoResetEvent(false);

		/// <summary>
		///   The wait for read.
		/// </summary>
		private readonly AutoResetEvent waitForRead = new AutoResetEvent(false);

		/// <summary>
		///   The data.
		/// </summary>
		private byte[] data;

		/// <summary>
		///   The is disposing.
		/// </summary>
		private bool isDisposing;

		/// <summary>
		///   The read position.
		/// </summary>
		private int readPosition;

		/// <summary>
		///   The write position.
		/// </summary>
		private int writePosition;

		/// <summary>
		///   Initializes a new instance of the <see cref = "Pipe" /> class.
		/// </summary>
		public Pipe()
		{
			this.data = new byte[32768];
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Pipe"/> class.
		/// </summary>
		/// <param name="size">
		/// The size.
		/// </param>
		public Pipe(int size)
		{
			this.data = new byte[size];
		}

		/// <summary>
		/// The flush.
		/// </summary>
		public void Flush()
		{
			this.dataSemaphore.WaitOne();
			this.readPosition = 0;
			this.writePosition = 0;
			this.dataSemaphore.Release();
		}

		/// <summary>
		/// The read.
		/// </summary>
		/// <param name="size">
		/// The size.
		/// </param>
		/// <returns>
		/// </returns>
		public byte[] Read(int size)
		{
			if (size == 0 || this.isDisposing)
			{
				return new byte[0];
			}

			var spare = 0;
			var result = new byte[size];
			long realLength = 0;

			// if the readposition and the write osition is at the same position do
			// wait for new data
			this.dataSemaphore.WaitOne();
			if (this.writePosition == this.readPosition)
			{
				this.dataSemaphore.Release();
				this.waitForNewData.WaitOne();
			}
			else
			{
				this.dataSemaphore.Release();
			}

			// be sure we have enough data
			do
			{
				if (this.isDisposing)
				{
					return new byte[0];
				}

				this.dataSemaphore.WaitOne();
				if (this.writePosition < this.readPosition)
				{
					realLength = (this.data.Length - this.readPosition) + this.writePosition;
				}
				else
				{
					realLength = this.writePosition - this.readPosition;
				}

				this.dataSemaphore.Release();

				// if there is not enough data to read, it will wait till new arrives or
				// one second passed. because it could be, that due to some really bad luck
				// the new data just happened between the lock open and the wait.
				if (size > realLength)
				{
					this.waitForNewData.WaitOne();
				}
			}
			while (size > realLength);

			if (this.isDisposing)
			{
				return new byte[0];
			}

			this.dataSemaphore.WaitOne();

			// they are on the same loop
			if (this.writePosition > this.readPosition)
			{
				Buffer.BlockCopy(this.data, this.readPosition, result, 0, size);
				this.readPosition += size;
			}
			else
			{
				if ((this.data.Length - this.readPosition) >= size)
				{
					Buffer.BlockCopy(this.data, this.readPosition, result, 0, size);
					this.readPosition += size;
				}
				else
				{
					spare = this.data.Length - this.readPosition;
					Buffer.BlockCopy(this.data, this.readPosition, result, 0, spare);
					Buffer.BlockCopy(this.data, 0, result, spare, size - spare);
					this.readPosition = size - spare;
				}
			}

			this.dataSemaphore.Release();

			this.waitForRead.Set();
			return result;
		}

		/// <summary>
		/// The read int.
		/// </summary>
		/// <returns>
		/// The read int.
		/// </returns>
		public int ReadInt()
		{
			return BitConverter.ToInt32(this.Read(4), 0);
		}

		/// <summary>
		/// The write.
		/// </summary>
		/// <param name="Data">
		/// The data.
		/// </param>
		public void Write(byte[] Data)
		{
			this.Write(Data, 0, Data.Length);
		}

		/// <summary>
		/// The write.
		/// </summary>
		/// <param name="Data">
		/// The data.
		/// </param>
		/// <param name="offset">
		/// The offset.
		/// </param>
		/// <param name="length">
		/// The length.
		/// </param>
		public void Write(byte[] Data, int offset, int length)
		{
			if (this.isDisposing)
			{
				return;
			}

			var spare = 0;

			this.dataSemaphore.WaitOne();

			// trying to write more than there is space?!
			while ((this.writePosition > this.readPosition
			        && this.data.Length - this.writePosition + this.readPosition - 1 < length)
			       || (this.writePosition < this.readPosition && this.readPosition - this.writePosition - 1 < length)
			       || (length > this.data.Length))
			{
				// calculate free space
				if (this.writePosition < this.readPosition)
				{
					spare = this.readPosition - this.writePosition - 1;
				}
				else
				{
					spare = this.data.Length - this.writePosition + this.readPosition - 1;
				}

				this.dataSemaphore.Release();
				this.Write(Data, offset, spare);
				this.dataSemaphore.WaitOne();

				offset += spare;
				length -= spare;
			}

			this.dataSemaphore.Release();
			Thread.Sleep(0);
			this.dataSemaphore.WaitOne();

			// check if there is enough space to write 
			if (this.data.Length - this.writePosition == length)
			{
				Buffer.BlockCopy(Data, offset, this.data, this.writePosition, length);
				this.writePosition = 0;
			}
			else if (this.data.Length - this.writePosition > length)
			{
				Buffer.BlockCopy(Data, offset, this.data, this.writePosition, length);
				this.writePosition += length;
			}
			else
			{
				spare = this.data.Length - this.writePosition;
				Buffer.BlockCopy(Data, offset, this.data, this.writePosition, spare);
				Buffer.BlockCopy(Data, offset + spare, this.data, 0, length - spare);
				this.writePosition = length - spare;
			}

			this.dataSemaphore.Release();

			this.waitForNewData.Set();
		}

		/// <summary>
		/// The dispose.
		/// </summary>
		public void Dispose()
		{
			if (this.isDisposing)
			{
				return;
			}
			else
			{
				this.isDisposing = true;
			}

			this.dataSemaphore.WaitOne();
			this.data = null;
			this.readPosition = 0;
			this.writePosition = 0;
			this.dataSemaphore.Release();

			// may trigger a waiting 
			this.waitForNewData.Set();
		}
	}
}