// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsServerMonitor.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Server
{
	using System;

	using Epics.Base;
	using Epics.Base.Constants;

	internal class EpicsServerMonitor : IDisposable
	{
		private readonly EpicsServerChannel channel;
		private readonly int dataCount = 1;
		private readonly RecordProperty property;
		private readonly EpicsRecord record;
		private readonly int subscriptionId;
		private readonly EpicsType type;
		private MonitorMask monitorMask;

		private object lastValue;

		internal EpicsServerMonitor(
			EpicsRecord record, 
			RecordProperty property, 
			EpicsServerChannel channel, 
			EpicsType type, 
			int dataCount, 
			MonitorMask monitorMask, 
			int subscriptionId)
		{
			this.record = record;
			this.property = property;
			this.channel = channel;
			this.type = type;
			this.dataCount = dataCount;
			this.monitorMask = monitorMask;
			this.subscriptionId = subscriptionId;

			try
			{
				var val = this.record[this.property.ToString()];
				if (val == null)
				{
					val = 0;
				}

				var realData = NetworkByteConverter.objectToByte(val, this.type, this.record);
				this.channel.sendMonitorChange(
					this.subscriptionId, this.type, this.dataCount, EpicsTransitionStatus.ECA_NORMAL, realData);

				this.StartMonitor();
			}
			catch (Exception e)
			{
				this.channel.sendMonitorChange(
					this.subscriptionId, this.type, this.dataCount, EpicsTransitionStatus.ECA_ADDFAIL, new byte[0]);
			}
		}

		public void Dispose()
		{
			this.StopMonitor();
			this.channel.sendMonitorClose(this.subscriptionId, this.type);

			if (this.channel.NotDisposing)
			{
				this.channel.dropMonitor(this.subscriptionId);
			}
		}

		internal void StartMonitor()
		{
			this.record.PropertyChanged += this.RecordPropertyChanged;
		}

		internal void StopMonitor()
		{
			this.record.PropertyChanged -= this.RecordPropertyChanged;
		}

		private void RecordPropertyChanged(EpicsRecord sender, RecordProperty prop, object val, Setter setter)
		{
			if (prop == this.property)
			{
				var toUpdate = true;
				if (val is short || val is int || val is float || val is double)
				{
					var v = Convert.ToDouble(val);
					var l = Convert.ToDouble(this.lastValue);
					if (Math.Abs(v - l) > sender.MDEL)
					{
						this.lastValue = val;
					}
					else
					{
						toUpdate = false;
					}
				}
				else if (val is string)
				{
					if ((string)val == (string)this.lastValue)
					{
						toUpdate = false;
					}
					else
					{
						this.lastValue = val;
					}
				}
				else
				{
					if (val.ToString() == this.lastValue.ToString())
					{
						toUpdate = false;
					}
					else
					{
						this.lastValue = val;
					}
				}

				if (toUpdate)
				{
					this.channel.sendMonitorChange(
						this.subscriptionId, 
						this.type, 
						this.dataCount, 
						EpicsTransitionStatus.ECA_NORMAL, 
						NetworkByteConverter.objectToByte(val, this.type, this.record));
				}
			}
		}
	}
}