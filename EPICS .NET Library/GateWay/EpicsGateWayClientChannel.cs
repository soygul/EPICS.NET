// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsGateWayClientChannel.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.GateWay
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Net;
	using System.Threading;

	using Epics.Base.Constants;

	#endregion

	/// <summary>
	/// The epics gate way client channel.
	/// </summary>
	internal class EpicsGateWayClientChannel : IDisposable
	{
		/// <summary>
		///   The channel.
		/// </summary>
		private readonly EpicsGateWayIocChannel Channel;

		/// <summary>
		///   The channel name.
		/// </summary>
		private readonly string ChannelName;

		/// <summary>
		///   The client chan id.
		/// </summary>
		private readonly uint ClientChanId;

		/// <summary>
		///   The conn.
		/// </summary>
		private readonly EpicsTCPGWConnection Conn;

		// ServerId
		/// <summary>
		///   The gw client chan id.
		/// </summary>
		private readonly uint GWClientChanId;

		/// <summary>
		///   The gate way.
		/// </summary>
		private readonly EpicsGateWay GateWay;

		// ClientId

		// connected IP-Address identifier
		/// <summary>
		///   The ip address.
		/// </summary>
		private readonly string IpAddress;

		/// <summary>
		///   The ip address endpoint.
		/// </summary>
		private readonly EndPoint IpAddressEndpoint;

		/// <summary>
		///   The subscription memory.
		/// </summary>
		private readonly Dictionary<uint, EpicsGateWayMonitor> subscriptionMemory =
			new Dictionary<uint, EpicsGateWayMonitor>();

		/// <summary>
		///   The access.
		/// </summary>
		private AccessRights Access;

		/// <summary>
		///   The not disposing.
		/// </summary>
		private bool notDisposing = true;

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsGateWayClientChannel"/> class.
		/// </summary>
		/// <param name="gateWay">
		/// The gate way.
		/// </param>
		/// <param name="clientChanId">
		/// The client chan id.
		/// </param>
		/// <param name="channelName">
		/// The channel name.
		/// </param>
		/// <param name="ipAddress">
		/// The ip address.
		/// </param>
		/// <param name="gWClientChanId">
		/// The g w client chan id.
		/// </param>
		internal EpicsGateWayClientChannel(
			EpicsGateWay gateWay, uint clientChanId, string channelName, EndPoint ipAddress, uint gWClientChanId)
		{
			this.GateWay = gateWay;
			this.IpAddress = ipAddress.ToString();
			this.ClientChanId = clientChanId;
			this.GWClientChanId = gWClientChanId;
			this.ChannelName = channelName;

			try
			{
				this.Conn = this.GateWay.TCPConnections[this.IpAddress];
			}
			catch (Exception e)
			{
				Trace.Write("ITCP Connection for Channel was closed before Channel was established (IP: " + this.IpAddress + ")");
				this.Dispose();
				return;
			}

			try
			{
				this.Channel = this.GateWay.ChannelListIocName[channelName];
			}
			catch (Exception e)
			{
				// if it was not yet established, give it a second to find, if not it's to slow and it shall be found next time
				var timeout = new TimeSpan(0, 0, 1);
				var wtch = new Stopwatch();
				wtch.Start();
				while (this.GateWay.ChannelSearchNameWait.ContainsKey(channelName))
				{
					Thread.Sleep(0);

					if (wtch.Elapsed > timeout)
					{
						break;
					}
				}

				wtch.Stop();

				try
				{
					this.Channel = this.GateWay.ChannelListIocName[channelName];
				}
				catch
				{
					Trace.Write("IChannel creation to fast, finding to slow.");
					this.Conn.Send(this.GateWay.ReceiverCodec.channelCreationFailMessage(this.ClientChanId));
					this.Dispose();
					return;
				}
			}

			this.IpAddressEndpoint = ipAddress;
			var calcRights = this.GateWay.Rules.GetAccessRights(this.IpAddressEndpoint, this.Conn.Username, this.ChannelName);
			if (calcRights < this.Channel.AccessRights)
			{
				this.Access = calcRights;
			}
			else
			{
				this.Access = this.Channel.AccessRights;
			}

			gateWay.Rules.RulesChanged += this.Rules_RulesChanged;

			this.Conn.ConnectionStateChanged += this.EpicsGateWayClientChannel_ConnectionStateChanged;

			this.Conn.Send(
				this.GateWay.ReceiverCodec.channelCreatedMessage(
					this.ClientChanId, this.GWClientChanId, this.Access, this.Channel.CreateMessageAnswer));

			this.GateWay.Statistic.OpenClientChannels++;
		}

		/// <summary>
		/// The dispose.
		/// </summary>
		public void Dispose()
		{
			if (this.notDisposing)
			{
				this.notDisposing = false;
			}
			else
			{
				return;
			}

			this.GateWay.Statistic.OpenClientChannels--;

			if (this.GateWay.TCPConnections.ContainsKey(this.IpAddress))
			{
				this.GateWay.TCPConnections[this.IpAddress].Send(
					this.GateWay.ConnectorCodec.closeChannelMessage(this.ClientChanId, this.GWClientChanId));

				this.GateWay.TCPConnections[this.IpAddress].ConnectionStateChanged -=
					this.EpicsGateWayClientChannel_ConnectionStateChanged;
			}

			lock (this.subscriptionMemory)
			{
				foreach (var pair in this.subscriptionMemory)
				{
					pair.Value.RemoveListener(pair.Key);
				}

				this.subscriptionMemory.Clear();
			}

			this.GateWay.Rules.RulesChanged -= this.Rules_RulesChanged;

			this.GateWay.DropClientChannel(this.GWClientChanId);
		}

		/// <summary>
		/// The get async.
		/// </summary>
		/// <param name="cIoId">
		/// The c io id.
		/// </param>
		/// <param name="header">
		/// The header.
		/// </param>
		internal void GetAsync(uint cIoId, byte[] header)
		{
			if (this.Access == AccessRights.NoAccess || this.Access == AccessRights.WriteOnly)
			{
				return;
			}

			var jobId = ++this.GateWay.jobId;
			this.GateWay.RegisterNewJob(jobId, new JobHandle(this.IpAddress, this.ClientChanId, this.GWClientChanId, cIoId));

			this.Channel.GetAsync(jobId, header);
		}

		/// <summary>
		/// The put async.
		/// </summary>
		/// <param name="cIoId">
		/// The c io id.
		/// </param>
		/// <param name="header">
		/// The header.
		/// </param>
		/// <param name="payload">
		/// The payload.
		/// </param>
		internal void PutAsync(uint cIoId, byte[] header, byte[] payload)
		{
			if (this.Access == AccessRights.NoAccess || this.Access == AccessRights.ReadOnly)
			{
				return;
			}

			var jobId = ++this.GateWay.jobId;
			this.GateWay.RegisterNewJob(jobId, new JobHandle(this.IpAddress, this.ClientChanId, this.GWClientChanId, cIoId));

			this.Channel.PutAsync(jobId, header, payload);
		}

		/// <summary>
		/// The remove monitor.
		/// </summary>
		/// <param name="SubscriptionId">
		/// The subscription id.
		/// </param>
		internal void RemoveMonitor(uint SubscriptionId)
		{
			lock (this.subscriptionMemory)
			{
				this.subscriptionMemory[SubscriptionId].RemoveListener(SubscriptionId);
				this.subscriptionMemory.Remove(SubscriptionId);
			}
		}

		/// <summary>
		/// The start monitor.
		/// </summary>
		/// <param name="SubscriptionId">
		/// The subscription id.
		/// </param>
		/// <param name="header">
		/// The header.
		/// </param>
		/// <param name="payload">
		/// The payload.
		/// </param>
		internal void StartMonitor(uint SubscriptionId, byte[] header, byte[] payload)
		{
			if (this.Access == AccessRights.NoAccess || this.Access == AccessRights.WriteOnly)
			{
				return;
			}

			var monitor = this.Channel.GetMonitor(header, payload);
			monitor.AddListener(new MonitorHandle(this.IpAddress, this.ClientChanId, SubscriptionId));

			lock (this.subscriptionMemory)
			{
				this.subscriptionMemory.Add(SubscriptionId, monitor);
			}
		}

		/// <summary>
		/// The epics gate way client channel_ connection state changed.
		/// </summary>
		/// <param name="connected">
		/// The connected.
		/// </param>
		private void EpicsGateWayClientChannel_ConnectionStateChanged(bool connected)
		{
			if (connected)
			{
				return;
			}

			this.Dispose();
		}

		/// <summary>
		/// The rules_ rules changed.
		/// </summary>
		private void Rules_RulesChanged()
		{
			var calcRights = this.GateWay.Rules.GetAccessRights(this.IpAddressEndpoint, this.Conn.Username, this.ChannelName);
			if (calcRights < this.Channel.AccessRights)
			{
				this.Access = calcRights;
			}
			else
			{
				this.Access = this.Channel.AccessRights;
			}
		}
	}
}