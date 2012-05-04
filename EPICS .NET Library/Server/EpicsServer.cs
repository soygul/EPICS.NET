// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsServer.cs" company="Turkish Accelerator Center">
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
	using System.Net.Sockets;
	using System.Threading;

	using Epics.Base;

	#endregion

	/// <summary>
	/// Allows to publish variables to epics as records/channel
	/// </summary>
	public class EpicsServer : IDisposable
	{
		// Disposing Status

		/// <summary>
		///   The beacon handler.
		/// </summary>
		internal Beaconizer beaconHandler;

		/// <summary>
		///   The channel list.
		/// </summary>
		internal Dictionary<int, EpicsServerChannel> channelList = new Dictionary<int, EpicsServerChannel>();

		/// <summary>
		///   The not disposing.
		/// </summary>
		internal bool notDisposing = true;

		// Configuration

		/// <summary>
		///   The open connection.
		/// </summary>
		internal Dictionary<string, EpicsServerTCPConnection> openConnection =
			new Dictionary<string, EpicsServerTCPConnection>();

		// Record-Variables
		/// <summary>
		///   The record list.
		/// </summary>
		internal Dictionary<string, EpicsRecord> recordList = new Dictionary<string, EpicsRecord>();

		/// <summary>
		///   The udp connection.
		/// </summary>
		internal EpicsServerUDPConnection udpConnection;

		/// <summary>
		///   The lock init.
		/// </summary>
		private readonly object lockInit = new object();

		// monitors
		/// <summary>
		///   The monitored record list.
		/// </summary>
		private readonly Dictionary<EpicsRecord, int> monitoredRecordList = new Dictionary<EpicsRecord, int>();

		/// <summary>
		///   The config.
		/// </summary>
		private EpicsServerConfig config;

		/// <summary>
		///   The connection grabber.
		/// </summary>
		private Thread connectionGrabber;

		/// <summary>
		///   The has monitors.
		/// </summary>
		private bool hasMonitors;

		// Channels

		/// <summary>
		///   The is init.
		/// </summary>
		private bool isInit;

		/// <summary>
		///   The record monitor caller.
		/// </summary>
		private Thread recordMonitorCaller;

		/// <summary>
		///   The sid.
		/// </summary>
		private int sid = 10;

		/// <summary>
		///   The tcp listener.
		/// </summary>
		private TcpListener tcpListener;

		/// <summary>
		///   Initializes a new instance of the <see cref = "EpicsServer" /> class.
		/// </summary>
		public EpicsServer()
		{
			this.config = new EpicsServerConfig();
			this.Codec = new EpicsServerCodec(this);
			this.config.ConfigChanged += this.config_ConfigChanged;
		}

		/// <summary>
		///   Gets Config.
		/// </summary>
		public EpicsServerConfig Config
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
		///   Gets Codec.
		/// </summary>
		internal EpicsServerCodec Codec { get; private set; }

		/// <summary>
		///   Gets or sets SID.
		/// </summary>
		internal int SID
		{
			get
			{
				return this.sid;
			}

			set
			{
				this.sid = value;
			}
		}

		/// <summary>
		/// Creates a Epics Record which allows to publish arrays. Important: do give a singular type not the array-kind
		/// </summary>
		/// <returns>
		/// EpicsArrayRecord published to the EpicsNetwork
		/// </returns>
		public EpicsArrayRecord<dataType> GetEpicsArrayRecord<dataType>(string recordName, int size)
		{
			this.Init();
			if (typeof(dataType).IsArray)
			{
				throw new Exception("Use single type, not array (example: GetEpicsArrayRecord<int>)");
			}

			if (this.recordList.ContainsKey(recordName))
			{
				if (this.recordList[recordName].GetType() != typeof(EpicsArrayRecord<dataType>))
				{
					throw new Exception(
						"Record already exists as different DataType ('" + this.recordList[recordName].GetType() + "')");
				}

				return (EpicsArrayRecord<dataType>)this.recordList[recordName];
			}
			else
			{
				var newRecord = new EpicsArrayRecord<dataType>(recordName, this, size);
				this.recordList.Add(recordName, newRecord);
				return newRecord;
			}
		}

		/// <summary>
		/// Returns a EpicsRecord Simulation. Will reuse a already existing or if not existing create a new Record.
		/// </summary>
		/// <typeparam name="dataType">
		/// Type of value the record serves (int,short,string,double,short)
		/// </typeparam>
		/// <param name="recordName">
		/// Name of the Record
		/// </param>
		/// <returns>
		/// EpicsRecord for manipulation ready
		/// </returns>
		public EpicsRecord<dataType> GetEpicsRecord<dataType>(string recordName)
		{
			this.Init();
			if (this.recordList.ContainsKey(recordName))
			{
				if (this.recordList[recordName].GetValueType() != typeof(dataType))
				{
					throw new Exception(
						"Record already exists as different DataType ('" + this.recordList[recordName].GetValueType() + "')");
				}

				return (EpicsRecord<dataType>)this.recordList[recordName];
			}
			else
			{
				var newRecord = new EpicsRecord<dataType>(recordName, this);
				this.recordList.Add(recordName, newRecord);
				return newRecord;
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

			// remove all Monitors
			lock (this.monitoredRecordList)
			{
				this.monitoredRecordList.Clear();
			}

			// remove all records
			lock (this.recordList)
			{
				foreach (var pair in this.recordList)
				{
					pair.Value.Dispose();
				}
			}

			// close UDP-Listener
			this.udpConnection.Dispose();

			// close TCP-Listener
			this.tcpListener.Stop();

			// close all connections (will also close all channels!)
			lock (this.openConnection)
			{
				foreach (var pair in this.openConnection)
				{
					pair.Value.Dispose();
				}
			}
		}

		/// <summary>
		/// The create epics channel.
		/// </summary>
		/// <param name="clientId">
		/// The client id.
		/// </param>
		/// <param name="sender">
		/// The sender.
		/// </param>
		/// <param name="channelName">
		/// The channel name.
		/// </param>
		/// <returns>
		/// </returns>
		internal EpicsServerChannel CreateEpicsChannel(int clientId, EndPoint sender, string channelName)
		{
			this.sid++;
			lock (this.channelList)
			{
				this.channelList.Add(
					this.sid, new EpicsServerChannel(this, this.sid, clientId, channelName, this.openConnection[sender.ToString()]));
			}

			return this.channelList[this.sid];
		}

		/// <summary>
		/// The drop epics channel.
		/// </summary>
		/// <param name="clientId">
		/// The client id.
		/// </param>
		internal void DropEpicsChannel(int clientId)
		{
			lock (this.channelList)
			{
				this.channelList.Remove(clientId);
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
			lock (this.openConnection)
			{
				if (this.openConnection.ContainsKey(endpoint))
				{
					this.openConnection.Remove(endpoint);
				}
			}
		}

		/// <summary>
		/// The drop epics record.
		/// </summary>
		/// <param name="recordName">
		/// The record name.
		/// </param>
		internal void DropEpicsRecord(string recordName)
		{
			lock (this.recordList)
			{
				if (this.recordList.ContainsKey(recordName))
				{
					this.recordList.Remove(recordName);
				}
			}
		}

		/// <summary>
		/// The set monitor.
		/// </summary>
		/// <param name="record">
		/// The record.
		/// </param>
		/// <param name="scanInterval">
		/// The scan interval.
		/// </param>
		internal void SetMonitor(EpicsRecord record, int scanInterval)
		{
			if (scanInterval == 0)
			{
				if (this.monitoredRecordList.ContainsKey(record))
				{
					lock (this.monitoredRecordList)
					{
						this.monitoredRecordList.Remove(record);
						if (this.monitoredRecordList.Count == 0)
						{
							this.hasMonitors = false;
						}
					}
				}
			}
			else
			{
				lock (this.monitoredRecordList)
				{
					if (this.monitoredRecordList.ContainsKey(record))
					{
						this.monitoredRecordList[record] = scanInterval;
					}
					else
					{
						this.monitoredRecordList.Add(record, scanInterval);
						if (this.monitoredRecordList.Count == 1)
						{
							this.hasMonitors = true;

							// prepare for calling the Monitors
							this.recordMonitorCaller = new Thread(this.HandleMonitors);
							this.recordMonitorCaller.IsBackground = true;
							this.recordMonitorCaller.Priority = ThreadPriority.Highest;
							this.recordMonitorCaller.Start();

							// recordScanTriggerThread.Start();
						}
					}
				}
			}
		}

		// <summary>
		/// <summary>
		/// The grab connection.
		/// </summary>
		private void GrabConnection()
		{
			while (this.notDisposing)
			{
				try
				{
					var newConnectionSocket = this.tcpListener.AcceptSocket();

					lock (this.openConnection)
					{
						if (!this.openConnection.ContainsKey(newConnectionSocket.RemoteEndPoint.ToString()))
						{
							this.openConnection.Add(
								newConnectionSocket.RemoteEndPoint.ToString(), new EpicsServerTCPConnection(newConnectionSocket, this));
						}
					}
				}
				catch
				{
					return;
				}
			}
		}

		/// <summary>
		/// The handle monitors.
		/// </summary>
		private void HandleMonitors()
		{
			var loops = 0;
			var loopInterval = this.config.MonitorExecutionInterval;
			var timeToSleep = new TimeSpan(0, 0, 0, 0, loopInterval);
			var watch = new Stopwatch();
			var idealTime = new TimeSpan(0);
			TimeSpan tmpVal;
			var nbLoops = 0;
			var totTime = new Stopwatch();
			totTime.Start();

			while (this.hasMonitors)
			{
				loops += loopInterval;
				nbLoops++;
				idealTime += timeToSleep;

				watch.Reset();
				watch.Start();
				lock (this.monitoredRecordList)
				{
					foreach (var pair in this.monitoredRecordList)
					{
						if (loops % pair.Value == 0)
						{
							pair.Key.scanTrigger();
						}
					}
				}

				watch.Stop();

				// if (timeToSleep >= watch.Elapsed) Thread.Sleep(timeToSleep - watch.Elapsed);
				tmpVal = idealTime - totTime.Elapsed;
				if (tmpVal.TotalMilliseconds > 0L)
				{
					Thread.Sleep(tmpVal);
				}

				if (nbLoops > 10000)
				{
					idealTime = new TimeSpan();
					nbLoops = 0;
					totTime.Stop();
					totTime.Reset();
					totTime.Start();
				}
			}
		}

		/// <summary>
		/// The init.
		/// </summary>
		private void Init()
		{
			lock (this.lockInit)
			{
				if (this.isInit)
				{
					return;
				}

				this.isInit = true;

				// starting TCP-Listener to retreive connections
				this.tcpListener = new TcpListener(IPAddress.Any, this.config.TCPPort);
				this.tcpListener.Start();

				// starting a Thread to grab new TCP-Connections and wrap them into a EpicsTCPServerConnection
				this.connectionGrabber = new Thread(this.GrabConnection);
				this.connectionGrabber.IsBackground = true;
				this.connectionGrabber.Start();

				// opening the UDP-Listener
				this.udpConnection = new EpicsServerUDPConnection(this);
				if (this.Config.ProducingBeacon)
				{
					this.beaconHandler = new Beaconizer(this.udpConnection, this.Config.BeaconPort);
				}
			}
		}

		/// <summary>
		/// The config_ config changed.
		/// </summary>
		/// <param name="propertyName">
		/// The property name.
		/// </param>
		private void config_ConfigChanged(string propertyName)
		{
			lock (this.lockInit)
			{
				if (!this.isInit)
				{
					return;
				}

				if (propertyName == "UDPPort")
				{
					this.beaconHandler.Dispose();
					this.udpConnection.Dispose();

					this.udpConnection = null;
					this.udpConnection = new EpicsServerUDPConnection(this);
					this.beaconHandler = new Beaconizer(this.udpConnection, this.Config.BeaconPort);
				}
				else if (propertyName == "TCPPort")
				{
					this.tcpListener.Stop();
					this.tcpListener = new TcpListener(IPAddress.Any, this.config.TCPPort);
					this.tcpListener.Start();

					this.connectionGrabber = new Thread(this.GrabConnection);
					this.connectionGrabber.IsBackground = true;
					this.connectionGrabber.Start();
				}
				else if (propertyName == "ProducingBeacon")
				{
					if (this.Config.ProducingBeacon)
					{
						this.beaconHandler = new Beaconizer(this.udpConnection, this.Config.BeaconPort);
					}
					else
					{
						this.beaconHandler.Dispose();
					}
				}
			}
		}
	}
}