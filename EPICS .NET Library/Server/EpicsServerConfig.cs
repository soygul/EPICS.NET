// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsServerConfig.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Server
{
	#region Using Directives

	using System;
	using System.Configuration;
	using System.Net;

	#endregion

	/// <summary>
	/// Configuration for EpicsServer. Does define default values, load from app.config (e#%PropertyName%) and allows afterwards
	///   configuration changes on runtime.
	/// </summary>
	public class EpicsServerConfig
	{
		/// <summary>
		///   The beacon interval.
		/// </summary>
		private int beaconInterval = 1000;

		/// <summary>
		///   The beacon port.
		/// </summary>
		private int beaconPort = 5065;

		/// <summary>
		///   The listen ip.
		/// </summary>
		private string listenIP = IPAddress.Any.ToString();

		/// <summary>
		///   The monitor execution interval.
		/// </summary>
		private int monitorExecutionInterval = 10;

		/// <summary>
		///   The producing beacon.
		/// </summary>
		private bool producingBeacon = true;

		/// <summary>
		///   The t cp buffer size.
		/// </summary>
		private int tCPBufferSize = 524288;

		/// <summary>
		///   The t cp max send size.
		/// </summary>
		private int tCPMaxSendSize = 4096;

		/// <summary>
		///   The t cp port.
		/// </summary>
		private int tCPPort = 5064;

		/// <summary>
		///   The u dp buffer size.
		/// </summary>
		private int uDPBufferSize = 65536;

		/// <summary>
		///   The u dp dest port.
		/// </summary>
		private int uDPDestPort = 5065;

		/// <summary>
		///   The u dp port.
		/// </summary>
		private int uDPPort = 5064;

		/// <summary>
		///   Initializes a new instance of the <see cref = "EpicsServerConfig" /> class.
		/// </summary>
		internal EpicsServerConfig()
		{
			if (ConfigurationSettings.AppSettings["eS#TCPPort"] != null)
			{
				this.tCPPort = Convert.ToInt32(ConfigurationSettings.AppSettings["eS#TCPPort"]);
			}

			if (ConfigurationSettings.AppSettings["eS#UDPPort"] != null)
			{
				this.uDPPort = Convert.ToInt32(ConfigurationSettings.AppSettings["eS#UDPPort"]);
			}

			if (ConfigurationSettings.AppSettings["eS#BeaconInterval"] != null)
			{
				this.beaconInterval = Convert.ToInt32(ConfigurationSettings.AppSettings["eS#BeaconInterval"]);
			}

			if (ConfigurationSettings.AppSettings["eS#ProducingBeacon"] != null)
			{
				this.producingBeacon = Convert.ToBoolean(ConfigurationSettings.AppSettings["eS#ProducingBeacon"]);
			}

			if (ConfigurationSettings.AppSettings["eS#TCPMaxSendSize"] != null)
			{
				this.tCPMaxSendSize = Convert.ToInt32(ConfigurationSettings.AppSettings["eS#TCPMaxSendSize"]);
			}

			if (ConfigurationSettings.AppSettings["eS#ListenIP"] != null)
			{
				this.listenIP = ConfigurationSettings.AppSettings["eS#ListenIP"];
			}

			if (ConfigurationSettings.AppSettings["eS#UDPBufferSize"] != null)
			{
				this.uDPBufferSize = Convert.ToInt32(ConfigurationSettings.AppSettings["eS#UDPBufferSize"]);
			}

			if (ConfigurationSettings.AppSettings["eS#TCPBufferSize"] != null)
			{
				this.tCPBufferSize = Convert.ToInt32(ConfigurationSettings.AppSettings["eS#TCPBufferSize"]);
			}

			if (ConfigurationSettings.AppSettings["eS#MonitorExecutionInterval"] != null)
			{
				this.monitorExecutionInterval = Convert.ToInt32(ConfigurationSettings.AppSettings["eS#MonitorExecutionInterval"]);
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
		///   Gets or sets BeaconInterval.
		/// </summary>
		public int BeaconInterval
		{
			get
			{
				return this.beaconInterval;
			}

			set
			{
				this.beaconInterval = value;
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("BeaconInterval");
				}
			}
		}

		/// <summary>
		///   Ports on which it shall listen and forward Beacons
		/// </summary>
		public int BeaconPort
		{
			get
			{
				return this.beaconPort;
			}

			set
			{
				this.beaconPort = value;
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("BeaconPort");
				}
			}
		}

		/// <summary>
		///   Gets or sets ListenIP.
		/// </summary>
		public string ListenIP
		{
			get
			{
				return this.listenIP;
			}

			set
			{
				this.listenIP = value;
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("ListenIP");
				}
			}
		}

		/// <summary>
		///   Miliseconds Interval for the MonitorInterval loop.<br />
		///   ScanIntervals on the Records which are not a multiple of this value, are ignored.
		/// </summary>
		public int MonitorExecutionInterval
		{
			get
			{
				return this.monitorExecutionInterval;
			}

			set
			{
				this.monitorExecutionInterval = value;
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("MonitorExecutionInterval");
				}
			}
		}

		/// <summary>
		///   Gets or sets a value indicating whether ProducingBeacon.
		/// </summary>
		public bool ProducingBeacon
		{
			get
			{
				return this.producingBeacon;
			}

			set
			{
				this.producingBeacon = value;
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("ProducingBeacon");
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
		///   Gets or sets TCPPort.
		/// </summary>
		public int TCPPort
		{
			get
			{
				return this.tCPPort;
			}

			set
			{
				this.tCPPort = value;
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("TCPPort");
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
		///   Gets or sets UDPDestPort.
		/// </summary>
		public int UDPDestPort
		{
			get
			{
				return this.uDPDestPort;
			}

			set
			{
				this.uDPDestPort = value;
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("UDPDestPort");
				}
			}
		}

		/// <summary>
		///   Gets or sets UDPPort.
		/// </summary>
		public int UDPPort
		{
			get
			{
				return this.uDPPort;
			}

			set
			{
				this.uDPPort = value;
				if (this.ConfigChanged != null)
				{
					this.ConfigChanged("UDPPort");
				}
			}
		}
	}
}