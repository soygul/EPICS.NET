// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsServerTCPConnection.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Server
{
	#region Using Directives

	using System;
	using System.Net.Sockets;
	using System.Threading;

	using Epics.Base;

	#endregion

	/// <summary>
	/// The epics server tcp connection.
	/// </summary>
	internal class EpicsServerTCPConnection : EpicsTCPConnection
	{
		/// <summary>
		///   The server.
		/// </summary>
		private readonly EpicsServer Server;

		/// <summary>
		///   The remote key.
		/// </summary>
		private readonly string remoteKey;

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsServerTCPConnection"/> class.
		/// </summary>
		/// <param name="TCPSocket">
		/// The tcp socket.
		/// </param>
		/// <param name="server">
		/// The server.
		/// </param>
		internal EpicsServerTCPConnection(Socket TCPSocket, EpicsServer server)
			: base(TCPSocket)
		{
			this.Server = server;
			this.remoteKey = TCPSocket.RemoteEndPoint.ToString();
		}

		/// <summary>
		///   Gets or sets Hostname.
		/// </summary>
		public string Hostname { get; set; }

		/// <summary>
		///   Gets or sets Username.
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// The child dispose.
		/// </summary>
		protected override void ChildDispose()
		{
			if (this.Server.notDisposing)
			{
				this.Server.DropEpicsConnection(this.remoteKey);
			}
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
				this.Server.Codec.ParseBytePackage(pipe, this.TCPSocket.RemoteEndPoint);
			}
			catch (NullReferenceException exc)
			{
				while (this.Server == null)
				{
					Thread.Sleep(0);
				}

				this.Server.Codec.ParseBytePackage(pipe, this.TCPSocket.RemoteEndPoint);
			}
			catch (Exception e)
			{
			}
		}
	}
}