// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IClient.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.ChannelAccess.Provider
{
	using System.Collections.Generic;

	public interface IClient
	{
		/// <summary>
		/// Gets a <see cref="IClientChannel{T}"/> reference based on the channel name.
		/// </summary>
		Dictionary<string, Channel> Channels { get; }
		
		IClientChannel<T> CreateChannel<T>(string channelNamePrefix, int sectorId, string channelNameSuffix, T initialValue);

		IClientChannel<T> CreateChannel<T>(string channelNamePrefix, int sectorId, string channelNameSuffix);

		IClientChannel<T> CreateChannel<T>(string channelName, T initialValue);

		IClientChannel<T> CreateChannel<T>(string channelName);

		IClientChannel<T> CreateChannel<T>(PredefinedChannel predefinedChannel);
	}
}
