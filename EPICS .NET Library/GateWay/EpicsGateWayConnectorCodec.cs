// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsGateWayConnectorCodec.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.GateWay
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Net;

	using Epics.Base;
	using Epics.Base.Constants;

	#endregion

	/// <summary>
	/// The epics gate way connector codec.
	/// </summary>
	internal class EpicsGateWayConnectorCodec : EpicsCodec
	{
		/// <summary>
		///   The gate way.
		/// </summary>
		private readonly EpicsGateWay GateWay;

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsGateWayConnectorCodec"/> class.
		/// </summary>
		/// <param name="gw">
		/// The gw.
		/// </param>
		internal EpicsGateWayConnectorCodec(EpicsGateWay gw)
		{
			this.GateWay = gw;
		}

		/// <summary>
		/// The close channel message.
		/// </summary>
		/// <param name="channelId">
		/// The channel id.
		/// </param>
		/// <param name="iocId">
		/// The ioc id.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] closeChannelMessage(uint channelId, uint iocId)
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);

			// add the versioning message
			mem.Capacity = 16;
			writer.Write(NetworkByteConverter.ToByteArray(CommandID.CA_PROTO_CLEAR_CHANNEL));
			writer.Write(new byte[6]);
			writer.Write(NetworkByteConverter.ToByteArray(iocId));
			writer.Write(NetworkByteConverter.ToByteArray(channelId));

			var buffer = mem.GetBuffer();
			writer.Close();
			mem.Dispose();

			return buffer;
		}

		/// <summary>
		/// The close subscription message.
		/// </summary>
		/// <param name="iocId">
		/// The ioc id.
		/// </param>
		/// <param name="subscriptionId">
		/// The subscription id.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] closeSubscriptionMessage(uint iocId, uint subscriptionId)
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);

			mem.Capacity = 16;
			writer.Write(NetworkByteConverter.ToByteArray(CommandID.CA_PROTO_EVENT_CANCEL));
			writer.Write(new byte[6]);
			writer.Write(NetworkByteConverter.ToByteArray(iocId));
			writer.Write(NetworkByteConverter.ToByteArray(subscriptionId));
			var buffer = mem.GetBuffer();
			writer.Close();
			mem.Dispose();

			return buffer;
		}

		/// <summary>
		/// The create subscription message.
		/// </summary>
		/// <param name="header">
		/// The header.
		/// </param>
		/// <param name="payload">
		/// The payload.
		/// </param>
		/// <param name="subscriptionId">
		/// The subscription id.
		/// </param>
		/// <param name="iocId">
		/// The ioc id.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] createSubscriptionMessage(byte[] header, byte[] payload, uint subscriptionId, uint iocId)
		{
			var message = new byte[header.Length + payload.Length];

			Buffer.BlockCopy(header, 0, message, 0, header.Length);
			Buffer.BlockCopy(NetworkByteConverter.ToByteArray(iocId), 0, message, 8, 4);
			Buffer.BlockCopy(NetworkByteConverter.ToByteArray(subscriptionId), 0, message, 12, 4);
			Buffer.BlockCopy(payload, 0, message, header.Length, payload.Length);

			return message;
		}

		/// <summary>
		/// The get message.
		/// </summary>
		/// <param name="header">
		/// The header.
		/// </param>
		/// <param name="jobId">
		/// The job id.
		/// </param>
		/// <param name="iocId">
		/// The ioc id.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] getMessage(byte[] header, uint jobId, uint iocId)
		{
			Buffer.BlockCopy(NetworkByteConverter.ToByteArray(iocId), 0, header, 8, 4);
			Buffer.BlockCopy(NetworkByteConverter.ToByteArray(jobId), 0, header, 12, 4);

			return header;
		}

		/// <summary>
		/// The put message.
		/// </summary>
		/// <param name="header">
		/// The header.
		/// </param>
		/// <param name="payload">
		/// The payload.
		/// </param>
		/// <param name="jobId">
		/// The job id.
		/// </param>
		/// <param name="iocId">
		/// The ioc id.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] putMessage(byte[] header, byte[] payload, uint jobId, uint iocId)
		{
			var message = new byte[header.Length + payload.Length];

			Buffer.BlockCopy(header, 0, message, 0, header.Length);
			Buffer.BlockCopy(NetworkByteConverter.ToByteArray(iocId), 0, message, 8, 4);
			Buffer.BlockCopy(NetworkByteConverter.ToByteArray(jobId), 0, message, 12, 4);
			Buffer.BlockCopy(payload, 0, message, header.Length, payload.Length);

			return message;
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
		internal byte[] searchPackage(string Channelname, uint CID)
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
			writer.Write(NetworkByteConverter.ToByteArray(CID));
			writer.Write(NetworkByteConverter.ToByteArray(CID));
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
		internal byte[] searchPackage(Queue<KeyValuePair<uint, string>> channelList)
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);
			KeyValuePair<uint, string> pair;
			var padding = 0;
			var counter = 0;

			writer.Write(cVersionMessage);

			// it's something which should in 99% work but is not clean
			mem.Capacity = 65536;

			while (counter < this.GateWay.Config.ChannelSearchMaxPackageSize && channelList.Count > 0)
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
				writer.Write(NetworkByteConverter.ToByteArray(pair.Key));
				writer.Write(NetworkByteConverter.ToByteArray(pair.Key));
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
		/// The start channel message.
		/// </summary>
		/// <param name="Channelname">
		/// The channelname.
		/// </param>
		/// <param name="gateWayChannelId">
		/// The gate way channel id.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] startChannelMessage(string Channelname, uint gateWayChannelId)
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
			writer.Write(NetworkByteConverter.ToByteArray(gateWayChannelId));
			writer.Write(NetworkByteConverter.ToByteArray((UInt32)CAConstants.CA_MINOR_PROTOCOL_REVISION));
			writer.Write(NetworkByteConverter.ToByteArray(Channelname));
			writer.Write(new byte[padding]);

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
			JobHandle info;
			this.GateWay.Statistic.BytesReceived += header.Length + payload.Length;

			switch (CommandId)
			{
				case CommandID.CA_PROTO_SEARCH:

					// passing CID,IPEndPoint and the Port
					this.GateWay.FoundChannel(Parameter2, iep, DataType);
					break;

				case CommandID.CA_PROTO_CREATE_CHAN:
					this.GateWay.ChannelListIocId[Parameter1].CreateMessageAnswer = header;
					this.GateWay.ChannelListIocId[Parameter1].IocChanId = Parameter2;
					break;

				case CommandID.CA_PROTO_READ:
				case CommandID.CA_PROTO_READ_NOTIFY:
					info = this.GateWay.HandleOpenJob(Parameter2);
					if (info.IoId == 0)
					{
						return;
					}

					this.GateWay.TCPConnections[info.Address].Send(
						this.GateWay.ReceiverCodec.channelReadMessage(info.ClientId, info.IoId, header, payload));
					break;

				case CommandID.CA_PROTO_WRITE_NOTIFY:
					info = this.GateWay.HandleOpenJob(Parameter2);
					if (info.IoId == 0)
					{
						return;
					}

					this.GateWay.TCPConnections[info.Address].Send(this.GateWay.ReceiverCodec.channelWroteMessage(info.IoId, header));
					break;

				case CommandID.CA_PROTO_EVENT_ADD:
					if (this.GateWay.SubscriptionList.ContainsKey(Parameter2))
					{
						this.GateWay.SubscriptionList[Parameter2].ForwardMessage(header, payload);
					}

					break;

				case CommandID.CA_PROTO_ACCESS_RIGHTS:
					this.GateWay.ChannelListIocId[Parameter1].AccessRights = (AccessRights)Parameter2;
					break;

					// failed to created a Channel.
				case CommandID.CA_PROTO_CREATE_CH_FAIL:
					this.GateWay.ChannelListIocId[Parameter1].Dispose();
					break;
				case CommandID.CA_PROTO_SERVER_DISCONN:
					this.GateWay.ChannelListIocId[Parameter1].Dispose();
					break;

				case CommandID.CA_PROTO_RSRV_IS_UP:

					break;

				case CommandID.CA_PROTO_VERSION:
				case CommandID.CA_PROTO_ECHO:
				case CommandID.CA_PROTO_CLEAR_CHANNEL:
					break;
				case CommandID.CA_PROTO_ERROR:
					break;
				default:
					return;
			}
		}
	}
}