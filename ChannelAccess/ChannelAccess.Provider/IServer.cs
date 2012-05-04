// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IServer.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.ChannelAccess.Provider
{
	using System;
	using System.Collections.Generic;

	public interface IServer
	{
		/// <summary>
		/// Gets a <see cref="IServerChannel{T}"/> reference based on the channel name.
		/// </summary>
		Dictionary<string, Channel> Channels { get; }

		IServerChannel<T> CreateChannel<T>(string channelName, T initialValue);

		IServerChannel<T> CreateChannel<T>(string channelName);

		IServerChannel<T> CreateChannel<T>(PredefinedChannel predefinedChannel);
	}
}
