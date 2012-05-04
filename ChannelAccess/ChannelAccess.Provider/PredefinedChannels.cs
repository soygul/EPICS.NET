// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PredefinedChannels.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.ChannelAccess.Provider
{
	using System;
	using System.Drawing;

	public class PredefinedChannel
	{
		public object InitialValue;

		internal PredefinedChannel(string prefix, string suffix, Type dataType)
		{
			this.Prefix = prefix;
			this.Suffix = suffix;
			this.DataType = dataType;
		}

		internal PredefinedChannel(string prefix, string suffix, Type dataType, object initialValue)
			: this(prefix, suffix, dataType)
		{
			this.InitialValue = initialValue;
		}

		public string ChannelName { get; set; }

		public Type DataType { get; set; }

		public string Prefix { get; set; }

		public int SectorId { get; set; }

		public string Suffix { get; set; }

		/// <summary>
		/// Utility function to create channel names from channel name prefix, ID, and suffix.
		/// </summary>
		public static string ChannelNameBuilder(string channelNamePrefix, int sectorID, string channelNameSuffix)
		{
			string sectorIdText;

			if (sectorID < 10)
			{
				sectorIdText = "0" + sectorID;
			}
			else
			{
				sectorIdText = sectorID.ToString();
			}

			return channelNamePrefix + sectorIdText + ":" + channelNameSuffix;
		}

		public PredefinedChannel SetId(int sectorId)
		{
			this.SectorId = sectorId;
			this.ChannelName = ChannelNameBuilder(this.Prefix, this.SectorId, this.Suffix);
			return this;
		}
	}

	/// <summary>
	/// Static data container used in place of magic strings in channel naming.
	/// </summary>
	public static class PredefinedChannels
	{
		public static class Aperture
		{
			private static readonly string BasePrefix = "Aperture";

			public static PredefinedChannel In
			{
				get
				{
					return new PredefinedChannel(BasePrefix, "In", typeof(int));
				}
			}

			public static PredefinedChannel Radius
			{
				get
				{
					return new PredefinedChannel(BasePrefix, "Radius", typeof(double));
				}
			}
		}

		public static class BeamPositionMonitor
		{
			private static readonly string BasePrefix = "BeamPositionMonitor";

			public static PredefinedChannel Current
			{
				get
				{
					return new PredefinedChannel(BasePrefix, "Current", typeof(double));
				}
			}

			public static PredefinedChannel XPosition
			{
				get
				{
					return new PredefinedChannel(BasePrefix, "XPosition", typeof(double));
				}
			}

			public static PredefinedChannel YPosition
			{
				get
				{
					return new PredefinedChannel(BasePrefix, "YPosition", typeof(double));
				}
			}
		}

		public static class BeamProfileMonitor
		{
			private static readonly string BasePrefix = "BeamProfileMonitor";

			public static PredefinedChannel CameraOn
			{
				get
				{
					return new PredefinedChannel(BasePrefix, "CameraOn", typeof(int));
				}
			}

			public static PredefinedChannel ViewScreenIn
			{
				get
				{
					return new PredefinedChannel(BasePrefix, "ViewScreenIn", typeof(int));
				}
			}

			public static PredefinedChannel LaserOn
			{
				get
				{
					return new PredefinedChannel(BasePrefix, "LaserOn", typeof(int));
				}
			}

			public static PredefinedChannel Image
			{
				get
				{
					return new PredefinedChannel(BasePrefix, "Image", typeof(string));
				}
			}
		}

		public static class BeamLossMonitor
		{
			private static readonly string BasePrefix = "BeamLossMonitor";

			public static PredefinedChannel BeamLoss
			{
				get
				{
					return new PredefinedChannel(BasePrefix, "BeamLoss", typeof(int));
				}
			}
		}

		public static class Cathode
		{
			private static readonly string BasePrefix = "Cathode";

			public static PredefinedChannel BeamOn
			{
				get
				{
					return new PredefinedChannel(BasePrefix, "BeamOn", typeof(int));
				}
			}

			public static PredefinedChannel Current
			{
				get
				{
					return new PredefinedChannel(BasePrefix, "Current", typeof(double));
				}
			}

			public static PredefinedChannel Temperature
			{
				get
				{
					return new PredefinedChannel(BasePrefix, "Temperature", typeof(double));
				}
			}
		}

		public static class GateValve
		{
			private static readonly string BasePrefix = "GateValve";

			public static PredefinedChannel Closed
			{
				get
				{
					return new PredefinedChannel(BasePrefix, "Closed", typeof(int));
				}
			}
		}

		public static class HighVoltageSupply
		{
			private static readonly string BasePrefix = "HighVoltageSupply";

			public static PredefinedChannel On
			{
				get
				{
					return new PredefinedChannel(BasePrefix, "On", typeof(int));
				}
			}

			public static PredefinedChannel MeasuredVoltage
			{
				get
				{
					return new PredefinedChannel(BasePrefix, "MeasuredVoltage", typeof(double));
				}
			}

			public static PredefinedChannel PresetVoltage
			{
				get
				{
					return new PredefinedChannel(BasePrefix, "PresetVoltage", typeof(double));
				}
			}
		}

		public static class Solenoid
		{
			private static readonly string BasePrefix = "Solenoid";

			public static PredefinedChannel Current
			{
				get
				{
					return new PredefinedChannel(BasePrefix, "Current", typeof(double));
				}
			}

			public static PredefinedChannel CurrentOn
			{
				get
				{
					return new PredefinedChannel(BasePrefix, "CurrentOn", typeof(int));
				}
			}
		}

		public static class SteeringMagnet
		{
			private static readonly string BasePrefix = "SteeringMagnet";

			public static PredefinedChannel XCurrent
			{
				get
				{
					return new PredefinedChannel(BasePrefix, "XCurrent", typeof(double));
				}
			}

			public static PredefinedChannel XCurrentOn
			{
				get
				{
					return new PredefinedChannel(BasePrefix, "XCurrentOn", typeof(int));
				}
			}

			public static PredefinedChannel YCurrent
			{
				get
				{
					return new PredefinedChannel(BasePrefix, "YCurrent", typeof(double));
				}
			}

			public static PredefinedChannel YCurrentOn
			{
				get
				{
					return new PredefinedChannel(BasePrefix, "YCurrentOn", typeof(int));
				}
			}
		}
		
		public static class Vacuum
		{
			private static readonly string BasePrefix = "Vacuum";

			public static PredefinedChannel On
			{
				get
				{
					return new PredefinedChannel(BasePrefix, "On", typeof(int));
				}
			}

			public static PredefinedChannel Pressure
			{
				get
				{
					return new PredefinedChannel(BasePrefix, "Pressure", typeof(double));
				}
			}
		}
	}
}
