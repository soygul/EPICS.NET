// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Client.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.ChannelAccess.EPICS
{
	using System;
	using System.Collections.Generic;

	using Epics.ChannelAccess.Provider;
	using Epics.Client;

	public class Client : IClient
	{
		private readonly EpicsClient epicsClient;

		public Client()
		{
			this.epicsClient = new EpicsClient();
			this.Channels = new Dictionary<string, Channel>();
		}

		/// <summary>
		/// Gets a <see cref="ClientChannel{T}"/> reference based on the channel name.
		/// </summary>
		public Dictionary<string, Channel> Channels { get; private set; }

		public IClientChannel<T> CreateChannel<T>(string channelName)
		{
			// Check to see if the channel already exists. If so, return the existing channel instance
			if (this.Channels.ContainsKey(channelName))
			{
				return (IClientChannel<T>)this.Channels[channelName];
			}
			else
			{
				var clientChannel = new ClientChannel<T>(this.epicsClient.CreateChannel<T>(channelName));
				this.Channels.Add(channelName, clientChannel);
				return clientChannel;
			}
		}

		public IClientChannel<T> CreateChannel<T>(string channelName, T initialValue)
		{
			// Check to see if the channel already exists. If so, return the existing channel instance
			if (this.Channels.ContainsKey(channelName))
			{
				return (IClientChannel<T>)this.Channels[channelName];
			}
			else
			{
				var clientChannel = new ClientChannel<T>(this.epicsClient.CreateChannel<T>(channelName), initialValue);
				this.Channels.Add(channelName, clientChannel);
				return clientChannel;
			}
		}

		public IClientChannel<T> CreateChannel<T>(string channelNamePrefix, int sectorId, string channelNameSuffix)
		{
			return this.CreateChannel<T>(PredefinedChannel.ChannelNameBuilder(channelNamePrefix, sectorId, channelNameSuffix));
		}

		public IClientChannel<T> CreateChannel<T>(string channelNamePrefix, int sectorId, string channelNameSuffix, T initialValue)
		{
			return this.CreateChannel(PredefinedChannel.ChannelNameBuilder(channelNamePrefix, sectorId, channelNameSuffix), initialValue);
		}

		/// <exception cref="ArgumentException"><c>ArgumentException</c>.</exception>
		public IClientChannel<T> CreateChannel<T>(PredefinedChannel predefinedChannel)
		{
			if (predefinedChannel.DataType != typeof(T))
			{
				throw new ArgumentException("Predefined channel value is: " + predefinedChannel.DataType
					+ ", but the supplied generic type parameter was: " + typeof(T).Name
					+ ". Generic type parameter and predefined channel data type should match.");
			}
			else if (predefinedChannel.InitialValue != null)
			{
				return this.CreateChannel(predefinedChannel.ChannelName, (T)predefinedChannel.InitialValue);
			}
			else
			{
				return CreateChannel<T>(predefinedChannel.ChannelName);
			}
		}
	}
}
