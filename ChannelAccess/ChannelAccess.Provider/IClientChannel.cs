// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IClientChannel.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.ChannelAccess.Provider
{
	using System;
	using System.ComponentModel;

	public interface IClientChannel<T> : INotifyPropertyChanged
	{
		event Action<T> Changed;

		/// <summary>
		/// Gets or sets the current value (data) of the channel.
		/// </summary>
		/// <remarks>Name of this property "Value" should remain the same for data binding to function properly as it is used
		/// by <c>PropertyChangedEventArgs("Value")</c> at the constructors of implementing types.</remarks>
		T Value { get; set; }

		/// <summary>
		/// First parameter T is the new value, second parameter T is the old value, third parameter is the evaluation result.
		/// If result is false, new value will not be appointed.
		/// </summary>
		IClientChannel<T> RegisterFilter(Func<T, T, bool> filter);

		IClientChannel<T> RegisterPercentageFilter(T publishAfterPercentageChange);
	}
}
