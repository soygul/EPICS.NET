// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsServerUDPConnection.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Server
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Net;
	using System.Net.NetworkInformation;
	using System.Threading;

	using Epics.Base;

	#endregion

	/// <summary>
	/// The epics server udp connection.
	/// </summary>
	internal class EpicsServerUDPConnection : EpicsUDPConnection
	{
		/// <summary>
		///   The server.
		/// </summary>
		private readonly EpicsServer Server;

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsServerUDPConnection"/> class.
		/// </summary>
		/// <param name="server">
		/// The server.
		/// </param>
		public EpicsServerUDPConnection(EpicsServer server)
			: base(new IPEndPoint(IPAddress.Parse(server.Config.ListenIP), server.Config.UDPPort))
		{
			this.Server = server;

			this.UDPSocket.ReceiveBufferSize = server.Config.UDPBufferSize;
			this.UDPSocket.SendBufferSize = server.Config.UDPBufferSize;

			var targetAddresses = new List<string>();
			var interfaces = NetworkInterface.GetAllNetworkInterfaces();
			long longIp = 0;
			IPAddress ip = null;

			foreach (var iface in interfaces)
			{
				var prop = iface.GetIPProperties();

				try
				{
					longIp = (prop.UnicastAddresses[0].IPv4Mask.Address ^ IPAddress.Broadcast.Address)
					         | prop.UnicastAddresses[0].Address.Address;
					ip = new IPAddress(longIp);
				}
				catch (Exception e)
				{
					continue;
				}

				targetAddresses.Add(ip + ":" + this.Server.Config.UDPDestPort);
			}

			if (targetAddresses.Count == 0)
			{
				targetAddresses.Add(IPAddress.Broadcast + ":" + this.Server.Config.UDPDestPort);
			}

			this.wrapTargetList(targetAddresses);
		}

		/// <summary>
		/// The process received data.
		/// </summary>
		/// <param name="dataPipe">
		/// The data pipe.
		/// </param>
		/// <param name="remoteEndPoint">
		/// The remote end point.
		/// </param>
		/// <param name="size">
		/// The size.
		/// </param>
		protected override void ProcessReceivedData(Pipe dataPipe, EndPoint remoteEndPoint, int size)
		{
			try
			{
				this.Server.Codec.ParseBytePackage(dataPipe, remoteEndPoint, size);
			}
			catch (NullReferenceException exc)
			{
				while (this.Server == null)
				{
					Thread.Sleep(0);
				}

				this.Server.Codec.ParseBytePackage(dataPipe, remoteEndPoint, size);
			}
			catch (Exception e)
			{
				Trace.Write("WServer Parsing Error");
			}
		}
	}
}