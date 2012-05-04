// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsTCPGWConnection.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.GateWay
{
	#region Using Directives

	using System;
	using System.Diagnostics;
	using System.Net;
	using System.Net.Sockets;
	using System.Threading;

	using Epics.Base;

	#endregion

	/// <summary>
	/// The epics tcpgw connection.
	/// </summary>
	internal class EpicsTCPGWConnection : EpicsTCPConnection
	{
		/// <summary>
		///   The gate way.
		/// </summary>
		private readonly EpicsGateWay GateWay;

		/// <summary>
		///   The client connection.
		/// </summary>
		private readonly bool clientConnection;

		/// <summary>
		///   The codec.
		/// </summary>
		private readonly EpicsCodec codec;

		/// <summary>
		///   The remote key.
		/// </summary>
		private readonly string remoteKey;

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsTCPGWConnection"/> class.
		/// </summary>
		/// <param name="TCPSocket">
		/// The tcp socket.
		/// </param>
		/// <param name="gw">
		/// The gw.
		/// </param>
		internal EpicsTCPGWConnection(Socket TCPSocket, EpicsGateWay gw)
			: base(TCPSocket)
		{
			this.GateWay = gw;
			this.codec = this.GateWay.ReceiverCodec;
			this.clientConnection = true;
			this.GateWay.Statistic.ClientConnected++;

			this.remoteKey = TCPSocket.RemoteEndPoint.ToString();

			TCPSocket.ReceiveBufferSize = this.GateWay.Config.TCPBufferSize;
			TCPSocket.SendBufferSize = this.GateWay.Config.TCPBufferSize;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsTCPGWConnection"/> class.
		/// </summary>
		/// <param name="ipe">
		/// The ipe.
		/// </param>
		/// <param name="gw">
		/// The gw.
		/// </param>
		internal EpicsTCPGWConnection(IPEndPoint ipe, EpicsGateWay gw)
			: base(ipe)
		{
			this.GateWay = gw;
			this.codec = this.GateWay.ConnectorCodec;
			this.GateWay.Statistic.ServerConnected++;

			this.TCPSocket.ReceiveBufferSize = this.GateWay.Config.TCPBufferSize;
			this.TCPSocket.SendBufferSize = this.GateWay.Config.TCPBufferSize;

			this.Send(EpicsCodec.CTCPGreet);

			this.remoteKey = ipe.ToString();
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
		/// The send.
		/// </summary>
		/// <param name="data">
		/// The data.
		/// </param>
		public override void Send(byte[] data)
		{
			this.GateWay.Statistic.BytesSend += data.Length;

			base.Send(data);
		}

		/// <summary>
		/// The child dispose.
		/// </summary>
		protected override void ChildDispose()
		{
			if (this.clientConnection)
			{
				this.GateWay.Statistic.ClientConnected--;
			}
			else
			{
				this.GateWay.Statistic.ServerConnected--;
			}

			if (this.GateWay.notDisposing)
			{
				this.GateWay.DropEpicsConnection(this.remoteKey);
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
				this.codec.ParseBytePackage(pipe, this.TCPSocket.RemoteEndPoint);
			}
			catch (NullReferenceException exc)
			{
				while (this.codec == null)
				{
					Thread.Sleep(0);
				}

				this.codec.ParseBytePackage(pipe, this.TCPSocket.RemoteEndPoint);
			}
			catch (Exception e)
			{
				Trace.Write("WFailure during parsing TCP Packet");
			}
		}
	}
}