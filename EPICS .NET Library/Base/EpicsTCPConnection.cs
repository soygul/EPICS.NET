// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsTCPConnection.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base
{
	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Net;
	using System.Net.Sockets;
	using System.Threading;

	/// <summary>
	/// The epics tcp connection.
	/// </summary>
	internal abstract class EpicsTCPConnection : IDisposable
	{
		/// <summary>
		///   The tcp max send size.
		/// </summary>
		protected int TCPMaxSendSize = 8192;

		/// <summary>
		///   The tcp socket.
		/// </summary>
		protected Socket TCPSocket;

		/// <summary>
		///   The not disposing.
		/// </summary>
		protected bool notDisposing = true;

		/// <summary>
		///   The data pipe.
		/// </summary>
		private readonly Pipe dataPipe = new Pipe();

		/// <summary>
		///   The ae conn.
		/// </summary>
		private AutoResetEvent aeConn = new AutoResetEvent(false);

		/// <summary>
		///   The handler.
		/// </summary>
		private Thread handler;

		/// <summary>
		///   The listener.
		/// </summary>
		private Thread listener;

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsTCPConnection"/> class. 
		///   TCPConnection Constructor for Server &gt; Client Connection
		/// </summary>
		/// <param name="tCPSocket">
		/// The t CP Socket.
		/// </param>
		internal EpicsTCPConnection(Socket tCPSocket)
		{
			this.TCPSocket = tCPSocket;
			this.Start();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsTCPConnection"/> class. 
		///   TCPConnection Constructor for Client &gt; Server Connection
		/// </summary>
		/// <param name="iep">
		/// IPEndPoint of the targeted Server
		/// </param>
		internal EpicsTCPConnection(IPEndPoint iep)
		{
			this.TCPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			this.TCPSocket.Connect(iep);

			this.Start();
		}

		/// <summary>
		/// The connection state changed delegate.
		/// </summary>
		/// <param name="connected">
		/// The connected.
		/// </param>
		internal delegate void ConnectionStateChangedDelegate(bool connected);

		/// <summary>
		///   The connection state changed.
		/// </summary>
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		internal event ConnectionStateChangedDelegate ConnectionStateChanged;

		/// <summary>
		///   Gets a value indicating whether Connected.
		/// </summary>
		public bool Connected
		{
			get
			{
				return this.TCPSocket.Connected;
			}
		}

		/// <summary>
		///   Gets RemoteEndPoint.
		/// </summary>
		public IPEndPoint RemoteEndPoint
		{
			get
			{
				return (IPEndPoint)this.TCPSocket.RemoteEndPoint;
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
			if (this.notDisposing)
			{
				try
				{
					this.TCPSocket.Send(data);
				}
				catch (SocketException e2)
				{
					// connection was forcibly closed
					if (e2.ErrorCode == 10054)
					{
						this.Dispose();
						return;
					}

					if (this.notDisposing)
					{
						Trace.Write("WException while sending on the TCP. (Reason: " + e2 + ",ErrorCode: " + e2.ErrorCode + ")");
					}
				}
				catch (Exception e)
				{
					if (this.notDisposing)
					{
						Trace.Write("WException while sending on the TCP. (Reason: " + e + ")");
					}
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

			try
			{
				this.ChildDispose();
			}
			catch (Exception e)
			{
				Trace.Write("EException during child dispose on TCP-connection. (Reason: " + e + ")");
			}

			this.TCPSocket.Close();

			this.dataPipe.Dispose();

			if (this.ConnectionStateChanged != null)
			{
				this.ConnectionStateChanged(false);
			}

			this.TCPSocket = null;
		}

		/// <summary>
		/// The child dispose.
		/// </summary>
		protected abstract void ChildDispose();

		/// <summary>
		/// The process received data.
		/// </summary>
		/// <param name="dataPipe">
		/// The data pipe.
		/// </param>
		protected abstract void processReceivedData(Pipe dataPipe);

		/// <summary>
		/// The handle.
		/// </summary>
		private void Handle()
		{
			while (this.notDisposing)
			{
				try
				{
					this.processReceivedData(this.dataPipe);
				}
				catch (Exception e)
				{
					if (this.notDisposing)
					{
						Trace.Write("WException while processing data on TCP-connection. (Reason: " + e + ")");
					}
				}
			}
		}

		/// <summary>
		/// The listen.
		/// </summary>
		private void Listen()
		{
			var lBuffer = new byte[this.TCPSocket.ReceiveBufferSize];
			var lBufferCount = 0;

			while (this.notDisposing)
			{
				try
				{
					lBufferCount = this.TCPSocket.Receive(lBuffer);

					if (lBufferCount > 0)
					{
						this.dataPipe.Write(lBuffer, 0, lBufferCount);
					}
					else
					{
						this.Dispose();
					}
				}
				catch (SocketException e)
				{
					if (this.notDisposing && e.ErrorCode != 10054)
					{
						Trace.Write("WTCP Connection unnicely closed. (" + e.ErrorCode + ")");
					}

					this.Dispose();
				}
				catch (Exception e2)
				{
					if (this.notDisposing)
					{
						Trace.Write("WException while listening on TCP-connection. (Reason: " + e2 + ")");
					}
				}
			}
		}

		/// <summary>
		/// The start.
		/// </summary>
		private void Start()
		{
			this.listener = new Thread(this.Listen);
			this.listener.IsBackground = true;
			this.listener.Start();

			// tell all listener that we are connected
			if (this.ConnectionStateChanged != null)
			{
				this.ConnectionStateChanged(true);
			}

			// start a new checker, to catch disconnects
			this.handler = new Thread(this.Handle);
			this.handler.IsBackground = true;
			this.handler.Start();
		}
	}
}