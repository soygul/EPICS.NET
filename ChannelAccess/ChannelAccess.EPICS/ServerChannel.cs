// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServerChannel.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.ChannelAccess.EPICS
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Epics.ChannelAccess.Provider;
	using Epics.Server;

	public class ServerChannel<T> : Channel, IServerChannel<T>
	{
		private readonly ChannelValueChangedDelegate channelValueChangedDelegate;
		private readonly EpicsRecord<T> record;
		private T value;

		/// <summary>
		/// First parameter T is the new value, second parameter T is the old value, third parameter is the evaluation result.
		/// If result is false, new value will not be appointed.
		/// </summary>
		private List<Func<T, T, bool>> filters;

		internal ServerChannel(EpicsRecord<T> epicsRecord, T initialValue)
		{
			this.record = epicsRecord;

			this.record.PropertyChanged += (sendingEpicsRecord, recordProperty, newValue, localOrExternalSetter) =>
			{
				// Bypassing the default EPICS .NET v1.x self-tracking (property changes) beahvior to be compatible with v2.x
				if (localOrExternalSetter == Setter.external)
				{
					if (recordProperty == RecordProperty.VAL)
					{
						this.value = (T)newValue;

						if (this.channelValueChangedDelegate != null)
						{
							this.channelValueChangedDelegate(this.value);
						}
					}
				}
			};

			if (!EqualityComparer<T>.Default.Equals(this.value, initialValue))
			{
				this.Value = initialValue;
			}

			// Bugfix for Epics 1.x channels with NaN values
			/*if (!EqualityComparer<T>.Default.Equals(this.record.VAL, initialValue))
			{
				this.recordValue.Value = initialValue;
			}*/
		}

		internal ServerChannel(EpicsRecord<T> epicsRecord)
			: this(epicsRecord, default(T))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ServerChannel{T}"/> class. 
		/// </summary>
		/// <param name="channelValueChangedDelegate">Callback delegate to inform the instantiating object about the changes
		/// in the channel value. Changes to the channel value made by the instantiating object will not be notified back,
		/// which prevents a infinite loop. Note that this won't prevent the subscriber channel of getting informed about
		/// the change in the channel value.</param>
		internal ServerChannel(EpicsRecord<T> epicsRecord, ChannelValueChangedDelegate channelValueChangedDelegate)
			: this(epicsRecord)
		{
			this.channelValueChangedDelegate = channelValueChangedDelegate;
		}

		internal delegate void ChannelValueChangedDelegate(T newValue);

		public T Value
		{
			get
			{
				return this.value;
			}

			set
			{
				if (this.EvaluateFilters(this.value, value))
				{
					// Update the channel value after filtering
					this.record.VAL = value; // ToDo: Make sure that this is Async -or- network put operation will take ages!
				}

				this.value = value;
			}
		}

		/// <summary>
		/// First parameter T is the new value, second parameter T is the old value, third parameter is the evaluation result.
		/// If result is false, new value will not be appointed.
		/// </summary>
		public IServerChannel<T> RegisterFilter(Func<T, T, bool> filter)
		{
			if (this.filters == null)
			{
				this.filters = new List<Func<T, T, bool>>();
			}

			this.filters.Add(filter);
			return this;
		}

		private bool EvaluateFilters(T newValue, T oldValue)
		{
			if (this.filters != null && this.filters.Any(filter => !filter(newValue, oldValue)))
			{
				return false;
			}

			// Default filters
			// 'this.value == value' filter is by default
			if (EqualityComparer<T>.Default.Equals(newValue, oldValue))
			{
				return false;
			}

			return true;
		}
	}
}
