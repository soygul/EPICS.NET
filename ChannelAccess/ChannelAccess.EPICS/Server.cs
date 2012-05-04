// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Server.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.ChannelAccess.EPICS
{
	using System;
	using System.Collections.Generic;

	using Epics.ChannelAccess.Provider;
	using Epics.Server;

	public class Server : IServer
	{
		private readonly EpicsServer epicsServer;

		public Server()
		{
			this.epicsServer = new EpicsServer();
			this.Channels = new Dictionary<string, Channel>();
		}

		/// <summary>
		/// Gets a <see cref="ServerChannel{T}"/> reference based on the channel name.
		/// </summary>
		public Dictionary<string, Channel> Channels { get; private set; }
		
		public IServerChannel<T> CreateChannel<T>(string channelName, T initialValue)
		{
			// Check to see if the channel already exists. If so, return the existing channel instance
			if (this.Channels.ContainsKey(channelName))
			{
				return (IServerChannel<T>)this.Channels[channelName];
			}
			else
			{
				var serverChannel = new ServerChannel<T>(this.epicsServer.GetEpicsRecord<T>(channelName));
				this.Channels.Add(channelName, serverChannel);

				if (!EqualityComparer<T>.Default.Equals(initialValue, default(T)))
				{
					serverChannel.Value = initialValue;
				}

				return serverChannel;
			}
		}

		public IServerChannel<T> CreateChannel<T>(string channelName)
		{
			return this.CreateChannel(channelName, default(T));
		}

		public IServerChannel<T> CreateChannel<T>(PredefinedChannel predefinedChannel)
		{
			if (predefinedChannel.DataType != typeof(T))
			{
				throw new ArgumentException("Predefined channel value is: " + predefinedChannel.DataType
					+ ", but the supplied generic type parameter was: " + typeof(T).Name
					+ ". Generic type parameter and predefined channel data type should match.");
			}
			else
			{
				return CreateChannel<T>(predefinedChannel.ChannelName);
			}
		}
	}
}
