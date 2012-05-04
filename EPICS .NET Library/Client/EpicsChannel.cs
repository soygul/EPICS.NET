// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsChannel.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Client
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Threading;

	using Epics.Base.Constants;
	using Epics.Base.ETypes;

	/// <summary>
	/// Monitor delegate
	/// </summary>
	/// <param name="sender">
	/// EpicsChannel on which the changed happened
	/// </param>
	/// <param name="newValue">
	/// Object value of the type the monitor was registered for
	/// </param>
	public delegate void EpicsDelegate(EpicsChannel sender, object newValue);

	/// <summary>
	/// Status delegate
	/// </summary>
	/// <param name="sender">
	/// Channel which had his status changed
	/// </param>
	/// <param name="newStatus">
	/// new status
	/// </param>
	public delegate void EpicsStatusDelegate(EpicsChannel sender, ChannelStatus newStatus);

	/// <summary>
	/// Gets called after an asynchronous put operation is complete.
	/// </summary>
	/// <param name="sender">
	/// Channel on which the put operation was requested.
	/// </param>
	/// <param name="succeeded">
	/// True if the put operation was successful. False if the put operation failed.
	/// </param>
	public delegate void EpicsPutDelegate(EpicsChannel sender, bool succeeded);

	/// <summary>
	/// Epics Channel to communicate with a record-property. Must be created through the epicsClient-Factory
	///   <example>
	/// EpicsClient client = new EpicsClient();<br/>
	///     EpicsChannel channel = client.CreateChannel("TEST:CHANNEL");<br/>
	///     object val = channel.Get();<br/>
	///     channel.Dispose();
	///   </example>
	/// </summary>
	public class EpicsChannel : IDisposable
	{
		// Disposing

		/// <summary>
		///   The client.
		/// </summary>
		internal EpicsClient client;

		/// <summary>
		///   The conn.
		/// </summary>
		internal EpicsClientTCPConnection conn;

		/// <summary>
		///   The translator.
		/// </summary>
		protected static Dictionary<Type, Dictionary<Type, Type>> Translator = new Dictionary<Type, Dictionary<Type, Type>>();

		/// <summary>
		///   The disposing.
		/// </summary>
		protected bool Disposing;

		/// <summary>
		///   The channel defined type.
		/// </summary>
		protected Type channelDefinedType;

		/// <summary>
		///   The enum array.
		/// </summary>
		protected string[] enumArray;

		// channel information

		/// <summary>
		///   The last value.
		/// </summary>
		protected object lastValue;

		/// <summary>
		///   The mask.
		/// </summary>
		protected MonitorMask mask = MonitorMask.VALUE_ALARM;

		/// <summary>
		///   The monitor data count.
		/// </summary>
		protected uint monitorDataCount;

		// monitor delegates
		/// <summary>
		///   The monitor register wait.
		/// </summary>
		protected bool monitorRegisterWait;

		/// <summary>
		///   The name.
		/// </summary>
		protected string name;

		/// <summary>
		///   sid of the channel
		/// </summary>
		protected uint sid;

		/// <summary>
		///   The wait for connect.
		/// </summary>
		protected AutoResetEvent waitForConnect = new AutoResetEvent(false);

		/// <summary>
		///   The wait for get.
		/// </summary>
		protected AutoResetEvent waitForGet = new AutoResetEvent(false);

		/// <summary>
		///   The wait for put.
		/// </summary>
		protected AutoResetEvent waitForPut = new AutoResetEvent(false);

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsChannel"/> class.
		/// </summary>
		/// <param name="channelname">
		/// The channelname.
		/// </param>
		/// <param name="CID">
		/// The cid.
		/// </param>
		/// <param name="Client">
		/// The client.
		/// </param>
		internal EpicsChannel(string channelname, int CID, EpicsClient Client)
		{
			this.Status = ChannelStatus.REQUESTED;

			this.client = Client;
			this.name = channelname;

			this.CID = (uint)CID;
		}

		/// <summary>
		///   Initializes a new instance of the <see cref = "EpicsChannel" /> class.
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
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public event EpicsDelegate MonitorChanged
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
				}
			}
		}

		/// <summary>
		///   Event-StatusMonitor which get called when the connectivity of this channel changes
		/// </summary>
		public virtual event EpicsStatusDelegate StatusChanged;

		/// <summary>
		///   The priv monitor changed.
		/// </summary>
		private event EpicsDelegate privMonitorChanged;

		/// <summary>
		///   Access Rights of the Channel
		/// </summary>
		public AccessRights AccessRight { get; internal set; }

		/// <summary>
		///   Count of elements in the value-array
		/// </summary>
		public uint ChannelDataCount { get; internal set; }

		/// <summary>
		///   Epics defined type for this Channel
		/// </summary>
		public Type ChannelDefinedType
		{
			get
			{
				return this.channelDefinedType;
			}

			internal set
			{
				this.channelDefinedType = value;
				if (!Translator.ContainsKey(value))
				{
					defineTranslation(value);
				}
			}
		}

		// properties

		/// <summary>
		///   Channelname of the created channel
		/// </summary>
		public string ChannelName
		{
			get
			{
				return this.name;
			}

			protected set
			{
				this.name = value;
			}
		}

		/// <summary>
		///   Allows to define the count of values which shall be returned by a array.<br />
		///   will automaticly take the maximu if not defined differently.<br />
		///   Has to be set before the first monitor is connected.
		/// </summary>
		public uint MonitorDataCount
		{
			get
			{
				return this.monitorDataCount;
			}

			set
			{
				if (this.privMonitorChanged == null || this.monitorRegisterWait)
				{
					this.monitorDataCount = value;
				}
				else
				{
					throw new Exception("Unallowed configuration change while Monitor active. Do remove all Monitors first");
				}
			}
		}

		/// <summary>
		///   Monitor Mask, allows to define what shall be monitored. Has to be set before the first monitor is connected!
		/// </summary>
		public MonitorMask MonitorMask
		{
			get
			{
				return this.mask;
			}

			set
			{
				if (this.privMonitorChanged == null)
				{
					this.mask = value;
				}
				else
				{
					throw new Exception("Unallowed Configuration change while Monitor active. Do remove all Monitors first");
				}
			}
		}

		/// <summary>
		///   Current connectivity status of the channel.
		/// </summary>
		public ChannelStatus Status { get; protected set; }

		/// <summary>
		///   Gets or sets CID.
		/// </summary>
		internal uint CID { get; set; }

		/// <summary>
		///   Gets or sets ChannelEpicsType.
		/// </summary>
		internal EpicsType ChannelEpicsType { get; set; }

		/// <summary>
		///   Gets or sets Conn.
		/// </summary>
		internal EpicsClientTCPConnection Conn
		{
			get
			{
				return this.conn;
			}

			set
			{
				// if it's set to null it was not able to be created on this connection.
				if (value == null)
				{
					this.conn.ConnectionStateChanged -= this.conn_ConnectionStateChanged;
					this.conn = null;
					this.client.SearchChannel((int)this.CID, this.name);
					return;
				}

				// if there is no other connection set. It would be possible that several iocs serve the same channel
				// and every of it would answer with is connection, but we only want the first one
				if (this.conn == null || this.conn.Connected == false)
				{
					this.conn = value;

					// shouldn't produce a problem, but may could
					// client.FoundChannel((int)CID);
					this.conn.Send(this.client.Codec.createChannelMessage(this.name, this.CID));
					this.conn.ConnectionStateChanged += this.conn_ConnectionStateChanged;
				}
			}
		}

		/// <summary>
		///   Gets or sets LastValue.
		/// </summary>
		internal object LastValue
		{
			get
			{
				return this.lastValue;
			}

			set
			{
				this.lastValue = value;
				this.waitForGet.Set();
			}
		}

		/// <summary>
		///   Property to set and get the SID of the Channel.
		///   By Setting the SID, you tell the channel that it was successfully connected. and it tries 
		///   if a monitor was requested to open a subscription.
		/// </summary>
		internal uint SID
		{
			get
			{
				return this.sid;
			}

			set
			{
				if (this.sid != 0)
				{
					return;
				}

				this.sid = value;
				this.client.FoundChannel((int)this.CID);

				this.waitForConnect.Set();
				if (this.monitorRegisterWait)
				{
					this.startMonitor();
					this.monitorRegisterWait = false;
				}

				this.Status = ChannelStatus.CONNECTED;
				if (this.StatusChanged != null)
				{
					this.StatusChanged(this, ChannelStatus.CONNECTED);
				}
			}
		}

		/// <summary>
		/// BlindGet is supposed for extended Epics Type for unknown DataTypes.
		/// </summary>
		/// <typeparam name="DBRType">
		/// requested ExtTyp with object as requested Type
		/// </typeparam>
		/// <returns>
		/// exteded DataType Structure with casted Value
		/// </returns>
		public object BlindGet<DBRType>()
		{
#if DEBUG
			if (this.sid == 0)
			{
				this.waitForConnect.WaitOne();
			}

#else
                if (sid == 0)
                    if (!waitForConnect.WaitOne(client.Config.ConnectTimeout, true))
                        throw new Exception("Connection not Established in Time");
#endif

			return this.BlindGet<DBRType>((int)this.ChannelDataCount);
		}

		/// <summary>
		/// BlindGet is supposed for extended Epics Type for unknown DataTypes.
		/// </summary>
		/// <typeparam name="DBRType">
		/// requested ExtTyp with object as requested Type
		/// </typeparam>
		/// <param name="datacount">
		/// amount of requested Amount
		/// </param>
		/// <returns>
		/// exteded DataType Structure with casted Value
		/// </returns>
		public object BlindGet<DBRType>(int datacount)
		{
#if DEBUG
			if (this.sid == 0)
			{
				this.waitForConnect.WaitOne();
			}

#else
                if (sid == 0)
                    if (!waitForConnect.WaitOne(client.Config.ConnectTimeout, true))
                        throw new Exception("Connection not Established in Time");
#endif

			if (datacount > this.ChannelDataCount)
			{
				throw new Exception("Requested higher datacount than existing");
			}
			{
				var reqType = typeof(DBRType);
				if (Translator[this.channelDefinedType].ContainsKey(reqType))
				{
					reqType = Translator[this.channelDefinedType][reqType];
				}

				this.conn.Send(this.client.Codec.getMessage(reqType, (ushort)datacount, this.SID, this.CID));
			}

#if DEBUG
			this.waitForGet.WaitOne();
#else
            if (!waitForGet.WaitOne(client.Config.GetTimeout, true))
                    throw new Exception("Server did not respond in Time to the Get Request");
#endif

			return this.lastValue;
		}

		/// <summary>
		/// returns the current value from the channel. <br/>
		///   Therefore it has to connect to the server and request the data.<br/>
		///   This function is really slow!
		/// </summary>
		/// <param name="dataCount">
		/// count of values required (for an array)
		/// </param>
		/// <returns>
		/// Value as channelDefinedType
		/// </returns>
		public object Get(int dataCount)
		{
#if DEBUG
			if (this.sid == 0)
			{
				this.waitForConnect.WaitOne();
			}

#else
                if (sid == 0)
                    if (!waitForConnect.WaitOne(client.Config.ConnectTimeout, true))
                        throw new Exception("Connection not Established in Time");
#endif

			if (dataCount > this.ChannelDataCount)
			{
				throw new Exception("Requested higher datacount than existing");
			}

			this.waitForGet.Reset();
			this.conn.Send(this.client.Codec.getMessage(this.ChannelEpicsType, (ushort)dataCount, this.SID, this.CID));

#if DEBUG
			this.waitForGet.WaitOne();
#else
            if (!waitForGet.WaitOne(client.Config.GetTimeout, true))
                    throw new Exception("Server did not respond in Time to the Get Request");
#endif

			return this.lastValue;
		}

		/// <summary>
		/// returns the current value from the channel.<br/>
		///   Therefore it has to connect to the server and request the data.<br/>
		///   This function is really slow!
		/// </summary>
		/// <returns>
		/// Value as channelDefinedType
		/// </returns>
		public object Get()
		{
			// needed because the ChannelDataCount will be set as soon as we receive the sid
#if DEBUG
			if (this.sid == 0)
			{
				this.waitForConnect.WaitOne();
			}

#else
                if (sid == 0)
                    if (!waitForConnect.WaitOne(client.Config.ConnectTimeout, true))
                        throw new Exception("Connection not Established in Time");
#endif

			return this.Get((int)this.ChannelDataCount);
		}

		/// <summary>
		/// returns the current value from the channel.<br/>
		///   Therefore it has to connect to the server and request the data.<br/>
		///   This function is really slow!
		/// </summary>
		/// <typeparam name="DBRType">
		/// Type you want to receive back. if you request more then one value you have to request a type-array
		/// </typeparam>
		/// <param name="datacount">
		/// Count of values
		/// </param>
		/// <returns>
		/// Value as requested type
		/// </returns>
		public DBRType Get<DBRType>(int datacount)
		{
			/*if (!typeof(DBRType).IsArray && datacount > 1)
                throw new Exception("Requested a datacount bigger then 1 but didn't ask for an array");*/
#if DEBUG
			if (this.sid == 0)
			{
				this.waitForConnect.WaitOne();
			}

#else
                if (sid == 0)
                    if (!waitForConnect.WaitOne(client.Config.ConnectTimeout, true))
                        throw new Exception("Connection not Established in Time");
#endif

			if (datacount > this.ChannelDataCount)
			{
				throw new Exception("Requested higher datacount than existing");
			}
			{
				var reqType = typeof(DBRType);
				if (Translator[this.channelDefinedType].ContainsKey(reqType))
				{
					throw new Exception("BLIND CAST! Use BlindGet<GenericType>()");
				}

				this.conn.Send(this.client.Codec.getMessage(reqType, (ushort)datacount, this.SID, this.CID));
			}

#if DEBUG
			this.waitForGet.WaitOne();
#else
            if (!waitForGet.WaitOne(client.Config.GetTimeout, true))
                    throw new Exception("Server did not respond in Time to the Get Request");
#endif

			return (DBRType)this.lastValue;
		}

		/// <summary>
		/// returns the current value from the channel.<br/>Therefore it has to connect to the server and request the data.<br/>
		///   This function is really slow!<br/>Important: it will automaticly take the epics defined amount, if this is bigger then
		///   one you have to request an array.
		/// </summary>
		/// <typeparam name="DBRType">
		/// Type you want to receive back. if you request more then one value you have to request a type-array
		/// </typeparam>
		/// <returns>
		/// Value as requested type
		/// </returns>
		public DBRType Get<DBRType>()
		{
#if DEBUG
			if (this.sid == 0)
			{
				this.waitForConnect.WaitOne();
			}

#else
            if (sid == 0)
                if (!waitForConnect.WaitOne(client.Config.ConnectTimeout, true))
                    throw new Exception("Connection not Established in Time");
#endif

			return this.Get<DBRType>((int)this.ChannelDataCount);
		}

		/// <summary>
		/// Calls the callback with the new value of the epics defined type
		/// </summary>
		/// <param name="GetListener">
		/// callback function
		/// </param>
		public void GetAsync(EpicsDelegate GetListener)
		{
			var newThread = new Thread(this.GetAsyncThread);
			newThread.Start(GetListener);
		}

		/// <summary>
		/// Calls the callback with the new value of the passed type.<br/>if the datacount of the channel is bigger then one
		///   you have to ask a array.
		/// </summary>
		/// <typeparam name="DBRType">
		/// Type you want to receive back. if you request more then one value you have to request a type-array
		/// </typeparam>
		/// <param name="GetListener">
		/// callback function
		/// </param>
		public void GetAsync<DBRType>(EpicsDelegate GetListener)
		{
			var newThread = new Thread(this.GetAsyncThread<DBRType>);
			newThread.Start(GetListener);
		}

		/// <summary>
		/// The get enum array.
		/// </summary>
		/// <returns>
		/// </returns>
		/// <exception cref="Exception">
		/// </exception>
		public string[] GetEnumArray()
		{
			if (this.enumArray != null)
			{
				return this.enumArray;
			}

#if DEBUG
			if (this.sid == 0)
			{
				this.waitForConnect.WaitOne();
			}

#else
                if (sid == 0)
                    if (!waitForConnect.WaitOne(client.Config.ConnectTimeout))
                        throw new Exception("Connection not Established in Time");
#endif

			if (this.ChannelEpicsType != EpicsType.Enum)
			{
				throw new Exception("Channel is not an Enum");
			}

			this.waitForGet.Reset();
			this.conn.Send(this.client.Codec.getMessage(EpicsType.Control_Enum, 1, this.SID, this.CID));

#if DEBUG
			this.waitForGet.WaitOne();
#else
                if (!waitForGet.WaitOne(client.Config.GetTimeout))
                    throw new Exception("Server did not respond in Time to the Get Request");
#endif

			this.enumArray = ((ExtEnumType)this.lastValue).EnumArray;

			return this.enumArray;
		}

		/// <summary>
		/// Sets Values to the Channel.
		/// </summary>
		/// <param name="value">
		/// Value to set
		/// </param>
		public void Put(string value)
		{
			this.Put((object)value);
		}

		/// <summary>
		/// Sets Values to the Channel.
		/// </summary>
		/// <param name="value">
		/// Value to set
		/// </param>
		public void Put(short value)
		{
			this.Put((object)value);
		}

		/// <summary>
		/// Sets Values to the Channel.
		/// </summary>
		/// <param name="value">
		/// Value to set
		/// </param>
		public void Put(float value)
		{
			this.Put((object)value);
		}

		/// <summary>
		/// Sets Values to the Channel.
		/// </summary>
		/// <param name="value">
		/// Value to set
		/// </param>
		public void Put(sbyte value)
		{
			this.Put((object)value);
		}

		/// <summary>
		/// Sets Values to the Channel.
		/// </summary>
		/// <param name="value">
		/// Value to set
		/// </param>
		public void Put(int value)
		{
			this.Put((object)value);
		}

		/// <summary>
		/// Sets Values to the Channel.
		/// </summary>
		/// <param name="value">
		/// Value to set
		/// </param>
		public void Put(double value)
		{
			this.Put((object)value);
		}

		/// <summary>
		/// Sets Values to the Channel.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		public void Put(string[] values)
		{
			this.Put<string>(values);
		}

		/// <summary>
		/// Sets Values to the Channel.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		public void Put(short[] values)
		{
			this.Put<short>(values);
		}

		/// <summary>
		/// Sets Values to the Channel.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		public void Put(float[] values)
		{
			this.Put<float>(values);
		}

		/// <summary>
		/// Sets Values to the Channel.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		public void Put(sbyte[] values)
		{
			this.Put<sbyte>(values);
		}

		/// <summary>
		/// Sets Values to the Channel.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		public void Put(int[] values)
		{
			this.Put<int>(values);
		}

		/// <summary>
		/// Sets Values to the Channel.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		public void Put(double[] values)
		{
			this.Put<double>(values);
		}

		// Hack: v1 -BEGIN-

		// Hack: v1 -END-

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <param name="value">
		/// Value to set
		/// </param>
		/// <param name="callBack">
		/// The call Back.
		/// </param>
		public void PutAsync(string value, EpicsPutDelegate callBack)
		{
			this.PutAsync((object)value, callBack);
		}

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <param name="value">
		/// Value to set
		/// </param>
		/// <param name="callBack">
		/// The call Back.
		/// </param>
		public void PutAsync(short value, EpicsPutDelegate callBack)
		{
			this.PutAsync((object)value, callBack);
		}

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <param name="value">
		/// Value to set
		/// </param>
		/// <param name="callBack">
		/// The call Back.
		/// </param>
		public void PutAsync(float value, EpicsPutDelegate callBack)
		{
			this.PutAsync((object)value, callBack);
		}

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <param name="value">
		/// Value to set
		/// </param>
		/// <param name="callBack">
		/// The call Back.
		/// </param>
		public void PutAsync(sbyte value, EpicsPutDelegate callBack)
		{
			this.PutAsync((object)value, callBack);
		}

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <param name="value">
		/// Value to set
		/// </param>
		/// <param name="callBack">
		/// The call Back.
		/// </param>
		public void PutAsync(int value, EpicsPutDelegate callBack)
		{
			this.PutAsync((object)value, callBack);
		}

		/*
		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <param name="value">
		/// Value to set
		/// </param>
		/// <param name="callBack">
		/// The call Back.
		/// </param>
		public void PutAsync(bool value, EpicsPutDelegate callBack)
		{
			throw new NotImplementedException("PutAsync(bool, callback) is not implemented yet!");
			//this.PutAsync((object)value, callBack);
		}
		*/

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <param name="value">
		/// Value to set
		/// </param>
		/// <param name="callBack">
		/// The call Back.
		/// </param>
		public void PutAsync(double value, EpicsPutDelegate callBack)
		{
			this.PutAsync((object)value, callBack);
		}

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		/// <param name="callBack">
		/// The call Back.
		/// </param>
		public void PutAsync(string[] values, EpicsPutDelegate callBack)
		{
			this.PutAsync<string>(values, callBack);
		}

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		/// <param name="callBack">
		/// The call Back.
		/// </param>
		public void PutAsync(short[] values, EpicsPutDelegate callBack)
		{
			this.PutAsync<short>(values, callBack);
		}

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		/// <param name="callBack">
		/// The call Back.
		/// </param>
		public void PutAsync(float[] values, EpicsPutDelegate callBack)
		{
			this.PutAsync<float>(values, callBack);
		}

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		/// <param name="callBack">
		/// The call Back.
		/// </param>
		public void PutAsync(sbyte[] values, EpicsPutDelegate callBack)
		{
			this.PutAsync<sbyte>(values, callBack);
		}

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		/// <param name="callBack">
		/// The call Back.
		/// </param>
		public void PutAsync(int[] values, EpicsPutDelegate callBack)
		{
			this.PutAsync<int>(values, callBack);
		}

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		/// <param name="callBack">
		/// The call Back.
		/// </param>
		public void PutAsync(double[] values, EpicsPutDelegate callBack)
		{
			this.PutAsync<double>(values, callBack);
		}

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <typeparam name="T">
		/// Generic data type.
		/// </typeparam>
		/// <param name="value">
		/// Value to set
		/// </param>
		/// <param name="callBack">
		/// The call Back.
		/// </param>
		public void PutAsyncGeneric<T>(T value, EpicsPutDelegate callBack)
		{
			this.PutAsync(value, callBack);
		}

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <typeparam name="T">
		/// Generic array type.
		/// </typeparam>
		/// <param name="values">
		/// The values.
		/// </param>
		/// <param name="callBack">
		/// The call Back.
		/// </param>
		public void PutAsyncGenericArray<T>(T[] values, EpicsPutDelegate callBack)
		{
			this.PutAsync(values, callBack);
		}

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <param name="value">
		/// Value to set
		/// </param>
		public void PutSync(string value)
		{
			this.PutSync((object)value);
		}

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <param name="value">
		/// Value to set
		/// </param>
		public void PutSync(short value)
		{
			this.PutSync((object)value);
		}

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <param name="value">
		/// Value to set
		/// </param>
		public void PutSync(float value)
		{
			this.PutSync((object)value);
		}

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <param name="value">
		/// Value to set
		/// </param>
		public void PutSync(sbyte value)
		{
			this.PutSync((object)value);
		}

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <param name="value">
		/// Value to set
		/// </param>
		public void PutSync(int value)
		{
			this.PutSync((object)value);
		}

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <param name="value">
		/// Value to set
		/// </param>
		public void PutSync(double value)
		{
			this.PutSync((object)value);
		}

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		public void PutSync(string[] values)
		{
			this.PutSync<string>(values);
		}

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		public void PutSync(short[] values)
		{
			this.PutSync<short>(values);
		}

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		public void PutSync(float[] values)
		{
			this.PutSync<float>(values);
		}

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		public void PutSync(sbyte[] values)
		{
			this.PutSync<sbyte>(values);
		}

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		public void PutSync(int[] values)
		{
			this.PutSync<int>(values);
		}

		/// <summary>
		/// Sets Values to the Channel and waits for the Affirmation.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		public void PutSync(double[] values)
		{
			this.PutSync<double>(values);
		}

		/// <summary>
		/// Disposes the Channel, closing all Monitors and correctly informs the IOC
		/// </summary>
		public virtual void Dispose()
		{
			if (this.Disposing)
			{
				return;
			}
			else
			{
				this.Disposing = true;
			}

			// tell the client this channel was found, it's a lie, but it will make sure the channel will not be created
			this.client.FoundChannel((int)this.CID);

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
		/// The conn_ connection state changed.
		/// </summary>
		/// <param name="connected">
		/// The connected.
		/// </param>
		internal virtual void conn_ConnectionStateChanged(bool connected)
		{
			if (connected != true)
			{
				this.sid = 0;
				this.conn.ConnectionStateChanged -= this.conn_ConnectionStateChanged;
				this.conn = null;
				this.waitForConnect.Reset();

				if (this.privMonitorChanged != null)
				{
					this.monitorRegisterWait = true;
				}

				this.Status = ChannelStatus.DISCONNECTED;
				if (this.StatusChanged != null)
				{
					this.StatusChanged(this, ChannelStatus.DISCONNECTED);
				}

				this.client.SearchChannel((int)this.CID, this.ChannelName);
			}
		}

		/// <summary>
		/// The receive value update.
		/// </summary>
		/// <param name="newValue">
		/// The new value.
		/// </param>
		internal virtual void receiveValueUpdate(object newValue)
		{
			if (this.privMonitorChanged != null)
			{
				this.privMonitorChanged(this, newValue);
			}
		}

		/// <summary>
		/// The write succeeded.
		/// </summary>
		internal void writeSucceeded()
		{
			this.waitForPut.Set();
		}

		/// <summary>
		/// The define translation.
		/// </summary>
		/// <param name="type">
		/// The type.
		/// </param>
		protected static void defineTranslation(Type type)
		{
			if (type == typeof(int) || type == typeof(int[]))
			{
				defineTranslation<int>();
			}
			else if (type == typeof(string) || type == typeof(string[]))
			{
				defineTranslation<string>();
			}
			else if (type == typeof(float) || type == typeof(float[]))
			{
				defineTranslation<float>();
			}
			else if (type == typeof(double) || type == typeof(double[]))
			{
				defineTranslation<double>();
			}
			else if (type == typeof(sbyte) || type == typeof(sbyte[]))
			{
				defineTranslation<sbyte>();
			}
			else if (type == typeof(short) || type == typeof(short[]))
			{
				defineTranslation<short>();
			}
		}

		/// <summary>
		/// The define translation.
		/// </summary>
		/// <typeparam name="dataType">
		/// </typeparam>
		protected static void defineTranslation<dataType>()
		{
			var type = typeof(dataType);
			lock (Translator)
			{
				if (Translator.ContainsKey(type))
				{
					return;
				}

				Translator.Add(type, new Dictionary<Type, Type>());

				Translator[type].Add(typeof(object), type);
				Translator[type].Add(typeof(ExtTimeType<object>), typeof(ExtTimeType<dataType>));
				Translator[type].Add(typeof(ExtControl<object>), typeof(ExtControl<dataType>));
				Translator[type].Add(typeof(ExtGraphic<object>), typeof(ExtGraphic<dataType>));
				Translator[type].Add(typeof(ExtType<object>), typeof(ExtType<dataType>));
				Translator[type].Add(typeof(ExtAcknowledge<object>), typeof(ExtAcknowledge<dataType>));

				Translator[type].Add(typeof(object[]), typeof(dataType[]));
				Translator[type].Add(typeof(ExtTimeType<object[]>), typeof(ExtTimeType<dataType[]>));
				Translator[type].Add(typeof(ExtControl<object[]>), typeof(ExtControl<dataType[]>));
				Translator[type].Add(typeof(ExtGraphic<object[]>), typeof(ExtGraphic<dataType[]>));
				Translator[type].Add(typeof(ExtType<object[]>), typeof(ExtType<dataType[]>));
				Translator[type].Add(typeof(ExtAcknowledge<object[]>), typeof(ExtAcknowledge<dataType[]>));
			}
		}

		/// <summary>
		/// The start monitor.
		/// </summary>
		protected virtual void startMonitor()
		{
			if (this.ChannelEpicsType == EpicsType.Enum)
			{
				this.conn.Send(
					this.client.Codec.createSubscriptionMessage(
						this.SID, this.CID, EpicsType.String, (ushort)this.monitorDataCount, this.mask));
			}
			else
			{
				this.conn.Send(
					this.client.Codec.createSubscriptionMessage(
						this.SID, this.CID, this.ChannelEpicsType, (ushort)this.monitorDataCount, this.mask));
			}
		}

		/// <summary>
		/// The get async thread.
		/// </summary>
		/// <param name="GetListener">
		/// The get listener.
		/// </param>
		private void GetAsyncThread(object GetListener)
		{
#if DEBUG
			if (this.sid == 0)
			{
				this.waitForConnect.WaitOne();
			}

#else
                if (sid == 0)
                    if (!waitForConnect.WaitOne(client.Config.ConnectTimeout, true))
                        throw new Exception("Connection not Established in Time");
#endif

			this.waitForGet.Reset();
			this.conn.Send(
				this.client.Codec.getMessage(this.ChannelEpicsType, (UInt16)this.ChannelDataCount, this.SID, this.CID));

#if DEBUG
			this.waitForGet.WaitOne();
#else
            if (!waitForGet.WaitOne(client.Config.GetTimeout, true))
                    throw new Exception("Server did not respond in Time to the Get Request");
#endif

			((EpicsDelegate)GetListener).Invoke(this, this.lastValue);
		}

		/// <summary>
		/// The get async thread.
		/// </summary>
		/// <param name="GetListener">
		/// The get listener.
		/// </param>
		/// <typeparam name="DBRType">
		/// </typeparam>
		private void GetAsyncThread<DBRType>(object GetListener)
		{
#if DEBUG
			if (this.sid == 0)
			{
				this.waitForConnect.WaitOne();
			}

#else
                if (sid == 0)
                    if (!waitForConnect.WaitOne(client.Config.ConnectTimeout, true))
                        throw new Exception("Connection not Established in Time");
#endif

			this.waitForGet.Reset();
			this.conn.Send(this.client.Codec.getMessage(typeof(DBRType), (UInt16)this.ChannelDataCount, this.SID, this.CID));

#if DEBUG
			this.waitForGet.WaitOne();
#else
            if (!waitForGet.WaitOne(client.Config.GetTimeout, true))
                    throw new Exception("Server did not respond in Time to the Get Request");
#endif

			((EpicsDelegate)GetListener).Invoke(this, this.lastValue);
		}

		/// <summary>
		/// The put.
		/// </summary>
		/// <param name="value">
		/// The value.
		/// </param>
		private void Put(object value)
		{
#if DEBUG
			if (this.sid == 0)
			{
				this.waitForConnect.WaitOne();
			}

#else
                if (sid == 0)
                    if (!waitForConnect.WaitOne(client.Config.ConnectTimeout, true))
                        throw new Exception("Connection not Established in Time");
#endif

			this.conn.Send(this.client.Codec.setMessage(value, value.GetType(), this.SID, this.CID));
		}

		/// <summary>
		/// The put.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		/// <typeparam name="dataType">
		/// </typeparam>
		private void Put<dataType>(dataType[] values)
		{
#if DEBUG
			if (this.sid == 0)
			{
				this.waitForConnect.WaitOne();
			}

#else
                if (sid == 0)
                    if (!waitForConnect.WaitOne(client.Config.ConnectTimeout, true))
                        throw new Exception("Connection not Established in Time");
#endif

			this.conn.Send(this.client.Codec.setMessage(values, values.GetType().GetElementType(), this.SID, this.CID));
		}

		/// <summary>
		/// The put a sync threaded.
		/// </summary>
		/// <param name="parameters">
		/// The parameters.
		/// </param>
		private void PutASyncThreaded(object parameters)
		{
			var vars = (object[])parameters;

#if DEBUG
			if (this.sid == 0)
			{
				this.waitForConnect.WaitOne();
			}

#else
                if (sid == 0)
                    if (!waitForConnect.WaitOne(client.Config.ConnectTimeout, true))
                        throw new Exception("Connection not Established in Time");
#endif

			this.conn.Send(this.client.Codec.setMessage(vars[0], vars[0].GetType(), this.SID, this.CID));
			this.waitForPut.Reset();

#if DEBUG
			this.waitForPut.WaitOne();
#else
                if (!waitForPut.WaitOne(client.Config.PutTimeout, true))
                    throw new Exception("PUT not succeded in Time");
#endif

			((EpicsPutDelegate)vars[1]).Invoke(this, true);
		}

		/// <summary>
		/// The put a sync threaded.
		/// </summary>
		/// <param name="parameters">
		/// The parameters.
		/// </param>
		/// <typeparam name="dataType">
		/// </typeparam>
		private void PutASyncThreaded<dataType>(object parameters)
		{
			var vars = (object[])parameters;

#if DEBUG
			if (this.sid == 0)
			{
				this.waitForConnect.WaitOne();
			}

#else
                if (sid == 0)
                    if (!waitForConnect.WaitOne(client.Config.ConnectTimeout))
                        ((EpicsPutDelegate)vars[1]).Invoke(this, false);
#endif

			this.conn.Send(
				this.client.Codec.setMessage((dataType[])vars[0], vars[0].GetType().GetElementType(), this.SID, this.CID));
			this.waitForPut.Reset();
#if DEBUG
			this.waitForPut.WaitOne();
#else
                if (!waitForPut.WaitOne(client.Config.PutTimeout))
                    ((EpicsPutDelegate)vars[1]).Invoke(this, false);
#endif

			((EpicsPutDelegate)vars[1]).Invoke(this, true);
		}

		/// <summary>
		/// The put async.
		/// </summary>
		/// <param name="value">
		/// The value.
		/// </param>
		/// <param name="callBack">
		/// The call back.
		/// </param>
		private void PutAsync(object value, EpicsPutDelegate callBack)
		{
			var newThread = new Thread(this.PutASyncThreaded);
			newThread.Start(new[] { value, callBack });
		}

		/// <summary>
		/// The put async.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		/// <param name="callBack">
		/// The call back.
		/// </param>
		/// <typeparam name="dataType">
		/// </typeparam>
		private void PutAsync<dataType>(dataType[] values, EpicsPutDelegate callBack)
		{
			var newThread = new Thread(this.PutASyncThreaded<dataType>);
			newThread.Start(new object[] { values, callBack });
		}

		/// <summary>
		/// The put sync.
		/// </summary>
		/// <param name="value">
		/// The value.
		/// </param>
		private void PutSync(object value)
		{
#if DEBUG
			if (this.sid == 0)
			{
				this.waitForConnect.WaitOne();
			}

#else
                if (sid == 0)
                    if (!waitForConnect.WaitOne(client.Config.ConnectTimeout, true))
                        throw new Exception("Connection not Established in Time");
#endif

			this.conn.Send(this.client.Codec.setMessage(value, value.GetType(), this.SID, this.CID, true));
			this.waitForPut.Reset();

#if DEBUG
			this.waitForPut.WaitOne();
#else
            if (!waitForPut.WaitOne(client.Config.PutTimeout, true))
                    throw new Exception("PUT not succeded in Time");
#endif
		}

		/// <summary>
		/// The put sync.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		/// <typeparam name="dataType">
		/// </typeparam>
		private void PutSync<dataType>(dataType[] values)
		{
#if DEBUG
			if (this.sid == 0)
			{
				this.waitForConnect.WaitOne();
			}

#else
                if (sid == 0)
                    if (!waitForConnect.WaitOne(client.Config.ConnectTimeout, true))
                        throw new Exception("Connection not Established in Time");
#endif

			this.conn.Send(this.client.Codec.setMessage(values, values.GetType().GetElementType(), this.SID, this.CID, true));
#if DEBUG
			this.waitForPut.WaitOne();
#else
                if (!waitForPut.WaitOne(client.Config.PutTimeout, true))
                    throw new Exception("PUT not succeded in Time");
#endif
		}
	}
}