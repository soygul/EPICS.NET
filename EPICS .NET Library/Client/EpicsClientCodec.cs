// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsClientCodec.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Client
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Net;

	using Epics.Base;
	using Epics.Base.Constants;

	/// <summary>
	/// The epics client codec.
	/// </summary>
	internal class EpicsClientCodec : EpicsCodec
	{
		/// <summary>
		///   The client.
		/// </summary>
		private readonly EpicsClient client;

		/// <summary>
		///   The tmp chan.
		/// </summary>
		private EpicsChannel tmpChan;

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsClientCodec"/> class.
		/// </summary>
		/// <param name="client">
		/// The client.
		/// </param>
		public EpicsClientCodec(EpicsClient client)
		{
			this.client = client;
		}

		/// <summary>
		/// The close channel message.
		/// </summary>
		/// <param name="SID">
		/// The sid.
		/// </param>
		/// <param name="CID">
		/// The cid.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] closeChannelMessage(uint SID, uint CID)
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);

			// add the versioning message
			mem.Capacity = 16;
			writer.Write(NetworkByteConverter.ToByteArray(CommandID.CA_PROTO_CLEAR_CHANNEL));
			writer.Write(new byte[6]);
			writer.Write(NetworkByteConverter.ToByteArray(SID));
			writer.Write(NetworkByteConverter.ToByteArray(CID));

			var buffer = mem.GetBuffer();
			writer.Close();
			mem.Dispose();

			return buffer;
		}

		/// <summary>
		/// The create channel message.
		/// </summary>
		/// <param name="Channelname">
		/// The channelname.
		/// </param>
		/// <param name="CID">
		/// The cid.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] createChannelMessage(string Channelname, uint CID)
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);

			var padding = 0;

			if (Channelname.Length % 8 == 0)
			{
				padding = 8;
			}
			else
			{
				padding = 8 - (Channelname.Length % 8);
			}

			mem.Capacity = 16 + Channelname.Length + padding;

			// add the versioning message
			writer.Write(NetworkByteConverter.ToByteArray(CommandID.CA_PROTO_CREATE_CHAN));
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)(Channelname.Length + padding)));
			writer.Write(new byte[4]);
			writer.Write(NetworkByteConverter.ToByteArray(CID));
			writer.Write(NetworkByteConverter.ToByteArray((UInt32)CAConstants.CA_MINOR_PROTOCOL_REVISION));
			writer.Write(NetworkByteConverter.ToByteArray(Channelname));
			writer.Write(new byte[padding]);

			var buffer = mem.GetBuffer();
			writer.Close();
			mem.Dispose();

			return buffer;
		}

		/// <summary>
		/// The create subscription message.
		/// </summary>
		/// <param name="SID">
		/// The sid.
		/// </param>
		/// <param name="SubscriptionID">
		/// The subscription id.
		/// </param>
		/// <param name="type">
		/// The type.
		/// </param>
		/// <param name="dataCount">
		/// The data count.
		/// </param>
		/// <param name="mask">
		/// The mask.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] createSubscriptionMessage(
			uint SID, uint SubscriptionID, EpicsType type, ushort dataCount, MonitorMask mask)
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);

			mem.Capacity = 32;
			writer.Write(NetworkByteConverter.ToByteArray(CommandID.CA_PROTO_EVENT_ADD));
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)16));
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)type));
			writer.Write(NetworkByteConverter.ToByteArray(dataCount));
			writer.Write(NetworkByteConverter.ToByteArray(SID));
			writer.Write(NetworkByteConverter.ToByteArray(SubscriptionID));
			writer.Write(new byte[12]);
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)mask));
			var buffer = mem.GetBuffer();
			writer.Close();
			mem.Dispose();

			return buffer;
		}

		/// <summary>
		/// The create subscription message.
		/// </summary>
		/// <param name="SID">
		/// The sid.
		/// </param>
		/// <param name="SubscriptionID">
		/// The subscription id.
		/// </param>
		/// <param name="type">
		/// The type.
		/// </param>
		/// <param name="dataCount">
		/// The data count.
		/// </param>
		/// <param name="mask">
		/// The mask.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] createSubscriptionMessage(
			uint SID, uint SubscriptionID, Type type, ushort dataCount, MonitorMask mask)
		{
			if (type.IsArray)
			{
				if (CTypeTranslator.ContainsKey(type.GetElementType()))
				{
					return this.createSubscriptionMessage(SID, SubscriptionID, CTypeTranslator[type.GetElementType()], dataCount, mask);
				}
				else
				{
					this.client.ExceptionContainer.Add(new Exception("Requested datatype which is not accepted by Epics"));
					return this.createSubscriptionMessage(SID, SubscriptionID, EpicsType.String, dataCount, mask);
				}
			}
			else if (CTypeTranslator.ContainsKey(type))
			{
				return this.createSubscriptionMessage(SID, SubscriptionID, CTypeTranslator[type], dataCount, mask);
			}
			else
			{
				this.client.ExceptionContainer.Add(new Exception("Requested datatype which is not accepted by Epics"));
				return this.createSubscriptionMessage(SID, SubscriptionID, EpicsType.String, dataCount, mask);
			}
		}

		/// <summary>
		/// The get message.
		/// </summary>
		/// <param name="type">
		/// The type.
		/// </param>
		/// <param name="count">
		/// The count.
		/// </param>
		/// <param name="SID">
		/// The sid.
		/// </param>
		/// <param name="CID">
		/// The cid.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] getMessage(EpicsType type, ushort count, uint SID, uint CID)
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);

			mem.Capacity = 16;
			writer.Write(NetworkByteConverter.ToByteArray(CommandID.CA_PROTO_READ_NOTIFY));
			writer.Write(new byte[2]);
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)type));
			writer.Write(NetworkByteConverter.ToByteArray(count));
			writer.Write(NetworkByteConverter.ToByteArray(SID));
			writer.Write(NetworkByteConverter.ToByteArray(CID));

			var buffer = mem.GetBuffer();
			writer.Close();
			mem.Dispose();

			return buffer;
		}

		/// <summary>
		/// The get message.
		/// </summary>
		/// <param name="type">
		/// The type.
		/// </param>
		/// <param name="count">
		/// The count.
		/// </param>
		/// <param name="SID">
		/// The sid.
		/// </param>
		/// <param name="CID">
		/// The cid.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] getMessage(Type type, ushort count, uint SID, uint CID)
		{
			if (type.IsArray)
			{
				if (CTypeTranslator.ContainsKey(type.GetElementType()))
				{
					return this.getMessage(CTypeTranslator[type.GetElementType()], count, SID, CID);
				}
				else
				{
					this.client.ExceptionContainer.Add(new Exception("Requested datatype which is not accepted by Epics"));
					return this.getMessage(EpicsType.String, count, SID, CID);
				}
			}
			else if (CTypeTranslator.ContainsKey(type))
			{
				return this.getMessage(CTypeTranslator[type], count, SID, CID);
			}
			else
			{
				this.client.ExceptionContainer.Add(new Exception("Requested datatype which is not accepted by Epics"));
				return this.getMessage(EpicsType.String, count, SID, CID);
			}
		}

		/// <summary>
		/// Builds a EPICS-Package for Searching a Channel.
		///   it contains 2 Messages: CA_PROTO_VERSION and CA_PROTO_SEARCH
		/// </summary>
		/// <param name="Channelname">
		/// String Name of a searched Channel
		/// </param>
		/// <param name="CID">
		/// CID of the given Channel
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] searchPackage(string Channelname, int CID)
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);

			var padding = 0;

			if (Channelname.Length % 8 == 0)
			{
				padding = 8;
			}
			else
			{
				padding = 8 - (Channelname.Length % 8);
			}

			mem.Capacity = 16 + 16 + Channelname.Length + padding;

			// add the versioning message
			writer.Write(cVersionMessage);
			writer.Write(NetworkByteConverter.ToByteArray(CommandID.CA_PROTO_SEARCH));
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)(Channelname.Length + padding)));
			writer.Write(NetworkByteConverter.ToByteArray(CAConstants.DONT_REPLY));
			writer.Write(NetworkByteConverter.ToByteArray(CAConstants.CA_MINOR_PROTOCOL_REVISION));
			writer.Write(NetworkByteConverter.ToByteArray((UInt32)CID));
			writer.Write(NetworkByteConverter.ToByteArray((UInt32)CID));
			writer.Write(NetworkByteConverter.ToByteArray(Channelname));
			writer.Write(new byte[padding]);

			var buffer = mem.GetBuffer();
			writer.Close();
			mem.Dispose();

			return buffer;
		}

		/// <summary>
		/// Builds a EPICS-Package for Searching multiple Channel.
		///   it contains 2 types of messages:
		///   a leading CA_PROTO_VERSION and then for every channel a
		///   CA_PROTO_SEARCH
		/// </summary>
		/// <param name="channelList">
		/// The channel List.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] searchPackage(Queue<KeyValuePair<int, string>> channelList)
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);
			KeyValuePair<int, string> pair;
			var padding = 0;
			var counter = 0;

			writer.Write(cVersionMessage);

			// it's something which should in 99% work but is not clean
			mem.Capacity = 65536;

			while (counter < this.client.Config.ChannelSearchMaxPackageSize && channelList.Count > 0)
			{
				lock (channelList)
				{
					pair = channelList.Dequeue();
				}

				counter++;

				if (pair.Value.Length % 8 == 0)
				{
					padding = 8;
				}
				else
				{
					padding = 8 - (pair.Value.Length % 8);
				}

				// add the versioning message
				writer.Write(NetworkByteConverter.ToByteArray(CommandID.CA_PROTO_SEARCH));
				writer.Write(NetworkByteConverter.ToByteArray((UInt16)(pair.Value.Length + padding)));
				writer.Write(NetworkByteConverter.ToByteArray(CAConstants.DONT_REPLY));
				writer.Write(NetworkByteConverter.ToByteArray(CAConstants.CA_MINOR_PROTOCOL_REVISION));
				writer.Write(NetworkByteConverter.ToByteArray((UInt32)pair.Key));
				writer.Write(NetworkByteConverter.ToByteArray((UInt32)pair.Key));
				writer.Write(NetworkByteConverter.ToByteArray(pair.Value));
				writer.Write(new byte[padding]);
			}

			mem.Capacity = (int)mem.Position;
			var buffer = mem.GetBuffer();
			writer.Close();
			mem.Dispose();

			return buffer;
		}

		/// <summary>
		/// The set message.
		/// </summary>
		/// <param name="value">
		/// The value.
		/// </param>
		/// <param name="type">
		/// The type.
		/// </param>
		/// <param name="SID">
		/// The sid.
		/// </param>
		/// <param name="CID">
		/// The cid.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] setMessage(object value, EpicsType type, uint SID, uint CID)
		{
			return setMessage(value, type, SID, CID, false);
		}

		/// <summary>
		/// The set message.
		/// </summary>
		/// <param name="value">
		/// The value.
		/// </param>
		/// <param name="type">
		/// The type.
		/// </param>
		/// <param name="SID">
		/// The sid.
		/// </param>
		/// <param name="CID">
		/// The cid.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] setMessage(object value, Type type, uint SID, uint CID)
		{
			return setMessage(value, type, SID, CID, false);
		}

		/// <summary>
		/// The set message.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		/// <param name="type">
		/// The type.
		/// </param>
		/// <param name="SID">
		/// The sid.
		/// </param>
		/// <param name="CID">
		/// The cid.
		/// </param>
		/// <typeparam name="dataType">
		/// </typeparam>
		/// <returns>
		/// </returns>
		internal byte[] setMessage<dataType>(dataType[] values, EpicsType type, uint SID, uint CID)
		{
			return setMessage(values, type, SID, CID, false);
		}

		/// <summary>
		/// The set message.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		/// <param name="type">
		/// The type.
		/// </param>
		/// <param name="SID">
		/// The sid.
		/// </param>
		/// <param name="CID">
		/// The cid.
		/// </param>
		/// <typeparam name="dataType">
		/// </typeparam>
		/// <returns>
		/// </returns>
		internal byte[] setMessage<dataType>(dataType[] values, Type type, uint SID, uint CID)
		{
			return setMessage(values, type, SID, CID, false);
		}

		/// <summary>
		/// The set message.
		/// </summary>
		/// <param name="value">
		/// The value.
		/// </param>
		/// <param name="type">
		/// The type.
		/// </param>
		/// <param name="SID">
		/// The sid.
		/// </param>
		/// <param name="CID">
		/// The cid.
		/// </param>
		/// <param name="callBack">
		/// The call back.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] setMessage(object value, EpicsType type, uint SID, uint CID, bool callBack)
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);

			var payload = NetworkByteConverter.objectToByte(value, type);

			var padding = 0;

			if (payload.Length % 8 == 0)
			{
				padding = 8;
			}
			else
			{
				padding = 8 - (payload.Length % 8);
			}

			mem.Capacity = 16 + payload.Length + padding;

			// add the versioning message
			writer.Write(NetworkByteConverter.ToByteArray(callBack ? CommandID.CA_PROTO_WRITE_NOTIFY : CommandID.CA_PROTO_WRITE));
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)(payload.Length + padding)));
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)type));
			writer.Write(new byte[2] { 0, 1 });
			writer.Write(NetworkByteConverter.ToByteArray(SID));
			writer.Write(NetworkByteConverter.ToByteArray(CID));
			writer.Write(payload);
			writer.Write(new byte[padding]);

			var buffer = mem.GetBuffer();
			writer.Close();
			mem.Dispose();

			return buffer;
		}

		/// <summary>
		/// The set message.
		/// </summary>
		/// <param name="value">
		/// The value.
		/// </param>
		/// <param name="type">
		/// The type.
		/// </param>
		/// <param name="SID">
		/// The sid.
		/// </param>
		/// <param name="CID">
		/// The cid.
		/// </param>
		/// <param name="callBack">
		/// The call back.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] setMessage(object value, Type type, uint SID, uint CID, bool callBack)
		{
			if (type.IsArray)
			{
				if (CTypeTranslator.ContainsKey(type.GetElementType()))
				{
					return this.setMessage(value, CTypeTranslator[type.GetElementType()], SID, CID);
				}
				else
				{
					this.client.ExceptionContainer.Add(new Exception("Requested datatype which is not accepted by Epics"));
					return this.setMessage(value, EpicsType.String, SID, CID);
				}
			}
			else if (CTypeTranslator.ContainsKey(type))
			{
				return this.setMessage(value, CTypeTranslator[type], SID, CID);
			}
			else
			{
				this.client.ExceptionContainer.Add(new Exception("Requested datatype which is not accepted by Epics"));
				return this.setMessage(value, EpicsType.String, SID, CID);
			}
		}

		/// <summary>
		/// The set message.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		/// <param name="type">
		/// The type.
		/// </param>
		/// <param name="SID">
		/// The sid.
		/// </param>
		/// <param name="CID">
		/// The cid.
		/// </param>
		/// <param name="callBack">
		/// The call back.
		/// </param>
		/// <typeparam name="dataType">
		/// </typeparam>
		/// <returns>
		/// </returns>
		internal byte[] setMessage<dataType>(dataType[] values, EpicsType type, uint SID, uint CID, bool callBack)
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);

			// possible max length = extended header (16byte) + count*40byte
			mem.Capacity = 16 + values.Length * 40;

			// jump to the end of the header.
			mem.Position = 16;

			foreach (var value in values)
			{
				writer.Write(NetworkByteConverter.objectToByte(value, type));
			}

			// shrink to what we really need
			mem.Capacity = (int)mem.Position;

			// jump to the beginning
			mem.Position = 0;
			writer.Write(NetworkByteConverter.ToByteArray(callBack ? CommandID.CA_PROTO_WRITE_NOTIFY : CommandID.CA_PROTO_WRITE));
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)(mem.Capacity - 16)));
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)type));
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)values.Length));
			writer.Write(NetworkByteConverter.ToByteArray(SID));
			writer.Write(NetworkByteConverter.ToByteArray(CID));

			var buffer = mem.GetBuffer();
			writer.Close();
			mem.Dispose();

			return buffer;
		}

		/// <summary>
		/// The set message.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		/// <param name="type">
		/// The type.
		/// </param>
		/// <param name="SID">
		/// The sid.
		/// </param>
		/// <param name="CID">
		/// The cid.
		/// </param>
		/// <param name="callBack">
		/// The call back.
		/// </param>
		/// <typeparam name="dataType">
		/// </typeparam>
		/// <returns>
		/// </returns>
		internal byte[] setMessage<dataType>(dataType[] values, Type type, uint SID, uint CID, bool callBack)
		{
			if (type.IsArray)
			{
				if (CTypeTranslator.ContainsKey(type.GetElementType()))
				{
					return this.setMessage(values, CTypeTranslator[type.GetElementType()], SID, CID);
				}
				else
				{
					this.client.ExceptionContainer.Add(new Exception("Requested datatype which is not accepted by Epics"));
					return this.setMessage(values, EpicsType.String, SID, CID);
				}
			}
			else if (CTypeTranslator.ContainsKey(type))
			{
				return this.setMessage(values, CTypeTranslator[type], SID, CID);
			}
			else
			{
				this.client.ExceptionContainer.Add(new Exception("Requested datatype which is not accepted by Epics"));
				return this.setMessage(values, EpicsType.String, SID, CID);
			}
		}

		/// <summary>
		/// The stop subscription message.
		/// </summary>
		/// <param name="SID">
		/// The sid.
		/// </param>
		/// <param name="SubscriptionID">
		/// The subscription id.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] stopSubscriptionMessage(uint SID, uint SubscriptionID)
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);

			mem.Capacity = 16;
			writer.Write(NetworkByteConverter.ToByteArray(CommandID.CA_PROTO_EVENT_CANCEL));
			writer.Write(new byte[6]);
			writer.Write(NetworkByteConverter.ToByteArray(SID));
			writer.Write(NetworkByteConverter.ToByteArray(SubscriptionID));
			var buffer = mem.GetBuffer();
			writer.Close();
			mem.Dispose();

			return buffer;
		}

		/// <summary>
		/// The handle message.
		/// </summary>
		/// <param name="CommandId">
		/// The command id.
		/// </param>
		/// <param name="DataType">
		/// The data type.
		/// </param>
		/// <param name="PayloadSize">
		/// The payload size.
		/// </param>
		/// <param name="DataCount">
		/// The data count.
		/// </param>
		/// <param name="Parameter1">
		/// The parameter 1.
		/// </param>
		/// <param name="Parameter2">
		/// The parameter 2.
		/// </param>
		/// <param name="header">
		/// The header.
		/// </param>
		/// <param name="payload">
		/// The payload.
		/// </param>
		/// <param name="iep">
		/// The iep.
		/// </param>
		protected override void HandleMessage(
			ushort CommandId, 
			ushort DataType, 
			ref uint PayloadSize, 
			ref uint DataCount, 
			ref uint Parameter1, 
			ref uint Parameter2, 
			ref byte[] header, 
			ref byte[] payload, 
			ref EndPoint iep)
		{
			switch (CommandId)
			{
					// Got Back Value for Get
				case CommandID.CA_PROTO_READ_NOTIFY:
					if (DataCount == 0)
					{
						this.client.cidChannels[(int)Parameter2].LastValue = null;
					}
					else if (DataCount == 1)
					{
						this.client.cidChannels[(int)Parameter2].LastValue = NetworkByteConverter.byteToObject(
							payload, (EpicsType)DataType);
					}
					else
					{
						this.client.cidChannels[(int)Parameter2].LastValue = NetworkByteConverter.byteToObject(
							payload, (EpicsType)DataType, (int)DataCount);
					}

					break;

					// Got Status Back for Put
				case CommandID.CA_PROTO_WRITE_NOTIFY:
					if (this.client.cidChannels.ContainsKey((int)Parameter2))
					{
						this.client.cidChannels[(int)Parameter2].writeSucceeded();
					}

					break;

					// Did receive a Monitor Signal
				case CommandID.CA_PROTO_EVENT_ADD:
					if (DataCount == 0)
					{
						this.client.cidChannels[(int)Parameter2].receiveValueUpdate(null);
					}
					else if (DataCount == 1)
					{
						this.client.cidChannels[(int)Parameter2].receiveValueUpdate(
							NetworkByteConverter.byteToObject(payload, (EpicsType)DataType));
					}
					else
					{
						this.client.cidChannels[(int)Parameter2].receiveValueUpdate(
							NetworkByteConverter.byteToObject(payload, (EpicsType)DataType, (int)DataCount));
					}

					break;

					// Got information about Access Rights
				case CommandID.CA_PROTO_ACCESS_RIGHTS:
					this.client.cidChannels[(int)Parameter1].AccessRight = (AccessRights)Parameter2;
					break;

					// Could register a Channel on the IOC
				case CommandID.CA_PROTO_CREATE_CHAN:
					try
					{
						this.tmpChan = this.client.cidChannels[(int)Parameter1];
						this.tmpChan.ChannelEpicsType = (EpicsType)DataType;
						this.tmpChan.ChannelDefinedType = CETypeTranslator[(EpicsType)DataType];
						this.tmpChan.ChannelDataCount = DataCount;
						if (this.tmpChan.MonitorDataCount == 0)
						{
							this.tmpChan.MonitorDataCount = DataCount;
						}

						this.tmpChan.SID = Parameter2;
					}
					catch (Exception e)
					{
					}

					break;

					// failed to created a Channel.
				case CommandID.CA_PROTO_CREATE_CH_FAIL:
					this.tmpChan = this.client.cidChannels[(int)Parameter1];
					this.tmpChan.Conn = null;
					break;
				case CommandID.CA_PROTO_SERVER_DISCONN:
					this.client.cidChannels[(int)Parameter1].conn_ConnectionStateChanged(false);
					break;
				case CommandID.CA_PROTO_SEARCH:

					// if a channel is disposed it will fail here.
					try
					{
						var riep = new IPEndPoint(((IPEndPoint)iep).Address, DataType);
						this.client.cidChannels[(int)Parameter2].Conn = this.client.GetServerConnection(riep);
					}
					catch (Exception e)
					{
						this.client.ExceptionContainer.Add(new Exception("MINOR: FOUND CHANNEL WHICH ALREADY IS DISPOSED"));
					}

					break;
				case CommandID.CA_PROTO_RSRV_IS_UP:
					if (this.client.beaconCollection.ContainsKey(iep.ToString()))
					{
						lock (this.client.beaconCollection)
						{
							this.client.beaconCollection[iep.ToString()] = DateTime.Now;
						}
					}
					else
					{
						this.client.addIocBeaconed(iep);
					}

					break;
				case CommandID.CA_PROTO_VERSION:
				case CommandID.CA_PROTO_ECHO:
				case CommandID.CA_PROTO_CLEAR_CHANNEL:
					if (PayloadSize > 0)
					{
						this.client.ExceptionContainer.Add(
							new Exception("MESSAGE WITHOUT PAYLOAD, REQUESTED PAYLOAD. POSSIBLE MISSREADING!"));
					}

					break;
				case CommandID.CA_PROTO_ERROR:
					this.client.ExceptionContainer.Add(
						new Exception("EPICS-ERROR: " + Parameter2 + " - " + NetworkByteConverter.ToString(payload, 16)));
					break;
				default:
					this.client.ExceptionContainer.Add(new Exception("NETWORK MISS READING - DROP WHOLE PACKAGE!"));
					return;
			}
		}
	}
}