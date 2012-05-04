// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsClientTCPConnection.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Client
{
	using System;
	using System.Net;
	using System.Threading;

	using Epics.Base;

	/// <summary>
	/// The epics client tcp connection.
	/// </summary>
	internal class EpicsClientTCPConnection : EpicsTCPConnection
	{
		/// <summary>
		///   The client.
		/// </summary>
		private readonly EpicsClient Client;

		/// <summary>
		///   The sent.
		/// </summary>
		private bool sent;

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsClientTCPConnection"/> class.
		/// </summary>
		/// <param name="iep">
		/// The iep.
		/// </param>
		/// <param name="client">
		/// The client.
		/// </param>
		internal EpicsClientTCPConnection(IPEndPoint iep, EpicsClient client)
			: base(iep)
		{
			this.Client = client;

			this.TCPMaxSendSize = client.Config.TCPMaxSendSize;
			this.TCPSocket.ReceiveBufferSize = client.Config.TCPBufferSize;
			this.TCPSocket.SendBufferSize = client.Config.TCPBufferSize;

			this.Send(EpicsCodec.CTCPGreet);
			this.sent = true;
		}

		/// <summary>
		/// The child dispose.
		/// </summary>
		protected override void ChildDispose()
		{
			if (this.Client.notDisposing)
			{
				this.Client.dropEpicsConnection(this.TCPSocket.RemoteEndPoint.ToString());
			}

			// Client.Codec.DropMessageCaches(TCPSocket.RemoteEndPoint);
		}

		/// <summary>
		/// The process received data.
		/// </summary>
		/// <param name="pipe">
		/// The pipe.
		/// </param>
		protected override void processReceivedData(Pipe pipe)
		{
			try
			{
				this.Client.Codec.ParseBytePackage(pipe, this.TCPSocket.RemoteEndPoint);
			}
			catch (NullReferenceException exc)
			{
				while (this.Client == null)
				{
					Thread.Sleep(0);
				}

				this.Client.Codec.ParseBytePackage(pipe, this.TCPSocket.RemoteEndPoint);
			}
			catch (Exception e)
			{
				this.Client.ExceptionContainer.Add(e);
			}
		}
	}
}