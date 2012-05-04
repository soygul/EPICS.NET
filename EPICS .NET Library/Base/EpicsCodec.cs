// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsCodec.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Net;

	using Epics.Base.Constants;
	using Epics.Base.ETypes;

	/// <summary>
	/// The epics codec.
	/// </summary>
	internal abstract class EpicsCodec
	{
		/// <summary>
		///   The c e type translator.
		/// </summary>
		public static readonly Dictionary<EpicsType, Type> CETypeTranslator;

		/// <summary>
		///   The c type translator.
		/// </summary>
		public static readonly Dictionary<Type, EpicsType> CTypeTranslator;

		/// <summary>
		///   The c tcp greet.
		/// </summary>
		public static byte[] CTCPGreet;

		/// <summary>
		///   The c echo message.
		/// </summary>
		internal static byte[] CEchoMessage = new byte[16] { 0, 23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

		/// <summary>
		///   The c version message.
		/// </summary>
		internal static byte[] cVersionMessage;

		/// <summary>
		///   Initializes static members of the <see cref = "EpicsCodec" /> class.
		/// </summary>
		static EpicsCodec()
		{
			versionMessage();
			tcpGreetPackage();

			CTypeTranslator = new Dictionary<Type, EpicsType>
				{
					{ typeof(string), EpicsType.String }, 
					{ typeof(short), EpicsType.Short }, 
					{ typeof(float), EpicsType.Float }, 
					{ typeof(Enum), EpicsType.Enum }, 
					{ typeof(sbyte), EpicsType.SByte }, 
					{ typeof(int), EpicsType.Int }, 
					{ typeof(double), EpicsType.Double }, 
					{ typeof(ExtType<string>), EpicsType.Status_String }, 
					{ typeof(ExtType<short>), EpicsType.Status_Short }, 
					{ typeof(ExtType<float>), EpicsType.Status_Float }, 
					{ typeof(ExtType<Enum>), EpicsType.Status_Enum }, 
					{ typeof(ExtType<sbyte>), EpicsType.Status_SByte }, 
					{ typeof(ExtType<int>), EpicsType.Status_Int }, 
					{ typeof(ExtType<double>), EpicsType.Status_Double }, 
					{ typeof(ExtTimeType<string>), EpicsType.Time_String }, 
					{ typeof(ExtTimeType<short>), EpicsType.Time_Short }, 
					{ typeof(ExtTimeType<float>), EpicsType.Time_Float }, 
					{ typeof(ExtTimeType<Enum>), EpicsType.Time_Enum }, 
					{ typeof(ExtTimeType<sbyte>), EpicsType.Time_SByte }, 
					{ typeof(ExtTimeType<int>), EpicsType.Time_Int }, 
					{ typeof(ExtTimeType<double>), EpicsType.Time_Double }, 
					{ typeof(ExtControl<string>), EpicsType.Control_String }, 
					{ typeof(ExtControl<short>), EpicsType.Control_Short }, 
					{ typeof(ExtControl<float>), EpicsType.Control_Float }, 
					{ typeof(ExtControl<Enum>), EpicsType.Control_Enum }, 
					{ typeof(ExtControl<sbyte>), EpicsType.Control_SByte }, 
					{ typeof(ExtControl<int>), EpicsType.Control_Int }, 
					{ typeof(ExtControl<double>), EpicsType.Control_Double }, 
					{ typeof(ExtGraphic<string>), EpicsType.Display_String }, 
					{ typeof(ExtGraphic<short>), EpicsType.Display_Short }, 
					{ typeof(ExtGraphic<float>), EpicsType.Display_Float }, 
					{ typeof(ExtGraphic<sbyte>), EpicsType.Display_SByte }, 
					{ typeof(ExtGraphic<int>), EpicsType.Display_Int }, 
					{ typeof(ExtGraphic<double>), EpicsType.Display_Double }, 
					{ typeof(ExtType<string[]>), EpicsType.Status_String }, 
					{ typeof(ExtType<short[]>), EpicsType.Status_Short }, 
					{ typeof(ExtType<float[]>), EpicsType.Status_Float }, 
					{ typeof(ExtType<Enum[]>), EpicsType.Status_Enum }, 
					{ typeof(ExtType<sbyte[]>), EpicsType.Status_SByte }, 
					{ typeof(ExtType<int[]>), EpicsType.Status_Int }, 
					{ typeof(ExtType<double[]>), EpicsType.Status_Double }, 
					{ typeof(ExtTimeType<string[]>), EpicsType.Time_String }, 
					{ typeof(ExtTimeType<short[]>), EpicsType.Time_Short }, 
					{ typeof(ExtTimeType<float[]>), EpicsType.Time_Float }, 
					{ typeof(ExtTimeType<Enum[]>), EpicsType.Time_Enum }, 
					{ typeof(ExtTimeType<sbyte[]>), EpicsType.Time_SByte }, 
					{ typeof(ExtTimeType<int[]>), EpicsType.Time_Int }, 
					{ typeof(ExtTimeType<double[]>), EpicsType.Time_Double }, 
					{ typeof(ExtControl<string[]>), EpicsType.Control_String }, 
					{ typeof(ExtControl<short[]>), EpicsType.Control_Short }, 
					{ typeof(ExtControl<float[]>), EpicsType.Control_Float }, 
					{ typeof(ExtControl<Enum[]>), EpicsType.Control_Enum }, 
					{ typeof(ExtControl<sbyte[]>), EpicsType.Control_SByte }, 
					{ typeof(ExtControl<int[]>), EpicsType.Control_Int }, 
					{ typeof(ExtControl<double[]>), EpicsType.Control_Double }, 
					{ typeof(ExtGraphic<string[]>), EpicsType.Display_String }, 
					{ typeof(ExtGraphic<short[]>), EpicsType.Display_Short }, 
					{ typeof(ExtGraphic<float[]>), EpicsType.Display_Float }, 
					{ typeof(ExtGraphic<sbyte[]>), EpicsType.Display_SByte }, 
					{ typeof(ExtGraphic<int[]>), EpicsType.Display_Int }, 
					{ typeof(ExtGraphic<double[]>), EpicsType.Display_Double }
					
					
					
					
					
					// { typeof(bool), EpicsType.Bool }, // Not implemented yet!
				};

			CETypeTranslator = new Dictionary<EpicsType, Type>();
			foreach (var pair in CTypeTranslator)
			{
				if (!CETypeTranslator.ContainsKey(pair.Value))
				{
					CETypeTranslator.Add(pair.Value, pair.Key);
				}
			}
		}

		/// <summary>
		/// The beacon message.
		/// </summary>
		/// <param name="port">
		/// The port.
		/// </param>
		/// <param name="sequenceNumber">
		/// The sequence number.
		/// </param>
		/// <param name="ip">
		/// The ip.
		/// </param>
		/// <returns>
		/// </returns>
		internal static byte[] beaconMessage(int port, int sequenceNumber, uint ip)
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);

			mem.Capacity = 16;

			writer.Write(NetworkByteConverter.ToByteArray(CommandID.CA_PROTO_RSRV_IS_UP));
			writer.Write(new byte[2]);
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)port));
			writer.Write(new byte[2]);
			writer.Write(NetworkByteConverter.ToByteArray((UInt32)sequenceNumber));
			writer.Write(NetworkByteConverter.ToByteArray(ip));

			var buffer = mem.GetBuffer();
			writer.Close();
			mem.Dispose();

			return buffer;
		}

		/// <summary>
		/// The parse byte package.
		/// </summary>
		/// <param name="dataPipe">
		/// The data pipe.
		/// </param>
		/// <param name="remoteEndPoint">
		/// The remote end point.
		/// </param>
		internal void ParseBytePackage(Pipe dataPipe, EndPoint remoteEndPoint)
		{
			this.ParseBytePackage(dataPipe, remoteEndPoint, 0);
		}

		/// <summary>
		/// The parse byte package.
		/// </summary>
		/// <param name="dataPipe">
		/// The data pipe.
		/// </param>
		/// <param name="remoteEndPoint">
		/// The remote end point.
		/// </param>
		/// <param name="maxPacketSize">
		/// The max packet size.
		/// </param>
		internal void ParseBytePackage(Pipe dataPipe, EndPoint remoteEndPoint, int maxPacketSize)
		{
			var remoteAddress = remoteEndPoint.ToString();
			uint readedBytes = 0;
			ushort cmdId, dataType;
			uint payloadSize, dataCount, param1, param2;
			byte[] payload = null;
			byte[] header = null;

			while (maxPacketSize == 0 || maxPacketSize >= (readedBytes + 16))
			{
				header = dataPipe.Read(16);

				// Pipe destroyed
				if (header.Length == 0)
				{
					return;
				}

				cmdId = NetworkByteConverter.ToUInt16(header, 0);
				payloadSize = NetworkByteConverter.ToUInt16(header, 2);
				dataType = NetworkByteConverter.ToUInt16(header, 4);
				param1 = NetworkByteConverter.ToUInt32(header, 8);
				param2 = NetworkByteConverter.ToUInt32(header, 12);

				if (payloadSize == 0xFFFF)
				{
					payloadSize = NetworkByteConverter.ToUInt32(dataPipe.Read(4));
					dataCount = NetworkByteConverter.ToUInt32(dataPipe.Read(4));

					readedBytes += payloadSize + 24;
				}
				else
				{
					dataCount = NetworkByteConverter.ToUInt16(header, 6);
					readedBytes += payloadSize + 16;
				}

				payload = dataPipe.Read((int)payloadSize);

				this.HandleMessage(
					cmdId, 
					dataType, 
					ref payloadSize, 
					ref dataCount, 
					ref param1, 
					ref param2, 
					ref header, 
					ref payload, 
					ref remoteEndPoint);
			}
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
		protected abstract void HandleMessage(
			ushort CommandId, 
			ushort DataType, 
			ref uint PayloadSize, 
			ref uint DataCount, 
			ref uint Parameter1, 
			ref uint Parameter2, 
			ref byte[] header, 
			ref byte[] payload, 
			ref EndPoint iep);

		/// <summary>
		/// Generates a EPICS-TCP-Greet Package, which is needed by establishing a new TCP-ServerConnection.
		///   It contains 3 Packages : CA_PROTO_VERSION,CA_PROTO_CLIENT_NAME and CA_PROTO_HOST_NAME
		///     
		///   *cached*
		/// </summary>
		private static void tcpGreetPackage()
		{
			var mem = new MemoryStream();
			var writer = new BinaryWriter(mem);

			var userPadding = 0;
			var hostPadding = 0;

			// calc username padding
			if (Environment.UserName.Length % 8 == 0)
			{
				userPadding = 8;
			}
			else
			{
				userPadding = 8 - (Environment.UserName.Length % 8);
			}

			// calc hostpadding
			if (Environment.MachineName.Length % 8 == 0)
			{
				hostPadding = 8;
			}
			else
			{
				hostPadding = 8 - (Environment.MachineName.Length % 8);
			}

			// calc total length
			mem.Capacity = (3 * 16) + Environment.UserName.Length + userPadding + Environment.MachineName.Length + hostPadding;

			// add the version message first
			writer.Write(cVersionMessage);

			// add the username message
			writer.Write(NetworkByteConverter.ToByteArray(CommandID.CA_PROTO_CLIENT_NAME));
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)(Environment.UserName.Length + userPadding)));
			writer.Write(new byte[12]);
			writer.Write(NetworkByteConverter.ToByteArray(Environment.UserName));
			writer.Write(new byte[userPadding]);

			// add the hostname message
			writer.Write(NetworkByteConverter.ToByteArray(CommandID.CA_PROTO_HOST_NAME));
			writer.Write(NetworkByteConverter.ToByteArray((UInt16)(Environment.MachineName.Length + hostPadding)));
			writer.Write(new byte[12]);
			writer.Write(NetworkByteConverter.ToByteArray(Environment.MachineName));
			writer.Write(new byte[hostPadding]);

			CTCPGreet = mem.GetBuffer();
			writer.Close();
			mem.Dispose();
		}

		/// <summary>
		/// Generates a 16 byte MessageHeader containing the standard VersionMessage with
		///   Priority 1.
		///   It's only used in combination with other messages for this reason it's private.
		///   *cached*
		/// </summary>
		private static void versionMessage()
		{
			cVersionMessage = new byte[16] { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 };
			Buffer.BlockCopy(NetworkByteConverter.ToByteArray(CAConstants.CA_MINOR_PROTOCOL_REVISION), 0, cVersionMessage, 6, 2);
		}
	}
}