// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsGateWayConfig.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.GateWay
{
	using Epics.Base;

	/// <summary>
	/// The epics gate way config.
	/// </summary>
	public class EpicsGateWayConfig
	{
		// config Change Monitors

		// config-values

		/// <summary>
		///   The beacon port from.
		/// </summary>
		private int beaconPortFrom = 5065;

		/// <summary>
		///   The beacon port to.
		/// </summary>
		private int beaconPortTo = 5065;

		/// <summary>
		///   The channel search interval.
		/// </summary>
		private int channelSearchInterval = 100;

		/// <summary>
		///   The channel search max package size.
		/// </summary>
		private int channelSearchMaxPackageSize = 300;

		/// <summary>
		///   The server list.
		/// </summary>
		private ConfigList serverList = new ConfigList();

		/// <summary>
		///   The t cp buffer size.
		/// </summary>
		private int tCPBufferSize = 524288;

		/// <summary>
		///   The t cp listen port.
		/// </summary>
		private int tCPListenPort;

		/// <summary>
		///   The t cp max send size.
		/// </summary>
		private int tCPMaxSendSize = 4096;

		/// <summary>
		///   The u dp buffer size.
		/// </summary>
		private int uDPBufferSize = 65536;

		/// <summary>
		///   The u dp listen port.
		/// </summary>
		private int uDPListenPort = 5064;

		/// <summary>
		///   The u dp send port.
		/// </summary>
		private int uDPSendPort = 5064;

		/// <summary>
		///   Initializes a new instance of the <see cref = "EpicsGateWayConfig" /> class.
		/// </summary>
		public EpicsGateWayConfig()
		{
			this.serverList.ConfigChanged += this.serverList_ConfigChanged;
		}

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
		///   Ports on which it shall listen and forward Beacons
		/// </summary>
		public int BeaconPortFrom
		{
			get
			{
				return this.beaconPortFrom;
			}

			set
			{
				this.beaconPortFrom = value;
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("BeaconPortFrom");
				}
			}
		}

		/// <summary>
		///   Ports on which it shall listen and forward Beacons
		/// </summary>
		public int BeaconPortTo
		{
			get
			{
				return this.beaconPortTo;
			}

			set
			{
				this.beaconPortTo = value;
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("BeaconPortTo");
				}
			}
		}

		/// <summary>
		///   Interval it sends new search packages
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
		///   List of Addresses to ask for Channels
		/// </summary>
		public ConfigList ServerList
		{
			get
			{
				return this.serverList;
			}

			set
			{
				this.serverList = value;
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("ServerList");
				}
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
		///   TCP Port to Listen for new Connections
		/// </summary>
		public int TCPListenPort
		{
			get
			{
				return this.tCPListenPort;
			}

			set
			{
				this.tCPListenPort = value;
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("TCPListenPort");
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
		///   Socket Buffersize for TCP Connection
		/// </summary>
		public int UDPListenPort
		{
			get
			{
				return this.uDPListenPort;
			}

			set
			{
				this.uDPListenPort = value;
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("UDPListenPort");
				}
			}
		}

		/// <summary>
		///   Socket Buffersize for TCP Connection
		/// </summary>
		public int UDPSendPort
		{
			get
			{
				return this.uDPSendPort;
			}

			set
			{
				this.uDPSendPort = value;
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("UDPSendPort");
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