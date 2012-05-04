// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IServerChannel.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.ChannelAccess.Provider
{
	using System;

	public interface IServerChannel<T>
	{
		T Value { get; set; }

		/// <summary>
		/// First parameter T is the new value, second parameter T is the old value, third parameter is the evaluation result.
		/// If result is false, new value will not be appointed.
		/// </summary>
		IServerChannel<T> RegisterFilter(Func<T, T, bool> filter);
	}
}