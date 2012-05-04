// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsExceptionList.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base
{
	using System;
	using System.Collections.Generic;
	using System.Threading;

	/// <summary>
	/// Class which does monitor and save exceptions which happened in asynchron threads.
	/// </summary>
	public class EpicsExceptionList
	{
		/// <summary>
		///   The auto cleaner.
		/// </summary>
		private readonly Thread autoCleaner;

		/// <summary>
		///   The exception list.
		/// </summary>
		private readonly Dictionary<DateTime, Exception> exceptionList = new Dictionary<DateTime, Exception>();

		/// <summary>
		///   Initializes a new instance of the <see cref = "EpicsExceptionList" /> class.
		/// </summary>
		internal EpicsExceptionList()
		{
			this.autoCleaner = new Thread(this.autoClean);
			this.autoCleaner.IsBackground = true;
			this.autoCleaner.Start();
		}

		/// <summary>
		/// Delegate for exception capturing
		/// </summary>
		/// <param name="caughtException">
		/// Exception created in another thread
		/// </param>
		public delegate void ExceptionCaughtDelegate(Exception caughtException);

		/// <summary>
		///   Monitor for new exception caught
		/// </summary>
		public virtual event ExceptionCaughtDelegate ExceptionCaught;

		/// <summary>
		/// Clears the exceptions list
		/// </summary>
		public void clearList()
		{
			lock (this.exceptionList)
			{
				this.exceptionList.Clear();
			}
		}

		/// <summary>
		/// Get the full list of exceptions tracked by the class, including the datetime of the occurance
		/// </summary>
		/// <returns>
		/// Full list of exceptions
		/// </returns>
		public Dictionary<DateTime, Exception> getExceptionList()
		{
			return this.exceptionList;
		}

		/// <summary>
		/// The add.
		/// </summary>
		/// <param name="epicsException">
		/// The epics exception.
		/// </param>
		internal void Add(Exception epicsException)
		{
			lock (this.exceptionList)
			{
				if (!this.exceptionList.ContainsKey(DateTime.Now))
				{
					this.exceptionList.Add(DateTime.Now, epicsException);
				}
			}

			if (this.ExceptionCaught != null)
			{
				this.ExceptionCaught(epicsException);
			}
		}

		/// <summary>
		/// The auto clean.
		/// </summary>
		private void autoClean()
		{
			var timesToRemove = new List<DateTime>();

			while (true)
			{
				lock (this.exceptionList)
				{
					foreach (var entry in this.exceptionList)
					{
						if (entry.Key < DateTime.Now - new TimeSpan(0, 5, 0))
						{
							timesToRemove.Add(entry.Key);
						}
						else
						{
							break;
						}
					}

					foreach (var key in timesToRemove)
					{
						this.exceptionList.Remove(key);
					}

					timesToRemove.Clear();
				}

#if DEBUG
				Thread.Sleep(50);
#else 
        Thread.Sleep(30000);
#endif
			}
		}
	}
}