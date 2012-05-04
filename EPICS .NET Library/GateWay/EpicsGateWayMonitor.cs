// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsGateWayMonitor.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.GateWay
{
	#region Using Directives

	using System;
	using System.Collections.Generic;

	#endregion

	/// <summary>
	/// The epics gate way monitor.
	/// </summary>
	internal class EpicsGateWayMonitor : IDisposable
	{
		/// <summary>
		///   The channel.
		/// </summary>
		private readonly EpicsGateWayIocChannel Channel;

		/// <summary>
		///   The gate way.
		/// </summary>
		private readonly EpicsGateWay GateWay;

		/// <summary>
		///   The key.
		/// </summary>
		private readonly long Key;

		/// <summary>
		///   The listeners.
		/// </summary>
		private readonly Dictionary<uint, MonitorHandle> Listeners = new Dictionary<uint, MonitorHandle>();

		/// <summary>
		///   The last monitor message header.
		/// </summary>
		private byte[] lastMonitorMessageHeader;

		/// <summary>
		///   The last monitor message payload.
		/// </summary>
		private byte[] lastMonitorMessagePayload;

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsGateWayMonitor"/> class.
		/// </summary>
		/// <param name="channel">
		/// The channel.
		/// </param>
		/// <param name="gateWay">
		/// The gate way.
		/// </param>
		/// <param name="key">
		/// The key.
		/// </param>
		/// <param name="header">
		/// The header.
		/// </param>
		/// <param name="payload">
		/// The payload.
		/// </param>
		internal EpicsGateWayMonitor(
			EpicsGateWayIocChannel channel, EpicsGateWay gateWay, long key, byte[] header, byte[] payload)
		{
			this.Channel = channel;
			this.GateWay = gateWay;
			this.Key = key;

			this.SubscriptionId = this.GateWay.RegisterNewMonitor(this);

			this.Channel.Connection.Send(
				this.GateWay.ConnectorCodec.createSubscriptionMessage(header, payload, this.SubscriptionId, this.Channel.IocChanId));
		}

		/// <summary>
		///   Gets or sets SubscriptionId.
		/// </summary>
		public uint SubscriptionId { get; set; }

		/// <summary>
		/// The dispose.
		/// </summary>
		public void Dispose()
		{
			this.Channel.Connection.Send(
				this.GateWay.ConnectorCodec.closeSubscriptionMessage(this.Channel.IocChanId, this.SubscriptionId));

			if (this.Listeners.Count > 0)
			{
				this.GateWay.Statistic.MonitorsRunning -= this.Listeners.Count;
			}

			if (this.GateWay.notDisposing)
			{
				this.GateWay.DropMonitor(this.SubscriptionId);
				this.Channel.DropMonitor(this.Key);
			}
		}

		/// <summary>
		/// The add listener.
		/// </summary>
		/// <param name="item">
		/// The item.
		/// </param>
		internal void AddListener(MonitorHandle item)
		{
			this.GateWay.Statistic.MonitorsRunning++;

			lock (this.Listeners)
			{
				this.Listeners.Add(item.SubscriptionId, item);
			}

			if (this.lastMonitorMessageHeader != null)
			{
				this.GateWay.TCPConnections[item.NetAddress].Send(
					this.GateWay.ReceiverCodec.monitorChangeMessage(
						this.SubscriptionId, item.ClientId, this.lastMonitorMessageHeader, this.lastMonitorMessagePayload));
			}
		}

		/// <summary>
		/// The forward message.
		/// </summary>
		/// <param name="header">
		/// The header.
		/// </param>
		/// <param name="payload">
		/// The payload.
		/// </param>
		internal void ForwardMessage(byte[] header, byte[] payload)
		{
			try
			{
				lock (this.Listeners)
				{
					this.lastMonitorMessageHeader = header;
					this.lastMonitorMessagePayload = payload;

					foreach (var pair in this.Listeners)
					{
						this.GateWay.TCPConnections[pair.Value.NetAddress].Send(
							this.GateWay.ReceiverCodec.monitorChangeMessage(pair.Value.SubscriptionId, pair.Value.ClientId, header, payload));
					}
				}
			}
			catch (Exception e)
			{
			}
		}

		/// <summary>
		/// The remove listener.
		/// </summary>
		/// <param name="SubscriptionId">
		/// The subscription id.
		/// </param>
		internal void RemoveListener(uint SubscriptionId)
		{
			this.GateWay.Statistic.MonitorsRunning--;

			lock (this.Listeners)
			{
				this.Listeners.Remove(SubscriptionId);
			}

			if (this.Listeners.Count == 0)
			{
				this.Dispose();
			}
		}
	}
}