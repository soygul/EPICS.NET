// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsClient.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Client
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Net;
	using System.Net.Sockets;
	using System.Threading;

	using Epics.Base;
	using Epics.Base.Constants;

	/// <summary>
	/// Epics Factory to create Channels.
	///   <example>
	/// EpicsClient client = new EpicsClient();<br/>
	///     EpicsChannel channel = client.CreateChannel("TEST:CHANNEL");<br/>
	///     object value = channel.Get();<br/>
	///     channel.Put(100);<br/>
	///     client.Dispose();
	///   </example>
	/// </summary>
	public class EpicsClient : IDisposable
	{
		/// <summary>
		///   The not disposing.
		/// </summary>
		internal bool notDisposing = true;

		/// <summary>
		///   EpicsClient Configuration, allows runtime modification.
		/// </summary>
		public EpicsClientConfig Config { get; private set; }

		/// <summary>
		///   The cid counter.
		/// </summary>
		private static int CIDCounter;

		/// <summary>
		///   The udp connection.
		/// </summary>
		private EpicsClientUDPConnection UDPConnection;

		/// <summary>
		///   The udp beacon connection.
		/// </summary>
		private EpicsClientUDPConnection UDPBeaconConnection;

		/// <summary>
		///   Gets Codec.
		/// </summary>
		internal EpicsClientCodec Codec { get; private set; }

		/// <summary>
		///   Exception container for asynchron threads (if something strange happens and you don't get a exception, check here)
		/// </summary>
		public EpicsExceptionList ExceptionContainer { get; private set; }

		/// <summary>
		///   The cid channels.
		/// </summary>
		internal Dictionary<int, EpicsChannel> cidChannels = new Dictionary<int, EpicsChannel>();

		/// <summary>
		///   The conn counter.
		/// </summary>
		private int connCounter;

		/// <summary>
		///   The connections.
		/// </summary>
		private readonly Dictionary<string, EpicsClientTCPConnection> Connections =
			new Dictionary<string, EpicsClientTCPConnection>();

		/// <summary>
		///   The search names.
		/// </summary>
		private readonly Queue<KeyValuePair<int, string>> searchNames = new Queue<KeyValuePair<int, string>>();

		/// <summary>
		///   The searching channels.
		/// </summary>
		internal Dictionary<int, string> searchingChannels = new Dictionary<int, string>();

		// beacon catcher
		/// <summary>
		///   The beacon collection.
		/// </summary>
		internal Dictionary<string, DateTime> beaconCollection = new Dictionary<string, DateTime>();

		/// <summary>
		///   The search per ioc.
		/// </summary>
		internal Dictionary<string, Queue<KeyValuePair<int, string>>> searchPerIoc =
			new Dictionary<string, Queue<KeyValuePair<int, string>>>();

		/// <summary>
		///   The beacon checker.
		/// </summary>
		private readonly Thread beaconChecker;

		/// <summary>
		///   The startup running.
		/// </summary>
		private bool startupRunning = true;

		/// <summary>
		///   The search handler.
		/// </summary>
		private readonly Thread searchHandler;

		/// <summary>
		///   Initializes a new instance of the <see cref = "EpicsClient" /> class. 
		///   Constructor
		/// </summary>
		public EpicsClient()
		{
			this.ExceptionContainer = new EpicsExceptionList();
			this.Config = new EpicsClientConfig();

			this.Codec = new EpicsClientCodec(this);
			this.UDPConnection = new EpicsClientUDPConnection(this);

			try
			{
				this.UDPBeaconConnection = new EpicsClientUDPConnection(this, this.Config.UDPBeaconPort);

				this.beaconChecker = new Thread(this.checkBeacons);
				this.beaconChecker.IsBackground = true;
				this.beaconChecker.Start();

				// The first 30 seconds is init-phase. New iocs will not be handled.
				ThreadPool.QueueUserWorkItem(
					delegate
						{
#if DEBUG
							Thread.Sleep(250);
							this.startupRunning = false;
#else
	
	// Thread.Sleep(31 * 1000);
							Thread.Sleep(1 * 1000);
											 this.startupRunning = false;
#endif
						});
			}
			catch (SocketException ex)
			{
				this.ExceptionContainer.Add(new Exception("BeaconHandling deactivated due to blocked Port"));
			}
			catch (Exception e)
			{
				this.ExceptionContainer.Add(e);
			}

			this.searchHandler = new Thread(this.SearchHandler);
			this.searchHandler.IsBackground = true;
			this.searchHandler.Start();
		}

		/// <summary>
		/// Creates a new EpicsChannel, even if the channel already exists.<br/>
		///   The Channel is at the Moment it's given back still in creation. So if the Channel doesn't exist will not be checked.<br/>
		///   Further the Channel will be created as his by configuration defined type.
		/// </summary>
		/// <param name="ChannelName">
		/// Name of the Channel
		/// </param>
		/// <returns>
		/// !new! EpicsChannel
		/// </returns>
		public EpicsChannel CreateChannel(string ChannelName)
		{
			var Channel = new EpicsChannel(ChannelName.Trim(), ++CIDCounter, this);
			lock (this.cidChannels)
			{
				this.cidChannels.Add((int)Channel.CID, Channel);
			}

			this.SearchChannel((int)Channel.CID, ChannelName);

			return Channel;
		}

		/// <summary>
		/// Creates a new generic <see cref="EpicsChannel"/>, even if the channel already exists.<br/>
		///   The Channel is at the Moment it's given back still in creation. So if the Channel doesn't exist will not be checked.<br/>
		///   Further the Channel will be created to serve the requested <typeparamref name="DataType"/>.
		/// </summary>
		/// <typeparam name="DataType">
		/// </typeparam>
		/// <param name="ChannelName">
		/// Name of the Channel.
		/// </param>
		/// <returns>
		/// !new! generic <see cref="EpicsChannel"/>
		/// </returns>
		public EpicsChannel<DataType> CreateChannel<DataType>(string ChannelName)
		{
			var Channel = new EpicsChannel<DataType>(ChannelName.Trim(), ++CIDCounter, this);
			lock (this.cidChannels)
			{
				this.cidChannels.Add((int)Channel.CID, Channel);
			}

			this.SearchChannel((int)Channel.CID, ChannelName);

			return Channel;
		}

		/// <summary>
		/// The get server connection.
		/// </summary>
		/// <param name="reqConnection">
		/// The req connection.
		/// </param>
		/// <returns>
		/// </returns>
		internal EpicsClientTCPConnection GetServerConnection(IPEndPoint reqConnection)
		{
			if (!this.Connections.ContainsKey(reqConnection.ToString()))
			{
				lock (this.Connections)
				{
					this.connCounter++;
					this.Connections.Add(reqConnection.ToString(), new EpicsClientTCPConnection(reqConnection, this));
				}
			}

			return this.Connections[reqConnection.ToString()];
		}

		/// <summary>
		/// The search channel.
		/// </summary>
		/// <param name="CID">
		/// The cid.
		/// </param>
		/// <param name="ChannelName">
		/// The channel name.
		/// </param>
		internal void SearchChannel(int CID, string ChannelName)
		{
			if (!this.searchingChannels.ContainsKey(CID))
			{
				lock (this.searchingChannels)
				{
					this.searchingChannels.Add(CID, ChannelName);
				}

				lock (this.searchNames)
				{
					this.searchNames.Enqueue(new KeyValuePair<int, string>(CID, ChannelName));
				}
			}
		}

		/// <summary>
		/// The found channel.
		/// </summary>
		/// <param name="CID">
		/// The cid.
		/// </param>
		internal void FoundChannel(int CID)
		{
			if (this.searchingChannels.ContainsKey(CID))
			{
				lock (this.searchingChannels)
				{
					this.searchingChannels.Remove(CID);
				}
			}
		}

		/// <summary>
		/// The search not found channel.
		/// </summary>
		private void SearchNotFoundChannel()
		{
			if (this.searchingChannels.Count > 0)
			{
				lock (this.searchingChannels)
				{
					lock (this.searchNames)
					{
						foreach (var searchEntry in this.searchingChannels)
						{
							this.searchNames.Enqueue(searchEntry);
						}
					}
				}
			}
		}

		/// <summary>
		/// The search handler.
		/// </summary>
		private void SearchHandler()
		{
			var i = 0;
			var dropIocSearch = new List<string>();

			do
			{
				if (this.searchNames.Count > 0)
				{
					this.UDPConnection.Send(this.Codec.searchPackage(this.searchNames));
				}

				if (this.searchPerIoc.Count > 0)
				{
					foreach (var pair in this.searchPerIoc)
					{
						var parts = pair.Key.Split(':');
						var iep = new IPEndPoint(IPAddress.Parse(parts[0]), this.Config.UDPIocPort);

						this.UDPConnection.AnswerTo(this.Codec.searchPackage(pair.Value), iep);

						if (pair.Value.Count == 0)
						{
							dropIocSearch.Add(pair.Key);
						}
					}

					lock (this.searchPerIoc)
					{
						foreach (var key in dropIocSearch)
						{
							this.searchPerIoc.Remove(key);
						}
					}

					dropIocSearch.Clear();
				}

				// every 10th loop check if we have nothing to search for. And if so add all not found Channels again
				// to the searchloop
				if ((i++) % this.Config.channelSearchResearchCounter == 0)
				{
					if (this.searchNames.Count == 0)
					{
						this.SearchNotFoundChannel();
					}
				}

				Thread.Sleep(this.Config.ChannelSearchInterval);
			}
			while (this.notDisposing);
		}

		/// <summary>
		/// The drop epics connection.
		/// </summary>
		/// <param name="key">
		/// The key.
		/// </param>
		internal void dropEpicsConnection(string key)
		{
			if (this.Connections.ContainsKey(key))
			{
				lock (this.Connections)
				{
					this.Connections.Remove(key);
				}
			}
		}

		/// <summary>
		/// The drop epics channel.
		/// </summary>
		/// <param name="CID">
		/// The cid.
		/// </param>
		internal void dropEpicsChannel(int CID)
		{
			if (this.cidChannels.ContainsKey(CID))
			{
				lock (this.cidChannels)
				{
					this.cidChannels.Remove(CID);
				}
			}
		}

		/// <summary>
		/// The check beacons.
		/// </summary>
		internal void checkBeacons()
		{
			var diedIoc = new List<string>();
			var beaconTimeOut = new TimeSpan(0, 0, 30);
			while (this.notDisposing)
			{
				lock (this.beaconCollection)
				{
					foreach (var pair in this.beaconCollection)
					{
						if (pair.Value < (DateTime.Now - beaconTimeOut))
						{
							diedIoc.Add(pair.Key);
						}
					}

					foreach (var key in diedIoc)
					{
						this.beaconCollection.Remove(key);
					}
				}

				diedIoc.Clear();
				Thread.Sleep(15000);
			}
		}

		/// <summary>
		/// The add ioc beaconed.
		/// </summary>
		/// <param name="iep">
		/// The iep.
		/// </param>
		internal void addIocBeaconed(EndPoint iep)
		{
			lock (this.beaconCollection)
			{
				this.beaconCollection.Add(iep.ToString(), DateTime.Now);
			}

			if (this.startupRunning)
			{
				return;
			}

			lock (this.searchPerIoc)
			{
				if (this.searchPerIoc.ContainsKey(iep.ToString()))
				{
					return;
				}
				else
				{
					this.searchPerIoc.Add(iep.ToString(), new Queue<KeyValuePair<int, string>>());
					lock (this.searchingChannels)
					{
						try
						{
							foreach (var pair in this.searchingChannels)
							{
								this.searchPerIoc[iep.ToString()].Enqueue(pair);
							}
						}
						catch (Exception e)
						{
							Trace.Write("WBeaconException: " + e);
						}
					}
				}
			}
		}

#if DEBUG

		/// <summary>
		/// DEBUG: Returns the Current Status of the EpicsClient including some statistics
		/// </summary>
		/// <returns>
		/// The get status.
		/// </returns>
		public string GetStatus()
		{
			var status = string.Empty;

			int conChannel = 0, disChannel = 0, reqChannel = 0;

			foreach (var pair in this.cidChannels)
			{
				if (pair.Value.Status == ChannelStatus.CONNECTED)
				{
					conChannel++;
				}
				else if (pair.Value.Status == ChannelStatus.DISCONNECTED)
				{
					disChannel++;
				}
				else
				{
					reqChannel++;
				}
			}

			status += string.Format(
				"TotalChannel:{0}({1}Conn,{2}Disc,{3}Req)", this.cidChannels.Count, conChannel, disChannel, reqChannel);
			status += string.Format(
				",searchBuffer:{0},ChannelsNotFound:{1}", this.searchNames.Count, this.searchingChannels.Count);
			status += string.Format(",CIDCounter:{0}", CIDCounter);
			status += string.Format(",OpenConnections:{0}/{1}", this.Connections.Count, this.connCounter);

			return status;
		}

#endif

		/// <summary>
		/// Cleans up all associated things and the client himself.<br/>
		///   Drops all channels and disconnects all channel.
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

			var copyCidChannels = this.cidChannels;
			foreach (var pair in copyCidChannels)
			{
				pair.Value.Dispose();
			}

			copyCidChannels.Clear();

			foreach (var pair in this.Connections)
			{
				pair.Value.Dispose();
			}

			this.Connections.Clear();

			this.UDPConnection.Dispose();
			this.UDPConnection = null;
		}
	}
}