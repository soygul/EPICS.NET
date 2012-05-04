// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsGateWay.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.GateWay
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Net;
	using System.Net.Sockets;
	using System.Threading;

	using Epics.Base;
	using Epics.Base.Constants;

	#endregion

	/// <summary>
	/// The epics gate way.
	/// </summary>
	public class EpicsGateWay : IDisposable
	{
		// Client-Site

		/// <summary>
		///   The gw client chan id.
		/// </summary>
		public uint GWClientChanId;

		/// <summary>
		///   The gw ioc chan id.
		/// </summary>
		public uint GWIocChanId;

		/// <summary>
		///   The subscription id.
		/// </summary>
		public uint SubscriptionId;

		/// <summary>
		///   The job id.
		/// </summary>
		public uint jobId;

		/// <summary>
		///   The not disposing.
		/// </summary>
		public bool notDisposing = true;

		/// <summary>
		///   The channel list client id.
		/// </summary>
		internal Dictionary<uint, EpicsGateWayClientChannel> ChannelListClientId =
			new Dictionary<uint, EpicsGateWayClientChannel>();

		/// <summary>
		///   The channel list ioc id.
		/// </summary>
		internal Dictionary<uint, EpicsGateWayIocChannel> ChannelListIocId = new Dictionary<uint, EpicsGateWayIocChannel>();

		/// <summary>
		///   The channel list ioc name.
		/// </summary>
		internal Dictionary<string, EpicsGateWayIocChannel> ChannelListIocName =
			new Dictionary<string, EpicsGateWayIocChannel>();

		/// <summary>
		///   The channel search name wait.
		/// </summary>
		internal Dictionary<string, EpicsGateWayIocChannel> ChannelSearchNameWait =
			new Dictionary<string, EpicsGateWayIocChannel>();

		/// <summary>
		///   The subscription list.
		/// </summary>
		internal Dictionary<uint, EpicsGateWayMonitor> SubscriptionList = new Dictionary<uint, EpicsGateWayMonitor>();

		/// <summary>
		///   The tcp connections.
		/// </summary>
		internal Dictionary<string, EpicsTCPGWConnection> TCPConnections = new Dictionary<string, EpicsTCPGWConnection>();

		/// <summary>
		///   The udp conn from.
		/// </summary>
		internal EpicsUDPGWConnection UdpConnFrom;

		/// <summary>
		///   The beacon dictionary.
		/// </summary>
		private readonly Dictionary<string, DateTime> BeaconDictionary = new Dictionary<string, DateTime>();

		/// <summary>
		///   The channel queue.
		/// </summary>
		private readonly Queue<KeyValuePair<uint, string>> ChannelQueue = new Queue<KeyValuePair<uint, string>>();

		/// <summary>
		///   The channel search id wait.
		/// </summary>
		private readonly Dictionary<uint, EpicsGateWayIocChannel> ChannelSearchIdWait =
			new Dictionary<uint, EpicsGateWayIocChannel>();

		// ActionHandling

		/// <summary>
		///   The job dict.
		/// </summary>
		private readonly Dictionary<uint, JobHandle> JobDict = new Dictionary<uint, JobHandle>();

		/// <summary>
		///   The beacon care bear.
		/// </summary>
		private Thread BeaconCareBear;

		/// <summary>
		///   The beacon handler.
		/// </summary>
		private Beaconizer BeaconHandler;

		/// <summary>
		///   The channel searcher.
		/// </summary>
		private Thread ChannelSearcher;

		/// <summary>
		///   The inited.
		/// </summary>
		private bool Inited;

		/// <summary>
		///   The udp conn to.
		/// </summary>
		private EpicsUDPGWConnection UdpConnTo;

		/// <summary>
		///   The config.
		/// </summary>
		private EpicsGateWayConfig config = new EpicsGateWayConfig();

		/// <summary>
		///   The connection grabber.
		/// </summary>
		private Thread connectionGrabber;

		/// <summary>
		///   The connector codec.
		/// </summary>
		private EpicsGateWayConnectorCodec connectorCodec;

		/// <summary>
		///   The receiver codec.
		/// </summary>
		private EpicsGateWayReceiverCodec receiverCodec;

		/// <summary>
		///   The tcp listener.
		/// </summary>
		private TcpListener tcpListener;

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsGateWay"/> class.
		/// </summary>
		/// <param name="ipFrom">
		/// The ip from.
		/// </param>
		/// <param name="uDPListen">
		/// The u dp listen.
		/// </param>
		/// <param name="beaconPortFrom">
		/// The beacon port from.
		/// </param>
		/// <param name="tCPListen">
		/// The t cp listen.
		/// </param>
		/// <param name="uDPSend">
		/// The u dp send.
		/// </param>
		/// <param name="ipTo">
		/// The ip to.
		/// </param>
		/// <param name="beaconPortTo">
		/// The beacon port to.
		/// </param>
		/// <param name="targetIPList">
		/// The target ip list.
		/// </param>
		public EpicsGateWay(
			string ipFrom, 
			int uDPListen, 
			int beaconPortFrom, 
			int tCPListen, 
			int uDPSend, 
			string ipTo, 
			int beaconPortTo, 
			string targetIPList)
		{
			this.Init(
				ipFrom, 
				uDPListen, 
				beaconPortFrom, 
				tCPListen, 
				uDPSend, 
				ipTo, 
				beaconPortTo, 
				targetIPList, 
				new EpicsGateWayDefaultAccess());
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsGateWay"/> class.
		/// </summary>
		/// <param name="ipFrom">
		/// The ip from.
		/// </param>
		/// <param name="uDPListen">
		/// The u dp listen.
		/// </param>
		/// <param name="beaconPortFrom">
		/// The beacon port from.
		/// </param>
		/// <param name="tCPListen">
		/// The t cp listen.
		/// </param>
		/// <param name="uDPSend">
		/// The u dp send.
		/// </param>
		/// <param name="ipTo">
		/// The ip to.
		/// </param>
		/// <param name="beaconPortTo">
		/// The beacon port to.
		/// </param>
		/// <param name="targetIPList">
		/// The target ip list.
		/// </param>
		/// <param name="Rules">
		/// The rules.
		/// </param>
		public EpicsGateWay(
			string ipFrom, 
			int uDPListen, 
			int beaconPortFrom, 
			int tCPListen, 
			int uDPSend, 
			string ipTo, 
			int beaconPortTo, 
			string targetIPList, 
			IRuleSet Rules)
		{
			this.Init(ipFrom, uDPListen, beaconPortFrom, tCPListen, uDPSend, ipTo, beaconPortTo, targetIPList, Rules);
		}

		/// <summary>
		///   Gets Config.
		/// </summary>
		public EpicsGateWayConfig Config
		{
			get
			{
				return this.config;
			}

			private set
			{
				this.config = value;
			}
		}

		/// <summary>
		///   Gets Statistic.
		/// </summary>
		public EpicsGateWayStatistics Statistic { get; private set; }

		/// <summary>
		///   Gets ConnectorCodec.
		/// </summary>
		internal EpicsGateWayConnectorCodec ConnectorCodec
		{
			get
			{
				return this.connectorCodec;
			}

			private set
			{
				this.connectorCodec = value;
			}
		}

		/// <summary>
		///   Gets ReceiverCodec.
		/// </summary>
		internal EpicsGateWayReceiverCodec ReceiverCodec
		{
			get
			{
				return this.receiverCodec;
			}

			private set
			{
				this.receiverCodec = value;
			}
		}

		/// <summary>
		///   Gets or sets Rules.
		/// </summary>
		internal IRuleSet Rules { get; set; }

		/// <summary>
		///   Gets TcpSocketPort.
		/// </summary>
		internal int TcpSocketPort { get; private set; }

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

			this.BeaconHandler.Dispose();
			this.BeaconHandler = null;

			// close Connections
			this.tcpListener.Stop();
			this.tcpListener = null;

			this.UdpConnFrom.Dispose();
			this.UdpConnTo.Dispose();

			// close Connections will close all channels and monitors related to them
			lock (this.TCPConnections)
			{
				foreach (var pair in this.TCPConnections)
				{
					pair.Value.Dispose();
				}

				this.TCPConnections.Clear();
			}
		}

		/// <summary>
		/// The care beacons.
		/// </summary>
		internal void CareBeacons()
		{
			var toDelete = new List<string>();
			var timeOut = new TimeSpan(0, 0, 45);

			while (this.notDisposing)
			{
				lock (this.BeaconDictionary)
				{
					foreach (var entry in this.BeaconDictionary)
					{
						if (DateTime.Now - entry.Value > timeOut)
						{
							toDelete.Add(entry.Key);
						}
					}

					foreach (var key in toDelete)
					{
						this.BeaconDictionary.Remove(key);
					}
				}

				Thread.Sleep(15000);
			}
		}

		/// <summary>
		/// The create client channel.
		/// </summary>
		/// <param name="ClientChanId">
		/// The client chan id.
		/// </param>
		/// <param name="channelName">
		/// The channel name.
		/// </param>
		/// <param name="iep">
		/// The iep.
		/// </param>
		internal void CreateClientChannel(uint ClientChanId, string channelName, EndPoint iep)
		{
			lock (this.ChannelListClientId)
			{
				this.GWClientChanId++;
				this.ChannelListClientId.Add(
					this.GWClientChanId, new EpicsGateWayClientChannel(this, ClientChanId, channelName, iep, this.GWClientChanId));
			}
		}

		/// <summary>
		/// The drop client channel.
		/// </summary>
		/// <param name="gWClientChanId">
		/// The g w client chan id.
		/// </param>
		internal void DropClientChannel(uint gWClientChanId)
		{
			lock (this.ChannelListClientId)
			{
				this.ChannelListClientId.Remove(gWClientChanId);
			}
		}

		/// <summary>
		/// The drop epics connection.
		/// </summary>
		/// <param name="endpoint">
		/// The endpoint.
		/// </param>
		internal void DropEpicsConnection(string endpoint)
		{
			lock (this.TCPConnections)
			{
				if (this.TCPConnections.ContainsKey(endpoint))
				{
					this.TCPConnections.Remove(endpoint);
				}
			}
		}

		/// <summary>
		/// The drop ioc channel.
		/// </summary>
		/// <param name="gWIocChanId">
		/// The g w ioc chan id.
		/// </param>
		internal void DropIocChannel(uint gWIocChanId)
		{
			lock (this.ChannelListIocId)
			{
				// if it does not exist, we do not have to remove it, sounds correct
				if (!this.ChannelListIocId.ContainsKey(gWIocChanId))
				{
					return;
				}

				lock (this.ChannelListIocName)
				{
					this.ChannelListIocName.Remove(this.ChannelListIocId[gWIocChanId].ChannelName);
				}

				this.ChannelListIocId.Remove(gWIocChanId);
			}
		}

		/// <summary>
		/// The drop monitor.
		/// </summary>
		/// <param name="key">
		/// The key.
		/// </param>
		internal void DropMonitor(uint key)
		{
			lock (this.SubscriptionList)
			{
				this.SubscriptionList.Remove(key);
			}
		}

		/// <summary>
		/// The found channel.
		/// </summary>
		/// <param name="gWIocChanId">
		/// The g w ioc chan id.
		/// </param>
		/// <param name="iep">
		/// The iep.
		/// </param>
		/// <param name="port">
		/// The port.
		/// </param>
		internal void FoundChannel(uint gWIocChanId, EndPoint iep, ushort port)
		{
			EpicsGateWayIocChannel tmpChannel = null;

			// remove from search List
			lock (this.ChannelSearchIdWait)
			{
				if (this.ChannelSearchIdWait.ContainsKey(gWIocChanId))
				{
					tmpChannel = this.ChannelSearchIdWait[gWIocChanId];
					this.ChannelSearchIdWait.Remove(gWIocChanId);
					this.ChannelSearchNameWait.Remove(tmpChannel.ChannelName);
				}
			}

			if (tmpChannel == null)
			{
				return;
			}

			if (this.ChannelListIocName.ContainsKey(tmpChannel.ChannelName))
			{
				return;
			}

			// add to the translation dictionary
			lock (this.ChannelListIocId)
			{
				this.ChannelListIocId.Add(gWIocChanId, tmpChannel);
				this.ChannelListIocName.Add(tmpChannel.ChannelName, tmpChannel);
			}

			tmpChannel.Connection = this.getConnection(new IPEndPoint(((IPEndPoint)iep).Address, port));
		}

		/// <summary>
		/// The handle open job.
		/// </summary>
		/// <param name="jobId">
		/// The job id.
		/// </param>
		/// <returns>
		/// </returns>
		internal JobHandle HandleOpenJob(uint jobId)
		{
			JobHandle item;

			lock (this.JobDict)
			{
				if (this.JobDict.ContainsKey(jobId))
				{
					item = this.JobDict[jobId];
					this.JobDict.Remove(jobId);
				}
				else
				{
					item = JobHandle.emptyOne;
				}
			}

			return item;
		}

		/// <summary>
		/// The register new job.
		/// </summary>
		/// <param name="jobId">
		/// The job id.
		/// </param>
		/// <param name="item">
		/// The item.
		/// </param>
		internal void RegisterNewJob(uint jobId, JobHandle item)
		{
			lock (this.JobDict)
			{
				this.JobDict.Add(jobId, item);
			}
		}

		/// <summary>
		/// The register new monitor.
		/// </summary>
		/// <param name="newMonitor">
		/// The new monitor.
		/// </param>
		/// <returns>
		/// The register new monitor.
		/// </returns>
		internal uint RegisterNewMonitor(EpicsGateWayMonitor newMonitor)
		{
			uint key;
			lock (this.SubscriptionList)
			{
				key = this.SubscriptionId++;
				this.SubscriptionList.Add(key, newMonitor);
			}

			return key;
		}

		/// <summary>
		/// The search for channel.
		/// </summary>
		/// <param name="clientChanId">
		/// The client chan id.
		/// </param>
		/// <param name="ChannelName">
		/// The channel name.
		/// </param>
		/// <param name="iep">
		/// The iep.
		/// </param>
		internal void SearchForChannel(uint clientChanId, string ChannelName, EndPoint iep)
		{
			if (this.Rules.GetAccessRights(iep, "*", ChannelName) == AccessRights.NoAccess)
			{
				return;
			}

			// check if we already have the channel
			if (this.ChannelListIocName.ContainsKey(ChannelName))
			{
				this.UdpConnFrom.Send(this.receiverCodec.channelFoundMessage(clientChanId), (IPEndPoint)iep);
			}
			else
			{
				lock (this.ChannelSearchIdWait)
				{
					if (!this.ChannelSearchNameWait.ContainsKey(ChannelName))
					{
						var tmpChannel = new EpicsGateWayIocChannel(ChannelName, ++this.GWIocChanId, this, iep, clientChanId);

						this.ChannelSearchIdWait.Add(tmpChannel.GWIocChanId, tmpChannel);
						this.ChannelSearchNameWait.Add(ChannelName, tmpChannel);

						lock (this.ChannelQueue)
						{
							this.ChannelQueue.Enqueue(new KeyValuePair<uint, string>(tmpChannel.GWIocChanId, ChannelName));
						}
					}
				}
			}
		}

		/// <summary>
		/// The get connection.
		/// </summary>
		/// <param name="remoteEndpoint">
		/// The remote endpoint.
		/// </param>
		/// <returns>
		/// </returns>
		internal EpicsTCPGWConnection getConnection(EndPoint remoteEndpoint)
		{
			if (!this.TCPConnections.ContainsKey(remoteEndpoint.ToString()))
			{
				lock (this.TCPConnections)
				{
					this.TCPConnections.Add(remoteEndpoint.ToString(), new EpicsTCPGWConnection((IPEndPoint)remoteEndpoint, this));
				}
			}

			return this.TCPConnections[remoteEndpoint.ToString()];
		}

		/// <summary>
		/// The handle beacon.
		/// </summary>
		/// <param name="sender">
		/// The sender.
		/// </param>
		internal void handleBeacon(EndPoint sender)
		{
			var needToProduceAnomaly = true;
			var EndPointName = sender.ToString();
			var AnomalySmallRange = new TimeSpan(0, 0, 0, 0, 300);

			lock (this.BeaconDictionary)
			{
				if (this.BeaconDictionary.ContainsKey(EndPointName))
				{
					if (DateTime.Now - this.BeaconDictionary[EndPointName] > AnomalySmallRange)
					{
						needToProduceAnomaly = false;
					}

					this.BeaconDictionary[EndPointName] = DateTime.Now;
				}
				else
				{
					this.BeaconDictionary.Add(EndPointName, DateTime.Now);
				}
			}

			if (needToProduceAnomaly)
			{
				this.BeaconHandler.ProduceAnomaly();
			}
		}

		/// <summary>
		/// The grab connection.
		/// </summary>
		private void GrabConnection()
		{
			Socket newConnectionSocket;
			while (this.notDisposing)
			{
				try
				{
					newConnectionSocket = this.tcpListener.AcceptSocket();
				}
				catch (Exception e)
				{
					continue;
				}

				lock (this.TCPConnections)
				{
					if (!this.TCPConnections.ContainsKey(newConnectionSocket.RemoteEndPoint.ToString()))
					{
						this.TCPConnections.Add(
							newConnectionSocket.RemoteEndPoint.ToString(), new EpicsTCPGWConnection(newConnectionSocket, this));
					}
				}
			}
		}

		/// <summary>
		/// The handle channel search.
		/// </summary>
		private void HandleChannelSearch()
		{
			var i = 0;

			var timeToSleep = new TimeSpan(0, 0, 0, 0, this.config.ChannelSearchInterval);
			var timeTillNotFound = new TimeSpan(0, 0, 15);
			var timeUsed = new Stopwatch();

			while (this.notDisposing)
			{
				timeUsed.Reset();
				timeUsed.Start();
				if (this.ChannelQueue.Count > 0)
				{
					this.UdpConnTo.Send(this.connectorCodec.searchPackage(this.ChannelQueue));
				}

				timeUsed.Stop();

				if ((i++) % 100 == 0)
				{
					lock (this.ChannelSearchIdWait)
					{
						var toDrop = new List<EpicsGateWayIocChannel>();

						foreach (var pair in this.ChannelSearchIdWait)
						{
							if (pair.Value.CreatedAt < DateTime.Now - timeTillNotFound)
							{
								toDrop.Add(pair.Value);
							}
						}

						foreach (var channel in toDrop)
						{
							channel.Dispose();
							this.ChannelSearchIdWait.Remove(channel.GWIocChanId);
							this.ChannelSearchNameWait.Remove(channel.ChannelName);
						}
					}
				}

				if (timeUsed.Elapsed < timeToSleep)
				{
					Thread.Sleep(timeToSleep - timeUsed.Elapsed);
				}
			}
		}

		/// <summary>
		/// The init.
		/// </summary>
		/// <param name="ipFrom">
		/// The ip from.
		/// </param>
		/// <param name="uDPListen">
		/// The u dp listen.
		/// </param>
		/// <param name="beaconPortFrom">
		/// The beacon port from.
		/// </param>
		/// <param name="tCPListen">
		/// The t cp listen.
		/// </param>
		/// <param name="uDPSend">
		/// The u dp send.
		/// </param>
		/// <param name="ipTo">
		/// The ip to.
		/// </param>
		/// <param name="beaconPortTo">
		/// The beacon port to.
		/// </param>
		/// <param name="targetIpList">
		/// The target ip list.
		/// </param>
		/// <param name="rules">
		/// The rules.
		/// </param>
		private void Init(
			string ipFrom, 
			int uDPListen, 
			int beaconPortFrom, 
			int tCPListen, 
			int uDPSend, 
			string ipTo, 
			int beaconPortTo, 
			string targetIpList, 
			IRuleSet rules)
		{
			this.Rules = rules;

			this.receiverCodec = new EpicsGateWayReceiverCodec(this);
			this.connectorCodec = new EpicsGateWayConnectorCodec(this);
			this.Statistic = new EpicsGateWayStatistics();

			// start listening for connection requests on 
			this.tcpListener = new TcpListener(new IPEndPoint(IPAddress.Parse(ipFrom), tCPListen));
			this.tcpListener.Start();

			this.Config.BeaconPortFrom = beaconPortFrom;
			this.Config.BeaconPortTo = beaconPortTo;
			this.Config.TCPListenPort = tCPListen;
			this.Config.UDPSendPort = uDPSend;
			this.Config.UDPListenPort = uDPListen;

			Trace.Write("I" + string.Format("Starting gateway {0}:{1} to ({2}):{3}.", ipFrom, uDPListen, targetIpList, uDPSend));

			this.Config.ServerList.Clear();
			if (targetIpList.Contains(","))
			{
				var ips = targetIpList.Split(',');
				foreach (var ip in ips)
				{
					this.Config.ServerList.Add(ip + ":" + uDPSend);
				}
			}
			else
			{
				this.Config.ServerList.Add(targetIpList + ":" + uDPSend);
			}

			// starting a Thread to grab new TCP-Connections and wrap them into a EpicsTCPServerConnection
			this.connectionGrabber = new Thread(this.GrabConnection);
			this.connectionGrabber.IsBackground = true;
			this.connectionGrabber.Start();

			this.ChannelSearcher = new Thread(this.HandleChannelSearch);
			this.ChannelSearcher.IsBackground = true;
			this.ChannelSearcher.Start();

			this.UdpConnFrom = new EpicsUDPGWConnection(
				this, this.receiverCodec, new IPEndPoint(IPAddress.Parse(ipFrom), this.Config.UDPListenPort));
			this.UdpConnTo = new EpicsUDPGWConnection(
				this, this.connectorCodec, new IPEndPoint(IPAddress.Parse(ipTo), this.Config.BeaconPortTo));

			this.BeaconHandler = new Beaconizer(this.UdpConnFrom, this.Config.BeaconPortFrom);
			ThreadPool.QueueUserWorkItem(
				delegate
					{
						Thread.Sleep(5 * 60 * 1000);
						this.Inited = true;
						this.CareBeacons();
					});
		}
	}
}