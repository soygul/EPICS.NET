// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsClientConfig.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Client
{
	#region Using Directives

	using System;
	using System.Configuration;
	using System.Net;
	using System.Net.NetworkInformation;

	using Epics.Base;

	#endregion

	/// <summary>
	/// Configuration for EpicsClient.<br/>
	///   Does define default values, load from app.config (e#%PropertyName%) and allows afterwards
	///   configuration changes on runtime.
	///   <example>
	/// EpicsClient client = new EpicsClient();<br/>
	///     client.Config.UDPBufferSize = 56000;<br/>
	///     client.Config.ServerList.Add("127.0.0.1");<br/>
	///     client.Config.GetTimeout = 1000;
	///   </example>
	/// </summary>
	public class EpicsClientConfig
	{
		/// <summary>
		///   The channel search research counter.
		/// </summary>
		internal int channelSearchResearchCounter = 200;

		/// <summary>
		///   The server list.
		/// </summary>
		private readonly ConfigList serverList = new ConfigList();

		/// <summary>
		///   The channel search interval.
		/// </summary>
		private int channelSearchInterval = 100;

		/// <summary>
		///   The channel search max package size.
		/// </summary>
		private int channelSearchMaxPackageSize = 500;

		/// <summary>
		///   The channel search resarch interval.
		/// </summary>
		private int channelSearchResarchInterval = 20000;

		/// <summary>
		///   The connect timeout.
		/// </summary>
		private int connectTimeout = 5000;

		/// <summary>
		///   The connection echo timeout.
		/// </summary>
		private int connectionEchoTimeout = 5000;

		/// <summary>
		///   The get timeout.
		/// </summary>
		private int getTimeout = 5000;

		/// <summary>
		///   The put timeout.
		/// </summary>
		private int putTimeout = 5000;

		/// <summary>
		///   The t cp buffer size.
		/// </summary>
		private int tCPBufferSize = 524288;

		/// <summary>
		///   The t cp max send size.
		/// </summary>
		private int tCPMaxSendSize = 8192;

		/// <summary>
		///   The u dp beacon port.
		/// </summary>
		private int uDPBeaconPort = 5065;

		/// <summary>
		///   The u dp buffer size.
		/// </summary>
		private int uDPBufferSize = 524288;

		/// <summary>
		///   The u dp ioc port.
		/// </summary>
		private int uDPIocPort = 5064;

		/// <summary>
		///   Initializes a new instance of the <see cref = "EpicsClientConfig" /> class.
		/// </summary>
		internal EpicsClientConfig()
		{
			this.serverList.ConfigChanged += this.serverList_ConfigChanged;

			var interfaces = NetworkInterface.GetAllNetworkInterfaces();
			long longIp = 0;
			IPAddress ip = null;

			foreach (var iface in interfaces)
			{
				var prop = iface.GetIPProperties();
				if (prop.UnicastAddresses.Count == 0)
				{
					continue;
				}

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

				// add gateways
				this.serverList.Add(ip + ":5066");

				// add servers
				this.serverList.Add(ip + ":5064");
			}

			// load appSettings
			if (ConfigurationSettings.AppSettings["e#GetTimeout"] != null)
			{
				this.getTimeout = Convert.ToInt32(ConfigurationSettings.AppSettings["e#GetTimeout"]);
			}

			if (ConfigurationSettings.AppSettings["e#PutTimeout"] != null)
			{
				this.putTimeout = Convert.ToInt32(ConfigurationSettings.AppSettings["e#PutTimeout"]);
			}

			if (ConfigurationSettings.AppSettings["e#ConnectTimeout"] != null)
			{
				this.connectTimeout = Convert.ToInt32(ConfigurationSettings.AppSettings["e#ConnectTimeout"]);
			}

			if (ConfigurationSettings.AppSettings["e#ConnectionEchoTimeout"] != null)
			{
				this.connectionEchoTimeout = Convert.ToInt32(ConfigurationSettings.AppSettings["e#ConnectionEchoTimeout"]);
			}

			if (ConfigurationSettings.AppSettings["e#ChannelSearchInterval"] != null)
			{
				this.channelSearchInterval = Convert.ToInt32(ConfigurationSettings.AppSettings["e#ChannelSearchInterval"]);
			}

			if (ConfigurationSettings.AppSettings["e#ChannelSearchMaxPackageSize"] != null)
			{
				this.channelSearchMaxPackageSize =
					Convert.ToInt32(ConfigurationSettings.AppSettings["e#ChannelSearchMaxPackageSize"]);
			}

			if (ConfigurationSettings.AppSettings["e#ChannelSearchResarchInterval"] != null)
			{
				this.channelSearchResarchInterval =
					Convert.ToInt32(ConfigurationSettings.AppSettings["e#ChannelSearchResarchInterval"]);
			}

			if (ConfigurationSettings.AppSettings["e#TCPMaxSendSize"] != null)
			{
				this.tCPMaxSendSize = Convert.ToInt32(ConfigurationSettings.AppSettings["e#TCPMaxSendSize"]);
			}

			if (ConfigurationSettings.AppSettings["e#UDPBufferSize"] != null)
			{
				this.uDPBufferSize = Convert.ToInt32(ConfigurationSettings.AppSettings["e#UDPBufferSize"]);
			}

			if (ConfigurationSettings.AppSettings["e#TCPBufferSize"] != null)
			{
				this.tCPBufferSize = Convert.ToInt32(ConfigurationSettings.AppSettings["e#TCPBufferSize"]);
			}

			if (ConfigurationSettings.AppSettings["e#UDPBeaconPort"] != null)
			{
				this.uDPBeaconPort = Convert.ToInt32(ConfigurationSettings.AppSettings["e#UDPBeaconPort"]);
			}

			if (ConfigurationSettings.AppSettings["e#UDPIocPort"] != null)
			{
				this.uDPIocPort = Convert.ToInt32(ConfigurationSettings.AppSettings["e#UDPIocPort"]);
			}

			if (ConfigurationSettings.AppSettings["e#ServerList"] != null)
			{
				this.serverList.Clear();
				if (ConfigurationSettings.AppSettings["e#ServerList"].Contains(";"))
				{
					var addresses = ConfigurationSettings.AppSettings["e#ServerList"].Split(';');
					foreach (var address in addresses)
					{
						if (address.Contains(":"))
						{
							this.serverList.Add(address);
						}
						else
						{
							this.serverList.Add(address + ":5064");
							this.serverList.Add(address + ":5066");
						}
					}
				}
				else
				{
					if (ConfigurationSettings.AppSettings["e#ServerList"].Contains(":"))
					{
						this.serverList.Add(ConfigurationSettings.AppSettings["e#ServerList"]);
					}
					else
					{
						this.serverList.Add(ConfigurationSettings.AppSettings["e#ServerList"] + ":5064");
						this.serverList.Add(ConfigurationSettings.AppSettings["e#ServerList"] + ":5066");
					}
				}
			}
		}

		// config Change Monitors

		/// <summary>
		/// The config changed delegate.
		/// </summary>
		/// <param name="propertyName">
		/// The property name.
		/// </param>
		internal delegate void ConfigChangedDelegate(string propertyName);

		/// <summary>
		///   The config changed.
		/// </summary>
		internal event ConfigChangedDelegate ConfigChanged;

		/// <summary>
		///   time in miliseconds which has to past before another UDP-Channel-Search-Packet is send. 
		///   !Attention! if you are sending to fast, it can kill the gateway or the IOC. 100ms is strongly suggested!
		/// </summary>
		public int ChannelSearchInterval
		{
			get
			{
				return this.channelSearchInterval;
			}

			set
			{
				this.channelSearchInterval = value;
				this.channelSearchResearchCounter =
					(int)Math.Ceiling((double)(this.channelSearchResarchInterval / this.channelSearchResarchInterval));
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("ChannelSearchInterval");
				}
			}
		}

		/// <summary>
		///   amount of channel-requests which is send in a UDP-Channel-Search-Packet.
		///   !Attention! if you are sending to high numbers, it can kill the gateway or the IOC. 300 is strongly suggested!
		/// </summary>
		public int ChannelSearchMaxPackageSize
		{
			get
			{
				return this.channelSearchMaxPackageSize;
			}

			set
			{
				this.channelSearchMaxPackageSize = value;
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("ChannelSearchMaxPackageSize");
				}
			}
		}

		/// <summary>
		///   Time in miliseconds before it will search again for not found channels
		/// </summary>
		public int ChannelSearchResarchInterval
		{
			get
			{
				return this.channelSearchResarchInterval;
			}

			set
			{
				this.channelSearchResarchInterval = value;
				this.channelSearchResearchCounter =
					(int)Math.Ceiling((double)(this.channelSearchResarchInterval / this.channelSearchResarchInterval));
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("ChannelSearchResarchInterval");
				}
			}
		}

		/// <summary>
		///   Time which is allowed to pass for a connection to the ioc to be established.
		///   includes finding the correct channel.
		/// </summary>
		public int ConnectTimeout
		{
			get
			{
				return this.connectTimeout;
			}

			set
			{
				this.connectTimeout = value;
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("ConnectTimeout");
				}
			}
		}

		/// <summary>
		///   timeinterval in milisecond between echo messages are send to established server connections to check
		///   their availability.
		/// </summary>
		public int ConnectionEchoTimeout
		{
			get
			{
				return this.connectionEchoTimeout;
			}

			set
			{
				this.connectionEchoTimeout = value;
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("ConnectionEchoTimeout");
				}
			}
		}

		/// <summary>
		///   Time which is allowed to pass between sending the request to get a value and the receiving.
		///   Does not include the time to connect
		/// </summary>
		public int GetTimeout
		{
			get
			{
				return this.getTimeout;
			}

			set
			{
				this.getTimeout = value;
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("GetTimeout");
				}
			}
		}

		/// <summary>
		///   Time which is allowed to pass between sending the request to put a value and the receiving.
		///   Does not include the time to connect
		/// </summary>
		public int PutTimeout
		{
			get
			{
				return this.putTimeout;
			}

			set
			{
				this.putTimeout = value;
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("PutTimeout");
				}
			}
		}

		/// <summary>
		///   List of server or broadcast-addresses which will be asked for a channel
		/// </summary>
		public ConfigList ServerList
		{
			get
			{
				return this.serverList;
			}

			private set
			{
			}
		}

		/// <summary>
		///   Socket Buffersize for TCP Connection
		/// </summary>
		public int TCPBufferSize
		{
			get
			{
				return this.tCPBufferSize;
			}

			set
			{
				this.tCPBufferSize = value;
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("TCPBufferSize");
				}
			}
		}

		/// <summary>
		///   maximum of bytes send in one tcp packet
		/// </summary>
		public int TCPMaxSendSize
		{
			get
			{
				return this.tCPMaxSendSize;
			}

			set
			{
				this.tCPMaxSendSize = value;
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("TCPMaxSendSize");
				}
			}
		}

		/// <summary>
		///   UDP Port on which the client has to listen for beacons
		/// </summary>
		public int UDPBeaconPort
		{
			get
			{
				return this.uDPBeaconPort;
			}

			set
			{
				this.uDPBeaconPort = value;
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("UDPBeaconPort");
				}
			}
		}

		/// <summary>
		///   Socket Buffersize for UDP Connection
		/// </summary>
		public int UDPBufferSize
		{
			get
			{
				return this.uDPBufferSize;
			}

			set
			{
				this.uDPBufferSize = value;
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("UDPBufferSize");
				}
			}
		}

		/// <summary>
		///   UDP Port on which the IOC should listen for udp-Requests
		/// </summary>
		public int UDPIocPort
		{
			get
			{
				return this.uDPIocPort;
			}

			set
			{
				this.uDPIocPort = value;
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("UDPIocPort");
				}
			}
		}

		/// <summary>
		/// The server list_ config changed.
		/// </summary>
		private void serverList_ConfigChanged()
		{
			if (this.ConfigChanged != null)
			{
				this.ConfigChanged("ServerList");
			}
		}
	}
}