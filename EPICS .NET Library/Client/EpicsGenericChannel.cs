// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsGenericChannel.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Client
{
	using Epics.Base.Constants;

	/// <summary>
	/// The epics delegate.
	/// </summary>
	/// <param name="sender">
	/// The sender.
	/// </param>
	/// <param name="newValue">
	/// The new value.
	/// </param>
	/// <typeparam name="DT">
	/// </typeparam>
	public delegate void EpicsDelegate<DT>(EpicsChannel<DT> sender, DT newValue);

	/// <summary>
	/// Generic Epics Channel, will ask the epics IOC for this type of value, even if his base type is another one.
	/// </summary>
	/// <typeparam name="DataType">
	/// Type wished for this channel
	/// </typeparam>
	public class EpicsChannel<DataType> : EpicsChannel
	{
		// status delegates

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsChannel{DataType}"/> class.
		/// </summary>
		/// <param name="channelname">
		/// The channel name.
		/// </param>
		/// <param name="CID">
		/// The CID.
		/// </param>
		/// <param name="Client">
		/// The client.
		/// </param>
		internal EpicsChannel(string channelname, int CID, EpicsClient Client)
		{
			this.name = channelname;
			this.client = Client;

			this.CID = (uint)CID;
		}

		/// <summary>
		///   Initializes a new instance of the <see cref = "EpicsChannel{DataType}" /> class.
		/// </summary>
		protected EpicsChannel()
		{
		}

		/// <summary>
		///   Event-Monitor which calls as soon a change on the channel happened which fits into the defined
		///   Monitormask (channel.MonitorMask).<br />The properties channel.MonitorMask and channel.MonitorDataCount
		///   do touch the behavior of this event and can't be changed when a monitor is already connected.
		///   <example>
		///     EpicsClient client = new EpicsClient();<br />
		///     EpicsChannel channel=clien.tCreateChannel("SEILER_C:CPU");<br />
		///     channel.MonitorMask = MonitorMask.VALUE;<br />
		///     channel.MonitorDataCount = 1;<br />
		///     channel.MonitorChanged += new EpicsDelegate(channel_MonitorChanged);
		///   </example>
		/// </summary>
		public new event EpicsDelegate<DataType> MonitorChanged
		{
			add
			{
				if (this.privMonitorChanged == null)
				{
					if (this.sid == 0)
					{
						this.monitorRegisterWait = true;
					}
					else
					{
						this.startMonitor();
						this.conn.ConnectionStateChanged += this.conn_ConnectionStateChanged;
					}
				}

				this.privMonitorChanged += value;
			}

			remove
			{
				this.privMonitorChanged -= value;
				if (this.privMonitorChanged == null)
				{
					this.conn.Send(this.client.Codec.stopSubscriptionMessage(this.SID, this.CID));
					this.conn.ConnectionStateChanged -= this.conn_ConnectionStateChanged;
				}
			}
		}

		/// <summary>
		///   Event-StatusMonitor which get called when the connection of this channel changes
		/// </summary>
		public override event EpicsStatusDelegate StatusChanged;

		// monitor delegates

		/// <summary>
		///   The priv monitor changed.
		/// </summary>
		private event EpicsDelegate<DataType> privMonitorChanged;

		/// <summary>
		/// Disposes the Channel, closing all Monitors and correctly informs the IOC
		/// </summary>
		public override void Dispose()
		{
			if (this.Disposing)
			{
				return;
			}
			else
			{
				this.Disposing = true;
			}

			// send message to close this channel
			if (this.sid > 0 && this.conn != null)
			{
				this.conn.ConnectionStateChanged -= this.conn_ConnectionStateChanged;

				if (this.privMonitorChanged != null)
				{
					this.conn.Send(this.client.Codec.stopSubscriptionMessage(this.sid, this.CID));
				}

				this.conn.Send(this.client.Codec.closeChannelMessage(this.sid, this.CID));
			}

			// if the client is not disposing, the channel removes himself from the list,
			// if the client is disposing he will remove us
			if (this.client.notDisposing)
			{
				this.client.dropEpicsChannel((int)this.CID);
			}
		}

		/// <summary>
		/// returns the current value from the channel.<br/>Therefore it has to connect to the server and request the data.<br/>
		///   This function is really slow!
		/// </summary>
		/// <returns>
		/// Value as type of the generic channel
		/// </returns>
		public new DataType Get()
		{
			return this.Get<DataType>();
		}

		/// <summary>
		/// returns the current value from the channel.<br/>Therefore it has to connect to the server and request the data.<br/>
		///   This function is really slow!
		/// </summary>
		/// <param name="datacount">
		/// The datacount.
		/// </param>
		/// <returns>
		/// Value as type of the generic channel
		/// </returns>
		public new DataType Get(int datacount)
		{
			return this.Get<DataType>(datacount);
		}

		/// <summary>
		/// The conn_ connection state changed.
		/// </summary>
		/// <param name="connected">
		/// The connected.
		/// </param>
		internal override void conn_ConnectionStateChanged(bool connected)
		{
			if (connected != true)
			{
				this.sid = 0;
				this.conn.ConnectionStateChanged -= this.conn_ConnectionStateChanged;
				this.conn = null;

				if (this.privMonitorChanged != null)
				{
					this.monitorRegisterWait = true;
				}

				this.client.SearchChannel((int)this.CID, this.ChannelName);

				if (this.StatusChanged != null)
				{
					this.StatusChanged(this, ChannelStatus.DISCONNECTED);
				}
			}
		}

		/// <summary>
		/// The receive value update.
		/// </summary>
		/// <param name="newValue">
		/// The new value.
		/// </param>
		internal override void receiveValueUpdate(object newValue)
		{
			if (this.privMonitorChanged != null)
			{
				this.privMonitorChanged(this, (DataType)newValue);
			}
		}

		/// <summary>
		/// The start monitor.
		/// </summary>
		protected override void startMonitor()
		{
			if (typeof(DataType).IsArray)
			{
				this.conn.Send(
					this.client.Codec.createSubscriptionMessage(
						this.SID, this.CID, typeof(DataType), (ushort)this.monitorDataCount, this.mask));
			}
			else
			{
				this.conn.Send(this.client.Codec.createSubscriptionMessage(this.SID, this.CID, typeof(DataType), 1, this.mask));
			}
		}
	}
}