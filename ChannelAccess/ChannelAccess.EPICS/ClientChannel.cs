// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientChannel.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.ChannelAccess.EPICS
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Windows.Forms;

	using Epics.ChannelAccess.Provider;
	using Epics.Client;

	using MiscUtil;

	public class ClientChannel<T> : Channel, IClientChannel<T>
	{
		/// <summary>
		/// Makes sure that MonitorChanged events get called on the Main thread. This is required by the data binding to function properly.
		/// </summary>
		/// <remarks>It may be required to call asyncOperation.OperationCompleted() after Post() but for now, it is omitted.</remarks>
		private readonly AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(null);

		private readonly EpicsChannel<T> channel;

		private bool isBusy;

		private T value;

		private List<Func<T, T, bool>> filters;

		internal ClientChannel(EpicsChannel<T> channel)
		{
			this.channel = channel;
			// ToDo: this.value = this.channel.Get(); // This is used to get the initial value from the channel
			this.channel.MonitorChanged += (sender, newValue) =>
				{
					var oldValue = this.value;
					this.value = newValue;

					if (this.EvaluateFilters(newValue, oldValue))
					{
						// Make sure that all the updates occur on the UI thread
						this.asyncOperation.Post(
							state =>
							{
								// Update listeners through this.Changed
								if (this.Changed != null)
								{
									this.Changed(newValue);
								}

								// Update UI through INotifyPropertyChanged
								if (this.PropertyChanged != null)
								{
									this.PropertyChanged(this, new PropertyChangedEventArgs("Value"));
								}
							},
							null);
					}
				};
		}

		internal ClientChannel(EpicsChannel<T> channel, T initialValue)
			: this(channel)
		{
			this.value = initialValue;
		}

		/// <summary>
		/// Event to be raised when the private value field of the public property of the channel changes. Notifies other binding 
		/// object of the change in the property value.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		public event Action<T> Changed;

		/// <summary>
		/// Gets or sets the internal data field 'private T <see cref="value"/>'. This property is meant to be used by the data binding
		/// objects.
		/// </summary>
		/// <remarks>Name of this property "Value" should remain the same for data binding to function properly as it is used
		/// by <c>PropertyChangedEventArgs("Value")</c> at the constructor (<see cref="ClientChannel{T}"/>) of this class.</remarks>
		public T Value
		{
			get
			{
				return this.value;
			}

			set
			{
				// ToDo: Set cadvanced filters for the comparator here, note that 'this.value == value' filter is by default which is not desired.
				// These default filters should be optional with an aditional contructor like ClientChannel(EpicsChannel<T> channel, Filters.Default)
				// Also implement +-1% filter where new value is only published if the change is greated than 1% (via Filters delegate)
				if (!EqualityComparer<T>.Default.Equals(this.value, value))
				{
					if (this.isBusy)
					{
						MessageBox.Show("The channel: " + this.channel.ChannelName + " is already queued for change.", "Information");
					}
					else
					{
						this.isBusy = true;
						this.value = value;

						// Actually below function doesn't exist in the library and I don't vant to use reflections to get the type of T for performance reasons
						this.channel.PutAsyncGeneric(this.value, (sender, succeeded) =>
						{
							if (succeeded)
							{
								this.isBusy = false;
							}
							else
							{
								MessageBox.Show("Put operation failed for channel: " + this.channel.ChannelName, "Warning");
								this.channel.GetAsync((senderChannel, newValue) => { this.value = (T)newValue; this.isBusy = false; });
							}
						});
					}
				}
			}
		}

		public IClientChannel<T> RegisterFilter(Func<T, T, bool> filter)
		{
			if (this.filters == null)
			{
				this.filters = new List<Func<T, T, bool>>();
			}

			this.filters.Add(filter);
			return this;
		}

		private T publishedValueDelta;
		public IClientChannel<T> RegisterPercentageFilter(T publishAfterPercentageChange)
		{
			// Check to see if T is numeric so can have some sort of a percentage change
			if (this.publishedValueDelta is double || this.publishedValueDelta is int || this.publishedValueDelta is short || this.publishedValueDelta is float)
			{
				return this.RegisterFilter((newValue, oldValue) =>
					{
						// Using pure generic math here
						this.publishedValueDelta = Operator.Add(this.publishedValueDelta, Operator.Subtract(newValue, oldValue));
						var change = Operator.Divide(Operator.MultiplyAlternative(this.publishedValueDelta, 100), oldValue);
						if (Operator.GreaterThanOrEqual(change, publishAfterPercentageChange) || Operator.LessThanOrEqual(change, Operator.Negate(publishAfterPercentageChange)))
						{
							this.publishedValueDelta = default(T);
							return true;
						}
						else
						{
							return false;
						}
					});
			}
			else
			{
				return this;
			}
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
