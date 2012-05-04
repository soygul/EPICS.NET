// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsServerChannel.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Server
{
	using System;
	using System.Collections.Generic;

	using Epics.Base;
	using Epics.Base.Constants;

	/// <summary>
	/// The epics server channel.
	/// </summary>
	internal class EpicsServerChannel : IDisposable
	{
		/// <summary>
		///   The channel name.
		/// </summary>
		private readonly string ChannelName;

		/// <summary>
		///   The client id.
		/// </summary>
		private readonly int ClientId;

		/// <summary>
		///   The conn.
		/// </summary>
		private readonly EpicsServerTCPConnection Conn;

		/// <summary>
		///   The property.
		/// </summary>
		private readonly RecordProperty Property = RecordProperty.VAL;

		/// <summary>
		///   The record.
		/// </summary>
		private readonly EpicsRecord Record;

		/// <summary>
		///   The server.
		/// </summary>
		private readonly EpicsServer Server;

		/// <summary>
		///   The server id.
		/// </summary>
		private readonly int ServerId;

		/// <summary>
		///   The monitors.
		/// </summary>
		private readonly Dictionary<int, EpicsServerMonitor> monitors = new Dictionary<int, EpicsServerMonitor>();

		/// <summary>
		///   The access.
		/// </summary>
		private AccessRights Access = AccessRights.ReadAndWrite;

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsServerChannel"/> class.
		/// </summary>
		/// <param name="server">
		/// The server.
		/// </param>
		/// <param name="serverId">
		/// The server id.
		/// </param>
		/// <param name="clientId">
		/// The client id.
		/// </param>
		/// <param name="channelName">
		/// The channel name.
		/// </param>
		/// <param name="conn">
		/// The conn.
		/// </param>
		internal EpicsServerChannel(
			EpicsServer server, int serverId, int clientId, string channelName, EpicsServerTCPConnection conn)
		{
			this.NotDisposing = true;
			this.ServerId = serverId;
			this.ClientId = clientId;
			this.ChannelName = channelName;
			this.Server = server;
			this.Conn = conn;
			this.Conn.ConnectionStateChanged += this.Conn_ConnectionStateChanged;

			try
			{
				if (channelName.Contains("."))
				{
					var splitted = channelName.Split('.');
					this.Record = this.Server.recordList[splitted[0]];
					this.Property = (RecordProperty)Enum.Parse(typeof(RecordProperty), splitted[1]);
				}
				else
				{
					this.Record = this.Server.recordList[this.ChannelName];
				}

				this.Conn.Send(
					this.Server.Codec.channelCreatedMessage(
						this.ClientId, this.ServerId, this.Record.TYPE, this.Record.dataCount, this.Access));
			}
			catch (Exception e)
			{
				this.Conn.Send(this.Server.Codec.channelCreationFailMessage(this.ClientId));
				this.Dispose();
			}
		}

		/// <summary>
		///   Gets a value indicating whether NotDisposing.
		/// </summary>
		public bool NotDisposing { get; private set; }

		/// <summary>
		/// The dispose.
		/// </summary>
		/// <param name="clientRequest">
		/// The client request.
		/// </param>
		public void Dispose(bool clientRequest)
		{
			if (clientRequest)
			{
				this.Conn.Send(this.Server.Codec.channelClearMessage(this.ClientId, this.ServerId));
				this.Server.DropEpicsChannel(this.ClientId);
			}
		}

		/// <summary>
		/// Disposes Channel, removes all connected Monitors and Connections
		/// </summary>
		public void Dispose()
		{
			this.commonDispose();

			this.Conn.Send(this.Server.Codec.channelDisconnectionMessage(this.ClientId));
			this.Server.DropEpicsChannel(this.ClientId);
		}

		/// <summary>
		/// The add monitor.
		/// </summary>
		/// <param name="type">
		/// The type.
		/// </param>
		/// <param name="dataCount">
		/// The data count.
		/// </param>
		/// <param name="subscriptionId">
		/// The subscription id.
		/// </param>
		/// <param name="mask">
		/// The mask.
		/// </param>
		internal void addMonitor(EpicsType type, int dataCount, int subscriptionId, MonitorMask mask)
		{
			// does he request to add a subscriptionId at the same Id as it already exist?
			if (this.monitors.ContainsKey(subscriptionId))
			{
				return;
			}

			lock (this.monitors)
			{
				this.monitors.Add(
					subscriptionId, new EpicsServerMonitor(this.Record, this.Property, this, type, dataCount, mask, subscriptionId));
			}
		}

		/// <summary>
		/// The drop monitor.
		/// </summary>
		/// <param name="key">
		/// The key.
		/// </param>
		internal void dropMonitor(int key)
		{
			lock (this.monitors)
			{
				this.monitors.Remove(key);
			}
		}

		/// <summary>
		/// The put value.
		/// </summary>
		/// <param name="ioId">
		/// The io id.
		/// </param>
		/// <param name="type">
		/// The type.
		/// </param>
		/// <param name="dataCount">
		/// The data count.
		/// </param>
		/// <param name="payload">
		/// The payload.
		/// </param>
		internal void putValue(int ioId, EpicsType type, int dataCount, byte[] payload)
		{
			try
			{
				object val;

				if (dataCount == 1)
				{
					val = NetworkByteConverter.byteToObject(payload, type);
				}
				else
				{
					val = NetworkByteConverter.byteToObject(payload, type, dataCount);
				}

				if (this.Property == RecordProperty.VAL)
				{
					if (dataCount == 1)
					{
						this.Record["VAL"] = Convert.ChangeType(val, this.Record.GetValueType());
					}
					else
					{
						switch (this.Record.type)
						{
							case EpicsType.String:
								((EpicsArray<string>)this.Record["VAL"]).Set(val);
								break;
							case EpicsType.Double:
								((EpicsArray<double>)this.Record["VAL"]).Set(val);
								break;
							case EpicsType.Float:
								((EpicsArray<float>)this.Record["VAL"]).Set(val);
								break;
							case EpicsType.Int:
								((EpicsArray<int>)this.Record["VAL"]).Set(val);
								break;
							case EpicsType.Short:
								((EpicsArray<short>)this.Record["VAL"]).Set(val);
								break;
							case EpicsType.SByte:
								((EpicsArray<sbyte>)this.Record["VAL"]).Set(val);
								break;
						}
					}
				}
				else
				{
					this.Record[this.Property.ToString()] = Convert.ChangeType(val, this.Record[this.Property.ToString()].GetType());
				}

				this.Conn.Send(
					this.Server.Codec.channelWroteMessage(this.ClientId, ioId, type, dataCount, EpicsTransitionStatus.ECA_NORMAL));
			}
			catch (Exception exp)
			{
				this.Conn.Send(
					this.Server.Codec.errorMessage(
						this.ClientId, EpicsTransitionStatus.ECA_BADSTR, "Message was not correct", new byte[16]));
				return;
			}
		}

		/// <summary>
		/// The read value.
		/// </summary>
		/// <param name="ioId">
		/// The io id.
		/// </param>
		/// <param name="type">
		/// The type.
		/// </param>
		/// <param name="dataCount">
		/// The data count.
		/// </param>
		internal void readValue(int ioId, EpicsType type, int dataCount)
		{
			byte[] val;

			var objVal = this.Record[this.Property.ToString()];
			if (objVal == null)
			{
				objVal = 0;
			}

			try
			{
				if (dataCount == 1)
				{
					val = NetworkByteConverter.objectToByte(objVal, type, this.Record);
				}
				else
				{
					if (objVal.GetType().IsGenericType)
					{
						val = NetworkByteConverter.objectToByte(objVal, type, this.Record, dataCount);
					}
					else
					{
						return;
					}
				}

				this.Conn.Send(this.Server.Codec.channelReadMessage(this.ClientId, ioId, type, dataCount, val));
			}
			catch (Exception e)
			{
				this.Conn.Send(
					this.Server.Codec.errorMessage(this.ClientId, EpicsTransitionStatus.ECA_BADTYPE, "WRONG TYPE", new byte[16]));
			}
		}

		/// <summary>
		/// The remove monitor.
		/// </summary>
		/// <param name="key">
		/// The key.
		/// </param>
		internal void removeMonitor(int key)
		{
			this.monitors[key].Dispose();
		}

		/// <summary>
		/// The restart monitor.
		/// </summary>
		/// <param name="key">
		/// The key.
		/// </param>
		internal void restartMonitor(int key)
		{
			this.monitors[key].StartMonitor();
		}

		/// <summary>
		/// The send monitor change.
		/// </summary>
		/// <param name="subscriptionId">
		/// The subscription id.
		/// </param>
		/// <param name="dataType">
		/// The data type.
		/// </param>
		/// <param name="dataCount">
		/// The data count.
		/// </param>
		/// <param name="status">
		/// The status.
		/// </param>
		/// <param name="data">
		/// The data.
		/// </param>
		internal void sendMonitorChange(
			int subscriptionId, EpicsType dataType, int dataCount, EpicsTransitionStatus status, byte[] data)
		{
			this.Conn.Send(this.Server.Codec.monitorChangeMessage(subscriptionId, this.ClientId, dataType, dataCount, data));
		}

		/// <summary>
		/// The send monitor close.
		/// </summary>
		/// <param name="subscriptionId">
		/// The subscription id.
		/// </param>
		/// <param name="type">
		/// The type.
		/// </param>
		internal void sendMonitorClose(int subscriptionId, EpicsType type)
		{
			this.Conn.Send(this.Server.Codec.monitorCloseMessage(type, this.ServerId, subscriptionId));
		}

		/// <summary>
		/// The stop monitor.
		/// </summary>
		/// <param name="key">
		/// The key.
		/// </param>
		internal void stopMonitor(int key)
		{
			this.monitors[key].StopMonitor();
		}

		/// <summary>
		/// Event on Connection change. Will drop Channel as soon the Connection is broke.
		/// </summary>
		/// <param name="connected">
		/// </param>
		private void Conn_ConnectionStateChanged(bool connected)
		{
			if (connected == false)
			{
				this.Dispose();
			}
		}

		/// <summary>
		/// The common dispose.
		/// </summary>
		private void commonDispose()
		{
			if (this.NotDisposing)
			{
				this.NotDisposing = false;
			}
			else
			{
				return;
			}

			lock (this.monitors)
			{
				foreach (var mon in this.monitors)
				{
					mon.Value.Dispose();
				}
			}

			this.monitors.Clear();
		}
	}
}