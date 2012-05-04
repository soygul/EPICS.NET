// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsGateWayIocChannel.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.GateWay
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Net;
	using System.Threading;

	using Epics.Base.Constants;

	#endregion

	/// <summary>
	/// The epics gate way ioc channel.
	/// </summary>
	internal class EpicsGateWayIocChannel : IDisposable
	{
		/// <summary>
		///   The gate way.
		/// </summary>
		private readonly EpicsGateWay GateWay;

		/// <summary>
		///   The monitors.
		/// </summary>
		private readonly Dictionary<long, EpicsGateWayMonitor> Monitors = new Dictionary<long, EpicsGateWayMonitor>();

		/// <summary>
		///   The wait for established.
		/// </summary>
		private readonly AutoResetEvent waitForEstablished = new AutoResetEvent(false);

		/// <summary>
		///   The client chan id.
		/// </summary>
		private uint ClientChanId;

		/// <summary>
		///   The udp requester.
		/// </summary>
		private EndPoint UdpRequester;

		/// <summary>
		///   The create message answer.
		/// </summary>
		private byte[] createMessageAnswer;

		// ServerId
		/// <summary>
		///   The ioc chan id.
		/// </summary>
		private uint iocChanId;

		/// <summary>
		///   The ioc net.
		/// </summary>
		private EpicsTCPGWConnection iocNet;

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsGateWayIocChannel"/> class.
		/// </summary>
		/// <param name="channelName">
		/// The channel name.
		/// </param>
		/// <param name="gwIocId">
		/// The gw ioc id.
		/// </param>
		/// <param name="gw">
		/// The gw.
		/// </param>
		/// <param name="udpRequester">
		/// The udp requester.
		/// </param>
		/// <param name="clientChanId">
		/// The client chan id.
		/// </param>
		internal EpicsGateWayIocChannel(
			string channelName, uint gwIocId, EpicsGateWay gw, EndPoint udpRequester, uint clientChanId)
		{
			this.CreatedAt = DateTime.Now;
			this.GateWay = gw;
			this.GWIocChanId = gwIocId;
			this.ChannelName = channelName;

			this.UdpRequester = udpRequester;
			this.ClientChanId = clientChanId;

			this.GateWay.Statistic.OpenServerChannels++;
		}

		/// <summary>
		///   Gets or sets AccessRights.
		/// </summary>
		public AccessRights AccessRights { get; internal set; }

		/// <summary>
		///   Gets ChannelName.
		/// </summary>
		public string ChannelName { get; private set; }

		/// <summary>
		///   Gets or sets Connection.
		/// </summary>
		public EpicsTCPGWConnection Connection
		{
			get
			{
				return this.iocNet;
			}

			set
			{
				if (this.iocNet == null)
				{
					this.iocNet = value;
					this.iocNet.Send(this.GateWay.ConnectorCodec.startChannelMessage(this.ChannelName, this.GWIocChanId));
					this.iocNet.ConnectionStateChanged += this.iocNet_ConnectionStateChanged;

					if (this.UdpRequester != null)
					{
						this.GateWay.UdpConnFrom.Send(
							this.GateWay.ReceiverCodec.channelFoundMessage(this.ClientChanId), (IPEndPoint)this.UdpRequester);

						this.ClientChanId = 0;
						this.UdpRequester = null;
					}
				}
			}
		}

		/// <summary>
		///   Gets or sets CreateMessageAnswer.
		/// </summary>
		public byte[] CreateMessageAnswer
		{
			get
			{
				if (this.createMessageAnswer != null)
				{
					return this.createMessageAnswer;
				}
				else
				{
					if (this.waitForEstablished.WaitOne(5000))
					{
						return this.createMessageAnswer;
					}
					else
					{
						return new byte[16];
					}
				}
			}

			set
			{
				this.createMessageAnswer = value;
				this.waitForEstablished.Set();
			}
		}

		// creationTime
		/// <summary>
		///   Gets CreatedAt.
		/// </summary>
		public DateTime CreatedAt { get; private set; }

		/// <summary>
		///   Gets GWIocChanId.
		/// </summary>
		public uint GWIocChanId { get; private set; }

		/// <summary>
		///   Gets or sets IocChanId.
		/// </summary>
		public uint IocChanId
		{
			get
			{
				return this.iocChanId;
			}

			set
			{
				this.iocChanId = value;
			}
		}

		/// <summary>
		/// The dispose.
		/// </summary>
		public void Dispose()
		{
			if (this.Connection != null && this.Connection.Connected)
			{
				this.Connection.Send(this.GateWay.ConnectorCodec.closeChannelMessage(this.GWIocChanId, this.iocChanId));
			}

			this.GateWay.Statistic.OpenServerChannels--;

			try
			{
				lock (this.Monitors)
				{
					foreach (var pair in this.Monitors)
					{
						pair.Value.Dispose();
					}

					this.Monitors.Clear();
				}
			}
			catch
			{
				if (this.Monitors.Count > 0)
				{
					this.Monitors.Clear();
				}
			}

			if (this.GateWay.notDisposing)
			{
				this.GateWay.DropIocChannel(this.GWIocChanId);
			}

			if (this.iocNet != null)
			{
				this.iocNet.ConnectionStateChanged -= this.iocNet_ConnectionStateChanged;
			}
		}

		/// <summary>
		/// The drop monitor.
		/// </summary>
		/// <param name="key">
		/// The key.
		/// </param>
		internal void DropMonitor(long key)
		{
			lock (this.Monitors)
			{
				this.Monitors.Remove(key);
			}
		}

		// variables about who requested this channel at first

		/// <summary>
		/// The get async.
		/// </summary>
		/// <param name="jobId">
		/// The job id.
		/// </param>
		/// <param name="header">
		/// The header.
		/// </param>
		internal void GetAsync(uint jobId, byte[] header)
		{
			this.iocNet.Send(this.GateWay.ConnectorCodec.getMessage(header, jobId, this.iocChanId));
		}

		/// <summary>
		/// The get monitor.
		/// </summary>
		/// <param name="header">
		/// The header.
		/// </param>
		/// <param name="payload">
		/// The payload.
		/// </param>
		/// <returns>
		/// </returns>
		internal EpicsGateWayMonitor GetMonitor(byte[] header, byte[] payload)
		{
			var key = new byte[8];
			Buffer.BlockCopy(header, 4, key, 0, 4);
			Buffer.BlockCopy(payload, 6, key, 4, 2);

			var Key = BitConverter.ToInt64(key, 0);

			lock (this.Monitors)
			{
				if (!this.Monitors.ContainsKey(Key))
				{
					this.Monitors.Add(Key, new EpicsGateWayMonitor(this, this.GateWay, Key, header, payload));
				}
			}

			return this.Monitors[Key];
		}

		/// <summary>
		/// The put async.
		/// </summary>
		/// <param name="jobId">
		/// The job id.
		/// </param>
		/// <param name="header">
		/// The header.
		/// </param>
		/// <param name="payload">
		/// The payload.
		/// </param>
		internal void PutAsync(uint jobId, byte[] header, byte[] payload)
		{
			this.iocNet.Send(this.GateWay.ConnectorCodec.putMessage(header, payload, jobId, this.iocChanId));
		}

		/// <summary>
		/// The ioc net_ connection state changed.
		/// </summary>
		/// <param name="connected">
		/// The connected.
		/// </param>
		private void iocNet_ConnectionStateChanged(bool connected)
		{
			if (connected)
			{
				return;
			}

			this.Dispose();
		}
	}
}