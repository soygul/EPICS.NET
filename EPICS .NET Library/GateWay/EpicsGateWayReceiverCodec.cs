// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsGateWayReceiverCodec.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.GateWay
{
	#region Using Directives

	using System;
	using System.IO;
	using System.Net;

	using Epics.Base;
	using Epics.Base.Constants;

	#endregion

	/// <summary>
	/// The epics gate way receiver codec.
	/// </summary>
	internal class EpicsGateWayReceiverCodec : EpicsCodec
	{
		/// <summary>
		///   The gate way.
		/// </summary>
		private readonly EpicsGateWay GateWay;

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsGateWayReceiverCodec"/> class.
		/// </summary>
		/// <param name="gw">
		/// The gw.
		/// </param>
		internal EpicsGateWayReceiverCodec(EpicsGateWay gw)
		{
			this.GateWay = gw;
		}

		/// <summary>
		/// The channel clear message.
		/// </summary>
		/// <param name="clientId">
		/// The client id.
		/// </param>
		/// <param name="serverId">
		/// The server id.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] channelClearMessage(uint clientId, uint serverId)
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);

			mem.Capacity = 16;

			writer.Write(NetworkByteConverter.ToByteArray(CommandID.CA_PROTO_CLEAR_CHANNEL));
			writer.Write(new byte[6]);
			writer.Write(NetworkByteConverter.ToByteArray(serverId));
			writer.Write(NetworkByteConverter.ToByteArray(clientId));

			var buffer = mem.GetBuffer();
			writer.Close();
			mem.Dispose();

			return buffer;
		}

		/// <summary>
		/// The channel created message.
		/// </summary>
		/// <param name="clientId">
		/// The client id.
		/// </param>
		/// <param name="serverId">
		/// The server id.
		/// </param>
		/// <param name="access">
		/// The access.
		/// </param>
		/// <param name="header">
		/// The header.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] channelCreatedMessage(uint clientId, uint serverId, AccessRights access, byte[] header)
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);

			mem.Capacity = 32;

			writer.Write(NetworkByteConverter.ToByteArray(CommandID.CA_PROTO_ACCESS_RIGHTS));
			writer.Write(new byte[6]);
			writer.Write(NetworkByteConverter.ToByteArray(clientId));
			writer.Write(NetworkByteConverter.ToByteArray((UInt32)access));

			Buffer.BlockCopy(NetworkByteConverter.ToByteArray(clientId), 0, header, 8, 4);
			Buffer.BlockCopy(NetworkByteConverter.ToByteArray(serverId), 0, header, 12, 4);

			writer.Write(header);

			var buffer = mem.GetBuffer();
			writer.Close();
			mem.Dispose();

			return buffer;
		}

		/// <summary>
		/// The channel creation fail message.
		/// </summary>
		/// <param name="clientId">
		/// The client id.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] channelCreationFailMessage(uint clientId)
		{
			var msg = new byte[16] { 0, 26, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
			Buffer.BlockCopy(NetworkByteConverter.ToByteArray(clientId), 0, msg, 8, 4);
			return msg;
		}

		/// <summary>
		/// The channel disconnection message.
		/// </summary>
		/// <param name="clientId">
		/// The client id.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] channelDisconnectionMessage(uint clientId)
		{
			var msg = new byte[16] { 0, 27, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
			Buffer.BlockCopy(NetworkByteConverter.ToByteArray(clientId), 0, msg, 8, 4);
			return msg;
		}

		/// <summary>
		/// The channel found message.
		/// </summary>
		/// <param name="clientId">
		/// The client id.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] channelFoundMessage(uint clientId)
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);

			mem.Capacity = 40;

			writer.Write(cVersionMessage);
			writer.Write(NetworkByteConverter.ToByteArray(CommandID.CA_PROTO_SEARCH));
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)8));
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)this.GateWay.Config.TCPListenPort));
			writer.Write(new byte[2]);
			writer.Write(NetworkByteConverter.ToByteArray(0xFFFFFFFF));
			writer.Write(NetworkByteConverter.ToByteArray(clientId));
			writer.Write(NetworkByteConverter.ToByteArray(CAConstants.CA_MINOR_PROTOCOL_REVISION));

			var buffer = mem.GetBuffer();
			writer.Close();
			mem.Dispose();

			return buffer;
		}

		/// <summary>
		/// The channel read message.
		/// </summary>
		/// <param name="clientId">
		/// The client id.
		/// </param>
		/// <param name="ioId">
		/// The io id.
		/// </param>
		/// <param name="header">
		/// The header.
		/// </param>
		/// <param name="data">
		/// The data.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] channelReadMessage(uint clientId, uint ioId, byte[] header, byte[] data)
		{
			var message = new byte[header.Length + data.Length];

			Buffer.BlockCopy(header, 0, message, 0, header.Length);
			Buffer.BlockCopy(NetworkByteConverter.ToByteArray(clientId), 0, message, 8, 4);
			Buffer.BlockCopy(NetworkByteConverter.ToByteArray(ioId), 0, message, 12, 4);
			Buffer.BlockCopy(data, 0, message, header.Length, data.Length);

			return message;
		}

		/// <summary>
		/// The channel wrote message.
		/// </summary>
		/// <param name="ioId">
		/// The io id.
		/// </param>
		/// <param name="header">
		/// The header.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] channelWroteMessage(uint ioId, byte[] header)
		{
			Buffer.BlockCopy(NetworkByteConverter.ToByteArray(ioId), 0, header, 12, 4);

			return header;
		}

		/// <summary>
		/// The error message.
		/// </summary>
		/// <param name="ioId">
		/// The io id.
		/// </param>
		/// <param name="header">
		/// The header.
		/// </param>
		/// <param name="data">
		/// The data.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] errorMessage(uint ioId, byte[] header, byte[] data)
		{
			var message = new byte[header.Length + data.Length];
			Buffer.BlockCopy(header, 0, message, 0, header.Length);
			Buffer.BlockCopy(NetworkByteConverter.ToByteArray(ioId), 0, message, 8, 4);
			Buffer.BlockCopy(data, 0, message, header.Length, data.Length);

			return message;
		}

		/// <summary>
		/// The monitor change message.
		/// </summary>
		/// <param name="subscriptionId">
		/// The subscription id.
		/// </param>
		/// <param name="clientId">
		/// The client id.
		/// </param>
		/// <param name="header">
		/// The header.
		/// </param>
		/// <param name="data">
		/// The data.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] monitorChangeMessage(uint subscriptionId, uint clientId, byte[] header, byte[] data)
		{
			var message = new byte[header.Length + data.Length];

			Buffer.BlockCopy(header, 0, message, 0, header.Length);
			Buffer.BlockCopy(NetworkByteConverter.ToByteArray(clientId), 0, message, 8, 4);
			Buffer.BlockCopy(NetworkByteConverter.ToByteArray(subscriptionId), 0, message, 12, 4);
			Buffer.BlockCopy(data, 0, message, header.Length, data.Length);

			return message;
		}

		/// <summary>
		/// The monitor close message.
		/// </summary>
		/// <param name="gateWayId">
		/// The gate way id.
		/// </param>
		/// <param name="subscriptionId">
		/// The subscription id.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] monitorCloseMessage(uint gateWayId, uint subscriptionId)
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);

			mem.Capacity = 16;

			writer.Write(NetworkByteConverter.ToByteArray(CommandID.CA_PROTO_EVENT_CANCEL));
			writer.Write(new byte[6]);
			writer.Write(NetworkByteConverter.ToByteArray(gateWayId));
			writer.Write(NetworkByteConverter.ToByteArray(subscriptionId));

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
			this.GateWay.Statistic.BytesReceived += header.Length + payload.Length;

			switch (CommandId)
			{
				case CommandID.CA_PROTO_SEARCH:
					this.GateWay.SearchForChannel(Parameter1, NetworkByteConverter.ToString(payload), iep);
					this.GateWay.Statistic.Searches++;
					break;

				case CommandID.CA_PROTO_CLIENT_NAME:
					this.GateWay.TCPConnections[iep.ToString()].Username = NetworkByteConverter.ToString(payload);
					break;
				case CommandID.CA_PROTO_HOST_NAME:
					this.GateWay.TCPConnections[iep.ToString()].Hostname = NetworkByteConverter.ToString(payload);
					break;

				case CommandID.CA_PROTO_CREATE_CHAN:
					this.GateWay.CreateClientChannel(Parameter1, NetworkByteConverter.ToString(payload), iep);
					break;
				case CommandID.CA_PROTO_CLEAR_CHANNEL:
					this.GateWay.ChannelListClientId[Parameter1].Dispose();
					break;

				case CommandID.CA_PROTO_WRITE_NOTIFY:
				case CommandID.CA_PROTO_WRITE:
					this.GateWay.ChannelListClientId[Parameter1].PutAsync(Parameter2, header, payload);
					this.GateWay.Statistic.Puts++;
					break;

				case CommandID.CA_PROTO_READ:
				case CommandID.CA_PROTO_READ_NOTIFY:
					this.GateWay.ChannelListClientId[Parameter1].GetAsync(Parameter2, header);
					this.GateWay.Statistic.Gets++;
					break;

				case CommandID.CA_PROTO_EVENTS_ON:
					break;
				case CommandID.CA_PROTO_EVENTS_OFF:
					break;

				case CommandID.CA_PROTO_EVENT_ADD:
					this.GateWay.ChannelListClientId[Parameter1].StartMonitor(Parameter2, header, payload);
					break;
				case CommandID.CA_PROTO_EVENT_CANCEL:
					this.GateWay.ChannelListClientId[Parameter1].RemoveMonitor(Parameter2);
					break;

				case CommandID.CA_PROTO_ECHO:
					this.GateWay.TCPConnections[iep.ToString()].Send(CEchoMessage);
					break;
			}
		}
	}
}