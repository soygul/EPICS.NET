// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsServerCodec.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Server
{
	#region Using Directives

	using System;
	using System.IO;
	using System.Net;

	using Epics.Base;
	using Epics.Base.Constants;

	#endregion

	/// <summary>
	/// The epics server codec.
	/// </summary>
	internal class EpicsServerCodec : EpicsCodec
	{
		/// <summary>
		///   The server.
		/// </summary>
		private readonly EpicsServer Server;

		/// <summary>
		/// Initializes a new instance of the <see cref="EpicsServerCodec"/> class.
		/// </summary>
		/// <param name="server">
		/// The server.
		/// </param>
		public EpicsServerCodec(EpicsServer server)
		{
			this.Server = server;
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
		internal byte[] channelClearMessage(int clientId, int serverId)
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);

			mem.Capacity = 16;

			writer.Write(NetworkByteConverter.ToByteArray(CommandID.CA_PROTO_CLEAR_CHANNEL));
			writer.Write(new byte[6]);
			writer.Write(NetworkByteConverter.ToByteArray((UInt32)serverId));
			writer.Write(NetworkByteConverter.ToByteArray((UInt32)clientId));

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
		/// <param name="dataType">
		/// The data type.
		/// </param>
		/// <param name="dataCount">
		/// The data count.
		/// </param>
		/// <param name="access">
		/// The access.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] channelCreatedMessage(
			int clientId, int serverId, EpicsType dataType, int dataCount, AccessRights access)
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);

			mem.Capacity = 32;

			writer.Write(NetworkByteConverter.ToByteArray(CommandID.CA_PROTO_ACCESS_RIGHTS));
			writer.Write(new byte[6]);
			writer.Write(NetworkByteConverter.ToByteArray((UInt32)clientId));
			writer.Write(NetworkByteConverter.ToByteArray((UInt32)access));

			writer.Write(NetworkByteConverter.ToByteArray(CommandID.CA_PROTO_CREATE_CHAN));
			writer.Write(new byte[2]);
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)dataType));
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)dataCount));
			writer.Write(NetworkByteConverter.ToByteArray((UInt32)clientId));
			writer.Write(NetworkByteConverter.ToByteArray((UInt32)serverId));

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
		internal byte[] channelCreationFailMessage(int clientId)
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
		internal byte[] channelDisconnectionMessage(int clientId)
		{
			var msg = new byte[16] { 0, 27, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
			Buffer.BlockCopy(NetworkByteConverter.ToByteArray(clientId), 0, msg, 8, 4);
			return msg;
		}

		/// <summary>
		/// The channel read message.
		/// </summary>
		/// <param name="clientId">
		/// IMPORTANT IT's not sure yet that this has to be the cliendId could also be the ioId
		/// </param>
		/// <param name="ioId">
		/// IMPORTANT IT's not sure yet that this has to be the ioId could also be the cliendId
		/// </param>
		/// <param name="dataType">
		/// </param>
		/// <param name="dataCount">
		/// </param>
		/// <param name="data">
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] channelReadMessage(int clientId, int ioId, EpicsType dataType, int dataCount, byte[] data)
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);

			var padding = 0;

			if (data.Length % 8 == 0)
			{
				padding = 8;
			}
			else
			{
				padding = 8 - (data.Length % 8);
			}

			mem.Capacity = 16 + data.Length + padding;

			writer.Write(NetworkByteConverter.ToByteArray(CommandID.CA_PROTO_READ_NOTIFY));
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)(data.Length + padding)));
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)dataType));
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)dataCount));
			writer.Write(NetworkByteConverter.ToByteArray((UInt32)clientId));
			writer.Write(NetworkByteConverter.ToByteArray((UInt32)ioId));

			writer.Write(data);
			writer.Write(new byte[padding]);

			var buffer = mem.GetBuffer();
			writer.Close();
			mem.Dispose();

			return buffer;
		}

		/// <summary>
		/// The channel wrote message.
		/// </summary>
		/// <param name="clientId">
		/// The client id.
		/// </param>
		/// <param name="ioId">
		/// The io id.
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
		/// <returns>
		/// </returns>
		internal byte[] channelWroteMessage(
			int clientId, int ioId, EpicsType dataType, int dataCount, EpicsTransitionStatus status)
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);

			mem.Capacity = 16;

			writer.Write(NetworkByteConverter.ToByteArray(CommandID.CA_PROTO_WRITE_NOTIFY));
			writer.Write(new byte[2]);
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)dataType));
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)dataCount));
			writer.Write(NetworkByteConverter.ToByteArray((UInt32)status));
			writer.Write(NetworkByteConverter.ToByteArray((UInt32)ioId));

			var buffer = mem.GetBuffer();
			writer.Close();
			mem.Dispose();

			return buffer;
		}

		/// <summary>
		/// The error message.
		/// </summary>
		/// <param name="clientId">
		/// The client id.
		/// </param>
		/// <param name="status">
		/// The status.
		/// </param>
		/// <param name="errorMessage">
		/// The error message.
		/// </param>
		/// <param name="header">
		/// The header.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] errorMessage(int clientId, EpicsTransitionStatus status, string errorMessage, byte[] header)
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);

			mem.Capacity = 16 + 16 + errorMessage.Length + 1;

			writer.Write(NetworkByteConverter.ToByteArray(CommandID.CA_PROTO_ERROR));
			writer.Write(NetworkByteConverter.ToByteArray(errorMessage.Length + 16));
			writer.Write(new byte[4]);
			writer.Write(NetworkByteConverter.ToByteArray((UInt32)clientId));
			writer.Write(NetworkByteConverter.ToByteArray((UInt32)status));

			// emptyheader
			writer.Write(header);
			writer.Write(NetworkByteConverter.ToByteArray(errorMessage));

			var buffer = mem.GetBuffer();
			writer.Close();
			mem.Dispose();

			return buffer;
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
		/// <param name="dataType">
		/// The data type.
		/// </param>
		/// <param name="dataCount">
		/// The data count.
		/// </param>
		/// <param name="data">
		/// The data.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] monitorChangeMessage(int subscriptionId, int clientId, EpicsType dataType, int dataCount, byte[] data)
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);

			var padding = 0;

			if (data.Length % 8 == 0)
			{
				padding = 8;
			}
			else
			{
				padding = 8 - (data.Length % 8);
			}

			mem.Capacity = 16 + data.Length + padding;

			writer.Write(NetworkByteConverter.ToByteArray(CommandID.CA_PROTO_EVENT_ADD));
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)(data.Length + padding)));
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)dataType));
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)dataCount));
			writer.Write(NetworkByteConverter.ToByteArray((UInt32)clientId));
			writer.Write(NetworkByteConverter.ToByteArray((UInt32)subscriptionId));

			writer.Write(data);
			writer.Write(new byte[padding]);

			var buffer = mem.GetBuffer();
			writer.Close();
			mem.Dispose();

			return buffer;
		}

		/// <summary>
		/// The monitor close message.
		/// </summary>
		/// <param name="dataType">
		/// The data type.
		/// </param>
		/// <param name="serverId">
		/// The server id.
		/// </param>
		/// <param name="subscriptionId">
		/// The subscription id.
		/// </param>
		/// <returns>
		/// </returns>
		internal byte[] monitorCloseMessage(EpicsType dataType, int serverId, int subscriptionId)
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);

			mem.Capacity = 16;

			writer.Write(NetworkByteConverter.ToByteArray(CommandID.CA_PROTO_EVENT_CANCEL));
			writer.Write(new byte[2]);
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)dataType));
			writer.Write(new byte[2]);
			writer.Write(NetworkByteConverter.ToByteArray((UInt32)serverId));
			writer.Write(NetworkByteConverter.ToByteArray((UInt32)subscriptionId));

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
				case CommandID.CA_PROTO_VERSION:

					break;
				case CommandID.CA_PROTO_SEARCH:
					{
						var channelName = NetworkByteConverter.ToString(payload);
						if (channelName.Contains("."))
						{
							channelName = channelName.Split('.')[0];
						}

						if (this.Server.recordList.ContainsKey(channelName))
						{
							this.Server.udpConnection.Send(this.channelFoundMessage(Parameter1), (IPEndPoint)iep);
						}
					}

					break;
				case CommandID.CA_PROTO_CLIENT_NAME:
					this.Server.openConnection[iep.ToString()].Username = NetworkByteConverter.ToString(payload);
					break;
				case CommandID.CA_PROTO_HOST_NAME:
					this.Server.openConnection[iep.ToString()].Hostname = NetworkByteConverter.ToString(payload);
					break;
				case CommandID.CA_PROTO_CREATE_CHAN:
					this.Server.CreateEpicsChannel((int)Parameter1, iep, NetworkByteConverter.ToString(payload));
					break;
				case CommandID.CA_PROTO_CLEAR_CHANNEL:
					this.Server.channelList[(int)Parameter1].Dispose(true);
					break;

				case CommandID.CA_PROTO_EVENT_ADD:
					int mask = NetworkByteConverter.ToUInt16(payload, 12);
					this.Server.channelList[(int)Parameter1].addMonitor(
						(EpicsType)DataType, (int)DataCount, (int)Parameter2, (MonitorMask)mask);
					break;
				case CommandID.CA_PROTO_EVENT_CANCEL:
					this.Server.channelList[(int)Parameter1].removeMonitor((int)Parameter2);
					break;
				case CommandID.CA_PROTO_EVENTS_OFF:

					break;
				case CommandID.CA_PROTO_EVENTS_ON:

					break;

				case CommandID.CA_PROTO_READ:
				case CommandID.CA_PROTO_READ_NOTIFY:
					this.Server.channelList[(int)Parameter1].readValue((int)Parameter2, (EpicsType)DataType, (int)DataCount);
					break;
				case CommandID.CA_PROTO_WRITE:
				case CommandID.CA_PROTO_WRITE_NOTIFY:
					this.Server.channelList[(int)Parameter1].putValue((int)Parameter2, (EpicsType)DataType, (int)DataCount, payload);
					break;

				case CommandID.CA_PROTO_ECHO:
					this.Server.openConnection[iep.ToString()].Send(CEchoMessage);
					break;
			}
		}

		/// <summary>
		/// The channel found message.
		/// </summary>
		/// <param name="clientId">
		/// The client id.
		/// </param>
		/// <returns>
		/// </returns>
		private byte[] channelFoundMessage(uint clientId)
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);

			mem.Capacity = 40;

			writer.Write(cVersionMessage);
			writer.Write(NetworkByteConverter.ToByteArray(CommandID.CA_PROTO_SEARCH));
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)8));
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)this.Server.Config.TCPPort));
			writer.Write(new byte[2]);
			writer.Write(NetworkByteConverter.ToByteArray(0xFFFFFFFF));
			writer.Write(NetworkByteConverter.ToByteArray(clientId));
			writer.Write(NetworkByteConverter.ToByteArray(CAConstants.CA_MINOR_PROTOCOL_REVISION));

			var buffer = mem.GetBuffer();
			writer.Close();
			mem.Dispose();

			return buffer;
		}
	}
}