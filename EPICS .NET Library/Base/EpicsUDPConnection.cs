// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsUDPConnection.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Net;
	using System.Net.Sockets;
	using System.Threading;

	#endregion

	/// <summary>
	/// The epics udp connection.
	/// </summary>
	internal abstract class EpicsUDPConnection : IDisposable
	{
		// --> 

		/// <summary>
		///   The udp socket.
		/// </summary>
		protected Socket UDPSocket;

		/// <summary>
		///   The not disposing.
		/// </summary>
		protected bool notDisposing = true;

		/// <summary>
		///   The data pipe.
		/// </summary>
		private readonly Pipe dataPipe = new Pipe();

		// -->
		/// <summary>
		///   The ep list.
		/// </summary>
		private IPEndPoint[] EPList = new IPEndPoint[0];

		// --> Threads
		// private Thread tReceiver = null;
		/// <summary>
		///   The t handler.
		/// </summary>
		private Thread tHandler;

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsUDPConnection"/> class. 
		///   Server Constructor listens on the given Port
		/// </summary>
		/// <param name="port">
		/// </param>
		internal EpicsUDPConnection(int port)
		{
			this.UDPSocket = UDPSocketManager.GetSocket(port);

			/*UDPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            UDPSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
            UDPSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            UDPSocket.Bind(new IPEndPoint(IPAddress.Any, port));*/
			this.Start();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsUDPConnection"/> class.
		/// </summary>
		/// <param name="ipe">
		/// The ipe.
		/// </param>
		internal EpicsUDPConnection(IPEndPoint ipe)
		{
			this.UDPSocket = UDPSocketManager.GetSocket(ipe);

			/*UDPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            UDPSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
            UDPSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            UDPSocket.Bind(ipe);*/
			this.Start();
		}

		/// <summary>
		///   Initializes a new instance of the <see cref = "EpicsUDPConnection" /> class. 
		///   Client Constructor, will try to get his Port automaticly
		/// </summary>
		internal EpicsUDPConnection()
		{
			this.UDPSocket = UDPSocketManager.GetSocket();

			/*UDPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            UDPSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
            UDPSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            //do a stupid send so that it get's himself an endpoint
            UDPSocket.SendTo(new byte[1]{0}, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 60000));*/
			this.Start();
		}

		/// <summary>
		///   Gets BroadcastIPAddress.
		/// </summary>
		public IPAddress BroadcastIPAddress
		{
			get
			{
				return new IPAddress(this.IP | IPAddress.Broadcast.Address ^ SubnetMask.MaskByIp(this.IP));
			}
		}

		/// <summary>
		///   Gets IP.
		/// </summary>
		public uint IP
		{
			get
			{
				return (UInt32)((IPEndPoint)this.UDPSocket.LocalEndPoint).Address.Address;
			}
		}

		/// <summary>
		///   Gets Port.
		/// </summary>
		public int Port
		{
			get
			{
				return ((IPEndPoint)this.UDPSocket.LocalEndPoint).Port;
			}
		}

		/// <summary>
		/// The get broadcast ip address.
		/// </summary>
		/// <param name="address">
		/// The address.
		/// </param>
		/// <returns>
		/// </returns>
		public static IPAddress GetBroadcastIPAddress(IPAddress address)
		{
			return new IPAddress(address.Address ^ IPAddress.Broadcast.Address | SubnetMask.MaskByIp(address.Address));
		}

		/// <summary>
		/// The send.
		/// </summary>
		/// <param name="data">
		/// The data.
		/// </param>
		public virtual void Send(byte[] data)
		{
			try
			{
				foreach (var ep in this.EPList)
				{
					this.UDPSocket.SendTo(data, ep);
				}
			}
			catch (Exception e)
			{
				if (this.notDisposing)
				{
					Trace.Write("WUDP Send missfunctioning. (Reason: " + e + ")");
				}
			}
		}

		/// <summary>
		/// The send.
		/// </summary>
		/// <param name="data">
		/// The data.
		/// </param>
		/// <param name="receiver">
		/// The receiver.
		/// </param>
		public virtual void Send(byte[] data, IPEndPoint receiver)
		{
			try
			{
				this.UDPSocket.SendTo(data, receiver);
			}
			catch (Exception e)
			{
				if (this.notDisposing)
				{
					Trace.Write("WUDP Send missfunctioning. (Reason: " + e + ")");
				}
			}
		}

		/// <summary>
		/// The dispose.
		/// </summary>
		public void Dispose()
		{
			if (this.notDisposing)
			{
				this.notDisposing = false;
			}
			else
			{
				return;
			}

			/*UDPSocket.Close();
            UDPSocket = null;*/
			UDPSocketManager.StopListen(this.UDPSocket, this.Listen);
			UDPSocketManager.DisposeSocket(this.UDPSocket);
		}

		/// <summary>
		/// The process received data.
		/// </summary>
		/// <param name="dataStream">
		/// The data stream.
		/// </param>
		/// <param name="remoteEndPoint">
		/// The remote end point.
		/// </param>
		/// <param name="Size">
		/// The size.
		/// </param>
		protected abstract void ProcessReceivedData(Pipe dataStream, EndPoint remoteEndPoint, int Size);

		/// <summary>
		/// The wrap target list.
		/// </summary>
		/// <param name="targetList">
		/// The target list.
		/// </param>
		protected void wrapTargetList(List<string> targetList)
		{
			var i = 0;

			this.EPList = new IPEndPoint[targetList.Count];
			foreach (var server in targetList)
			{
				var serverPair = server.Split(':');
				this.EPList[i] = new IPEndPoint(
					new IPAddress(Dns.GetHostAddresses(serverPair[0])[0].Address), Convert.ToInt32(serverPair[1]));
				i++;
			}
		}

		/// <summary>
		/// The handle.
		/// </summary>
		private void Handle()
		{
			while (this.notDisposing)
			{
				try
				{
					var ipe = new IPEndPoint(new IPAddress(this.dataPipe.Read(4)), this.dataPipe.ReadInt());
					this.ProcessReceivedData(this.dataPipe, ipe, this.dataPipe.ReadInt());
				}
				catch (Exception e)
				{
					if (this.notDisposing)
					{
						Trace.Write("WException while processing on UDP-connection. (Reason: " + e + ")");
					}
				}
			}
		}

		/// <summary>
		/// The listen.
		/// </summary>
		/// <param name="socket">
		/// The socket.
		/// </param>
		/// <param name="ipe">
		/// The ipe.
		/// </param>
		/// <param name="data">
		/// The data.
		/// </param>
		/// <param name="length">
		/// The length.
		/// </param>
		private void Listen(Socket socket, IPEndPoint ipe, byte[] data, int length)
		{
			try
			{
				this.dataPipe.Write(ipe.Address.GetAddressBytes());
				this.dataPipe.Write(BitConverter.GetBytes(ipe.Port));
				this.dataPipe.Write(BitConverter.GetBytes(length));
				this.dataPipe.Write(data, 0, length);
			}
			catch (Exception e)
			{
				if (this.notDisposing)
				{
					Trace.Write("WException while listening on UDP. (Reason: " + e + ")");
				}
			}
		}

		/// <summary>
		/// The start.
		/// </summary>
		private void Start()
		{
			UDPSocketManager.Listen(this.UDPSocket, this.Listen);

			/*//start Receiver
            tReceiver = new Thread(Listen);
            tReceiver.IsBackground = true;
            tReceiver.Start();*/

			// start Handler
			this.tHandler = new Thread(this.Handle);
			this.tHandler.IsBackground = true;
			this.tHandler.Start();
		}
	}
}