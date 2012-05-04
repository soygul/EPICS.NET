// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UDPSocketManager.cs" company="Turkish Accelerator Center">
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
	/// The udp receiver.
	/// </summary>
	/// <param name="receiver">
	/// The receiver.
	/// </param>
	/// <param name="sender">
	/// The sender.
	/// </param>
	/// <param name="data">
	/// The data.
	/// </param>
	/// <param name="size">
	/// The size.
	/// </param>
	public delegate void udpReceiver(Socket receiver, IPEndPoint sender, byte[] data, int size);

	/// <summary>
	/// The udp socket manager.
	/// </summary>
	internal static class UDPSocketManager
	{
		/// <summary>
		///   The listen functions.
		/// </summary>
		private static readonly Dictionary<string, List<udpReceiver>> ListenFunctions =
			new Dictionary<string, List<udpReceiver>>();

		/// <summary>
		///   The socket dict.
		/// </summary>
		private static readonly Dictionary<string, Socket> SocketDict = new Dictionary<string, Socket>();

		/// <summary>
		///   The socket user.
		/// </summary>
		private static readonly Dictionary<string, int> SocketUser = new Dictionary<string, int>();

		/// <summary>
		/// The dispose socket.
		/// </summary>
		/// <param name="ipe">
		/// The ipe.
		/// </param>
		public static void DisposeSocket(IPEndPoint ipe)
		{
			var canBeDisposed = false;
			lock (SocketUser)
			{
				if (SocketUser[ipe.ToString()] == 1)
				{
					canBeDisposed = true;
				}
				else
				{
					SocketUser[ipe.ToString()]--;
				}
			}

			if (canBeDisposed)
			{
				var socket = SocketDict[ipe.ToString()];
				DisposeSocket(socket, true);
			}
		}

		/// <summary>
		/// The dispose socket.
		/// </summary>
		/// <param name="socket">
		/// The socket.
		/// </param>
		public static void DisposeSocket(Socket socket)
		{
			DisposeSocket((IPEndPoint)socket.LocalEndPoint);
		}

		/// <summary>
		/// The get socket.
		/// </summary>
		/// <returns>
		/// </returns>
		public static Socket GetSocket()
		{
			var UDPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			UDPSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
			UDPSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

			// do a stupid send so that it get's himself an endpoint
			UDPSocket.SendTo(new byte[1] { 0 }, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 60000));

			lock (SocketDict)
			{
				SocketDict.Add(UDPSocket.LocalEndPoint.ToString(), UDPSocket);
			}

			lock (SocketUser)
			{
				SocketUser.Add(UDPSocket.LocalEndPoint.ToString(), 1);
			}

			return UDPSocket;
		}

		/// <summary>
		/// The get socket.
		/// </summary>
		/// <param name="port">
		/// The port.
		/// </param>
		/// <returns>
		/// </returns>
		public static Socket GetSocket(int port)
		{
			return GetSocket(new IPEndPoint(IPAddress.Any, port));
		}

		/// <summary>
		/// The get socket.
		/// </summary>
		/// <param name="ipe">
		/// The ipe.
		/// </param>
		/// <returns>
		/// </returns>
		public static Socket GetSocket(IPEndPoint ipe)
		{
			lock (SocketDict)
			{
				if (SocketDict.ContainsKey(ipe.ToString()))
				{
					lock (SocketUser)
					{
						SocketUser[ipe.ToString()]++;
					}

					return SocketDict[ipe.ToString()];
				}
				else
				{
					var UDPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
					UDPSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
					UDPSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
					UDPSocket.Bind(ipe);

					SocketDict.Add(UDPSocket.LocalEndPoint.ToString(), UDPSocket);
					lock (SocketUser)
					{
						SocketUser.Add(UDPSocket.LocalEndPoint.ToString(), 1);
					}

					return UDPSocket;
				}
			}
		}

		/// <summary>
		/// The listen.
		/// </summary>
		/// <param name="ipe">
		/// The ipe.
		/// </param>
		/// <param name="listenFunc">
		/// The listen func.
		/// </param>
		public static void Listen(IPEndPoint ipe, udpReceiver listenFunc)
		{
			var newListenfunction = false;
			lock (ListenFunctions)
			{
				if (ListenFunctions.ContainsKey(ipe.ToString()))
				{
					ListenFunctions[ipe.ToString()].Add(listenFunc);
				}
				else
				{
					newListenfunction = true;
					ListenFunctions.Add(ipe.ToString(), new List<udpReceiver> { listenFunc });
				}
			}

			if (newListenfunction)
			{
				var newListener = new Thread(ListenSocket);
				newListener.IsBackground = true;
				newListener.Start(SocketDict[ipe.ToString()]);

				// ThreadPool.QueueUserWorkItem(ListenSocket, SocketDict[ipe.ToString()]);
			}
		}

		/// <summary>
		/// The listen.
		/// </summary>
		/// <param name="port">
		/// The port.
		/// </param>
		/// <param name="listenFunc">
		/// The listen func.
		/// </param>
		public static void Listen(int port, udpReceiver listenFunc)
		{
			Listen(new IPEndPoint(IPAddress.Any, port), listenFunc);
		}

		/// <summary>
		/// The listen.
		/// </summary>
		/// <param name="socket">
		/// The socket.
		/// </param>
		/// <param name="listenFunc">
		/// The listen func.
		/// </param>
		public static void Listen(Socket socket, udpReceiver listenFunc)
		{
			Listen((IPEndPoint)socket.LocalEndPoint, listenFunc);
		}

		/// <summary>
		/// The stop listen.
		/// </summary>
		/// <param name="socket">
		/// The socket.
		/// </param>
		/// <param name="listenFunc">
		/// The listen func.
		/// </param>
		public static void StopListen(Socket socket, udpReceiver listenFunc)
		{
			lock (ListenFunctions)
			{
				ListenFunctions[socket.LocalEndPoint.ToString()].Remove(listenFunc);
			}
		}

		/// <summary>
		/// The dispose socket.
		/// </summary>
		/// <param name="socket">
		/// The socket.
		/// </param>
		/// <param name="force">
		/// The force.
		/// </param>
		private static void DisposeSocket(Socket socket, bool force)
		{
			try
			{
				lock (SocketDict)
				{
					lock (ListenFunctions)
					{
						ListenFunctions.Remove(socket.LocalEndPoint.ToString());
					}

					SocketDict.Remove(socket.LocalEndPoint.ToString());

					lock (SocketUser)
					{
						SocketUser.Remove(socket.LocalEndPoint.ToString());
					}

					socket.Close();
				}
			}
			catch (Exception e)
			{
				Trace.Write("EFailed Disposing UDPSocket (Exception: " + e + ")");
			}
		}

		/// <summary>
		/// The listen socket.
		/// </summary>
		/// <param name="state">
		/// The state.
		/// </param>
		private static void ListenSocket(object state)
		{
			Socket UDPSocket;

			byte[] lBuffer;
			List<udpReceiver> ownListeners;
			EndPoint sender = new IPEndPoint(IPAddress.Any, 0);
			var lBufferCount = 0;
			var first = true;

			UDPSocket = (Socket)state;
			var key = UDPSocket.LocalEndPoint.ToString();

			ownListeners = ListenFunctions[UDPSocket.LocalEndPoint.ToString()];
			lBuffer = new byte[UDPSocket.ReceiveBufferSize];

			while (true)
			{
				try
				{
					lBufferCount = UDPSocket.ReceiveFrom(lBuffer, ref sender);

					if (lBufferCount > 0)
					{
						lock (ownListeners)
						{
							foreach (var func in ownListeners)
							{
								func(UDPSocket, (IPEndPoint)sender, lBuffer, lBufferCount);
							}
						}

						first = false;
					}
					else
					{
						return;
					}
				}
				catch (SocketException e)
				{
					if (SocketUser.ContainsKey(key))
					{
						if (e.ErrorCode == 10054 && !first)
						{
							DisposeSocket(UDPSocket, true);
							return;
						}
						else
						{
							Trace.Write("IUDPSocketManager Listen Problem. (Code: " + e.ErrorCode + ", " + e.Message + ")");
						}

						first = false;
					}
					else
					{
						return;
					}
				}
				catch (Exception e)
				{
					if (SocketUser.ContainsKey(key))
					{
						Console.WriteLine(e.ToString());
						DisposeSocket(UDPSocket, true);
						return;
					}
					else
					{
						return;
					}
				}
			}
		}
	}
}