// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetworkByteConverter.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base
{
	#region Using Directives

	using System;
	using System.IO;
	using System.Text;

	using Epics.Base.Constants;
	using Epics.Base.ETypes;
	using Epics.Server;

	#endregion

	/// <summary>
	/// The network byte converter.
	/// </summary>
	/// <remarks>
	/// Helper methods to convert network byte streams to C# data.
	///   These methods are important because network byte streams are always big-endian,
	///   whereas C# is little-endian on most systems.
	/// </remarks>
	internal class NetworkByteConverter
	{
		/// <summary>
		///   The ascii.
		/// </summary>
		private static readonly Encoding ascii = new UTF8Encoding();

		/// <summary>
		///   The mem.
		/// </summary>
		private static readonly MemoryStream mem = new MemoryStream();

		/// <summary>
		///   The writer.
		/// </summary>
		private static readonly BinaryWriter writer = new BinaryWriter(mem);

		/// <summary>
		///   The is little endian architecture.
		/// </summary>
		private static bool IsLittleEndianArchitecture = BitConverter.IsLittleEndian;

		/// <summary>
		///   The timestamp base.
		/// </summary>
		private static DateTime TimestampBase = new DateTime(1990, 1, 1, 0, 0, 0);

		/// <summary>
		/// Converts a byte array to a Byte (1 Byte)
		/// </summary>
		/// <param name="bytes">
		/// An array of bytes.
		/// </param>
		/// <param name="startPos">
		/// The starting position within value.
		/// </param>
		/// <returns>
		/// The converted Byte
		/// </returns>
		public static byte ToByte(byte[] bytes, int startPos)
		{
			return bytes[startPos];
		}

		/// <summary>
		/// Converts a string to a byte array
		/// </summary>
		/// <param name="str">
		/// The string to convert to a byte array
		/// </param>
		/// <returns>
		/// The converted string as byte array
		/// </returns>
		public static byte[] ToByteArray(string str)
		{
			return ascii.GetBytes(str);
		}

		/// <summary>
		/// The to byte array.
		/// </summary>
		/// <param name="str">
		/// The str.
		/// </param>
		/// <param name="forceLength">
		/// The force length.
		/// </param>
		/// <returns>
		/// </returns>
		public static byte[] ToByteArray(string str, bool forceLength)
		{
			var result = new byte[40];
			var src = ascii.GetBytes(str);
			Buffer.BlockCopy(src, 0, result, 0, src.Length);

			return result;
		}

		/// <summary>
		/// Converts a Byte to network byte order
		/// </summary>
		/// <param name="var">
		/// The Byte to convert to a byte array
		/// </param>
		/// <returns>
		/// The converted byte as byte array
		/// </returns>
		public static byte[] ToByteArray(byte var)
		{
			byte[] ret = { var };
			return ret;
		}

		/// <summary>
		/// Converts a SByte to network byte order
		/// </summary>
		/// <param name="var">
		/// The SByte to convert to a byte array
		/// </param>
		/// <returns>
		/// The converted Sbyte as byte array
		/// </returns>
		public static byte[] ToByteArray(sbyte var)
		{
			return new[] { (byte)var };
		}

		/// <summary>
		/// Converts a Int16 to network byte order
		/// </summary>
		/// <param name="var">
		/// The Int16 to convert to a byte array
		/// </param>
		/// <returns>
		/// The converted Int16 as byte array
		/// </returns>
		public static byte[] ToByteArray(short var)
		{
			var ret = BitConverter.GetBytes(var);
			Array.Reverse(ret);
			return ret;
		}

		/// <summary>
		/// Converts a Int32 to network byte order
		/// </summary>
		/// <param name="var">
		/// The Int32 to convert to a byte array
		/// </param>
		/// <returns>
		/// The converted Int32 as byte array
		/// </returns>
		public static byte[] ToByteArray(int var)
		{
			var ret = BitConverter.GetBytes(var);
			Array.Reverse(ret);
			return ret;
		}

		/// <summary>
		/// Converts a Boolean to network byte order
		/// </summary>
		/// <param name="var">
		/// The Boolean to convert to a byte array
		/// </param>
		/// <returns>
		/// The converted Int32 as byte array
		/// </returns>
		public static byte[] ToByteArray(bool var)
		{
			var ret = BitConverter.GetBytes(var);
			Array.Reverse(ret);
			return ret;
		}

		/// <summary>
		/// Converts a UInt16 to network byte order
		/// </summary>
		/// <param name="var">
		/// The UInt16 to convert to a byte array
		/// </param>
		/// <returns>
		/// The converted UInt16 as byte array
		/// </returns>
		public static byte[] ToByteArray(ushort var)
		{
			var ret = BitConverter.GetBytes(var);
			Array.Reverse(ret);
			return ret;
		}

		/// <summary>
		/// Converts a UInt32 to network byte order
		/// </summary>
		/// <param name="var">
		/// The UInt32 to convert to a byte array
		/// </param>
		/// <returns>
		/// The converted UInt32 as byte array
		/// </returns>
		public static byte[] ToByteArray(uint var)
		{
			var ret = BitConverter.GetBytes(var);
			Array.Reverse(ret);
			return ret;
		}

		/// <summary>
		/// Converts a Double to network byte order
		/// </summary>
		/// <param name="var">
		/// The double to convert to a byte array
		/// </param>
		/// <returns>
		/// The converted Double as byte array
		/// </returns>
		public static byte[] ToByteArray(double var)
		{
			var ret = BitConverter.GetBytes(var);
			Array.Reverse(ret);
			return ret;
		}

		/// <summary>
		/// Converts a float to network byte order
		/// </summary>
		/// <param name="var">
		/// The float to convert to a byte array
		/// </param>
		/// <returns>
		/// The converted float as byte array
		/// </returns>
		public static byte[] ToByteArray(float var)
		{
			var ret = BitConverter.GetBytes(var);
			Array.Reverse(ret);
			return ret;
		}

		/// <summary>
		/// The to byte array.
		/// </summary>
		/// <param name="time">
		/// The time.
		/// </param>
		/// <returns>
		/// </returns>
		public static byte[] ToByteArray(DateTime time)
		{
			if (time == null)
			{
				return new byte[0];
			}

			var dateTimeBytes = new byte[8];
			var Diff = time.Ticks - TimestampBase.ToUniversalTime().Ticks;

			var secs = (UInt32)Math.Round((double)(Diff / 10000000));
			var nanosecs = (UInt32)(Diff - (secs * 10000000)) * 100;

			Buffer.BlockCopy(ToByteArray(secs), 0, dateTimeBytes, 0, 4);
			Buffer.BlockCopy(ToByteArray(nanosecs), 0, dateTimeBytes, 4, 4);

			return dateTimeBytes;
		}

		/// <summary>
		/// Converts a byte array to a Double (8 Bytes)
		/// </summary>
		/// <param name="bytes">
		/// An array of bytes.
		/// </param>
		/// <param name="startPos">
		/// The starting position within value.
		/// </param>
		/// <returns>
		/// The converted Double
		/// </returns>
		public static double ToDouble(byte[] bytes, int startPos)
		{
			var doubleBytes = new byte[8];
			Buffer.BlockCopy(bytes, startPos, doubleBytes, 0, 8);

			Array.Reverse(doubleBytes);

			return BitConverter.ToDouble(doubleBytes, 0);
		}

		/// <summary>
		/// The to double.
		/// </summary>
		/// <param name="bytes">
		/// The bytes.
		/// </param>
		/// <returns>
		/// The to double.
		/// </returns>
		public static double ToDouble(byte[] bytes)
		{
			return ToDouble(bytes, 0);
		}

		/// <summary>
		/// Converts a byte array to a Float (4 Bytes)
		/// </summary>
		/// <param name="bytes">
		/// An array of bytes.
		/// </param>
		/// <param name="startPos">
		/// The starting position within value.
		/// </param>
		/// <returns>
		/// The converted Float
		/// </returns>
		public static float ToFloat(byte[] bytes, int startPos)
		{
			var floatBytes = new byte[4];
			Buffer.BlockCopy(bytes, startPos, floatBytes, 0, 4);

			Array.Reverse(floatBytes);

			return BitConverter.ToSingle(floatBytes, 0);
		}

		/// <summary>
		/// The to float.
		/// </summary>
		/// <param name="bytes">
		/// The bytes.
		/// </param>
		/// <returns>
		/// The to float.
		/// </returns>
		public static float ToFloat(byte[] bytes)
		{
			return ToFloat(bytes, 0);
		}

		/// <summary>
		/// Converts a byte array to an Int16
		/// </summary>
		/// <param name="bytes">
		/// An array of bytes.
		/// </param>
		/// <param name="startPos">
		/// The starting position within value.
		/// </param>
		/// <returns>
		/// The converted Int16
		/// </returns>
		public static short ToInt16(byte[] bytes, int startPos)
		{
			var ushortBytes = new byte[2];
			Buffer.BlockCopy(bytes, startPos, ushortBytes, 0, 2);

			Array.Reverse(ushortBytes);

			return BitConverter.ToInt16(ushortBytes, 0);
		}

		/// <summary>
		/// The to int 16.
		/// </summary>
		/// <param name="bytes">
		/// The bytes.
		/// </param>
		/// <returns>
		/// The to int 16.
		/// </returns>
		public static short ToInt16(byte[] bytes)
		{
			return ToInt16(bytes, 0);
		}

		/// <summary>
		/// Converts a byte array to an Int32
		/// </summary>
		/// <param name="bytes">
		/// An array of bytes.
		/// </param>
		/// <param name="startPos">
		/// The starting position within value.
		/// </param>
		/// <returns>
		/// The converted Int32
		/// </returns>
		public static int ToInt32(byte[] bytes, int startPos)
		{
			var uintBytes = new byte[4];
			Buffer.BlockCopy(bytes, startPos, uintBytes, 0, 4);

			Array.Reverse(uintBytes);

			return BitConverter.ToInt32(uintBytes, 0);
		}

		/// <summary>
		/// The to int 32.
		/// </summary>
		/// <param name="bytes">
		/// The bytes.
		/// </param>
		/// <returns>
		/// The to int 32.
		/// </returns>
		public static int ToInt32(byte[] bytes)
		{
			return ToInt32(bytes, 0);
		}

		/// <summary>
		/// The to int 64.
		/// </summary>
		/// <param name="bytes">
		/// The bytes.
		/// </param>
		/// <param name="startPos">
		/// The start pos.
		/// </param>
		/// <returns>
		/// The to int 64.
		/// </returns>
		public static long ToInt64(byte[] bytes, int startPos)
		{
			var intBytes = new byte[8];
			Buffer.BlockCopy(bytes, startPos, intBytes, 0, 8);

			Array.Reverse(intBytes);

			return BitConverter.ToInt64(intBytes, 0);
		}

		/// <summary>
		/// The to int 64.
		/// </summary>
		/// <param name="bytes">
		/// The bytes.
		/// </param>
		/// <returns>
		/// The to int 64.
		/// </returns>
		public static long ToInt64(byte[] bytes)
		{
			return ToInt64(bytes, 0);
		}

		/// <summary>
		/// Converts a byte array to a signed byte (1 Byte)
		/// </summary>
		/// <param name="bytes">
		/// An array of bytes.
		/// </param>
		/// <param name="startPos">
		/// The starting position within value.
		/// </param>
		/// <returns>
		/// The converted sbyte
		/// </returns>
		public static sbyte ToSByte(byte[] bytes, int startPos)
		{
			// re-interpret cast (Convert.ToSByte throws exceptions if value is to big)
			return (SByte)bytes[startPos];
		}

		/// <summary>
		/// Converts a byte array to a string
		/// </summary>
		/// <param name="bytes">
		/// An array of bytes.
		/// </param>
		/// <param name="startPos">
		/// The starting position within value.
		/// </param>
		/// <param name="len">
		/// The number of bytes to decode.
		/// </param>
		/// <returns>
		/// The converted string
		/// </returns>
		public static string ToString(byte[] bytes, int startPos, int len)
		{
			var chars = new char[len];

			ascii.GetDecoder().GetChars(bytes, startPos, len, chars, 0);
			var ret = new string(chars);
			var indexOf = ret.IndexOf('\0');
			if (indexOf != -1)
			{
				ret = ret.Substring(0, indexOf);
			}

			return ret;
		}

		/// <summary>
		/// Converts a byte array (40 bytes) to a string (max. 39 chars, because of '/0')
		/// </summary>
		/// <param name="bytes">
		/// An array of bytes.
		/// </param>
		/// <param name="startPos">
		/// The starting position within value.
		/// </param>
		/// <returns>
		/// The converted string
		/// </returns>
		public static string ToString(byte[] bytes, int startPos)
		{
			if ((bytes.Length - startPos) < 40)
			{
				return ToString(bytes, startPos, bytes.Length - startPos);
			}
			else
			{
				return ToString(bytes, startPos, 40);
			}
		}

		/// <summary>
		/// Convert a byte array to a string
		/// </summary>
		/// <param name="data">
		/// byte array
		/// </param>
		/// <returns>
		/// the converted string
		/// </returns>
		public static string ToString(byte[] data)
		{
			return ToString(data, 0, data.Length);
		}

		/// <summary>
		/// Converts a byte array to a UInt16
		/// </summary>
		/// <param name="bytes">
		/// An array of bytes.
		/// </param>
		/// <param name="startPos">
		/// The starting position within value.
		/// </param>
		/// <returns>
		/// The converted UInt16
		/// </returns>
		public static ushort ToUInt16(byte[] bytes, int startPos)
		{
			var ushortBytes = new byte[2];
			Buffer.BlockCopy(bytes, startPos, ushortBytes, 0, 2);

			Array.Reverse(ushortBytes);

			return BitConverter.ToUInt16(ushortBytes, 0);
		}

		/// <summary>
		/// The to u int 16.
		/// </summary>
		/// <param name="bytes">
		/// The bytes.
		/// </param>
		/// <returns>
		/// The to u int 16.
		/// </returns>
		public static ushort ToUInt16(byte[] bytes)
		{
			return ToUInt16(bytes, 0);
		}

		/// <summary>
		/// Converts a byte array to a UInt32
		/// </summary>
		/// <param name="bytes">
		/// An array of bytes.
		/// </param>
		/// <param name="startPos">
		/// The starting position within value.
		/// </param>
		/// <returns>
		/// The converted UInt32
		/// </returns>
		public static uint ToUInt32(byte[] bytes, int startPos)
		{
			var uintBytes = new byte[4];
			Buffer.BlockCopy(bytes, startPos, uintBytes, 0, 4);

			Array.Reverse(uintBytes);

			return BitConverter.ToUInt32(uintBytes, 0);
		}

		/// <summary>
		/// The to u int 32.
		/// </summary>
		/// <param name="bytes">
		/// The bytes.
		/// </param>
		/// <returns>
		/// The to u int 32.
		/// </returns>
		public static uint ToUInt32(byte[] bytes)
		{
			return ToUInt32(bytes, 0);
		}

		/// <summary>
		/// The byte to object.
		/// </summary>
		/// <param name="payload">
		/// The payload.
		/// </param>
		/// <param name="epicsType">
		/// The epics type.
		/// </param>
		/// <returns>
		/// The byte to object.
		/// </returns>
		internal static object byteToObject(byte[] payload, EpicsType epicsType)
		{
			string egu;
			int offset;

			switch (epicsType)
			{
					// -> basic types
				case EpicsType.String:
					return ToString(payload);
				case EpicsType.Short:
					return ToUInt16(payload);
				case EpicsType.Float:
					return ToFloat(payload);
				case EpicsType.Enum:
					return null;
				case EpicsType.SByte:
					return ToSByte(payload, 0);
				case EpicsType.Int:
					return ToInt32(payload);
				case EpicsType.Double:
					return ToDouble(payload);

					// -> extended Statustypes
				case EpicsType.Status_Short:
					return new ExtType<short> { Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = ToInt16(payload, 4) };
				case EpicsType.Status_Float:
					return new ExtType<float> { Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = ToFloat(payload, 4) };
				case EpicsType.Status_Double:
					return new ExtType<double> { Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = ToDouble(payload, 4) };
				case EpicsType.Status_String:
					return new ExtType<string> { Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = ToString(payload, 4) };
					break;
				case EpicsType.Status_Int:
					return new ExtType<int> { Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = ToInt32(payload, 4) };
					break;
				case EpicsType.Status_Enum:
					return new ExtType<string> { Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = ToString(payload, 4) };
					break;

					// -> extended Timetypes
				case EpicsType.Time_Short:

					// For some reason, even if you ask for a short you receive back an int!
					// so we cut of the first to bytes of the value part of the payload
					if (payload.Length == 16)
					{
						return new ExtTimeType<short>(ToInt32(payload, 4), ToInt32(payload, 8))
							{
          Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = ToInt16(payload, 14) 
       };
					}
					else
					{
						return new ExtTimeType<short>(0, 0)
							{
          Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = ToInt16(payload, 6) 
       };
					}

				case EpicsType.Time_Float:
					if (payload.Length == 18)
					{
						return new ExtTimeType<float>(ToInt32(payload, 4), ToInt32(payload, 8))
							{
          Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = ToFloat(payload, 12) 
       };
					}
					else
					{
						return new ExtTimeType<float>(0, 0)
							{
          Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = ToFloat(payload, 4) 
       };
					}

				case EpicsType.Time_Double:
					if (payload.Length == 24)
					{
						return new ExtTimeType<double>(ToInt32(payload, 4), ToInt32(payload, 8))
							{
          Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = ToDouble(payload, 16) 
       };
					}
					else
					{
						// check if there are 4 bytes of unknown relation in this constalation
						return new ExtTimeType<double>(0, 0)
							{
          Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = ToDouble(payload, 8) 
       };
					}

				case EpicsType.Time_String:
					if (payload.Length == 56)
					{
						return new ExtTimeType<string>(ToInt32(payload, 4), ToInt32(payload, 8))
							{
          Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = ToString(payload, 12) 
       };
					}
					else
					{
						return new ExtTimeType<string>(0, 0)
							{
          Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = ToString(payload, 4) 
       };
					}

				case EpicsType.Time_Int:
					if (payload.Length == 16)
					{
						return new ExtTimeType<int>(ToInt32(payload, 4), ToInt32(payload, 8))
							{
          Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = ToInt32(payload, 12) 
       };
					}
					else
					{
						return new ExtTimeType<int>(0, 0)
							{
          Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = ToInt32(payload, 4) 
       };
					}

				case EpicsType.Time_Enum:
					return new ExtTimeType<string>(ToInt32(payload, 4), ToInt32(payload, 8))
						{
         Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = ToString(payload, 12) 
      };

					// -> extended Graphictypes
				case EpicsType.Display_Short:
					egu = ToString(payload, 4);
					offset = 1 + egu.Length;

					return new ExtGraphic<short> {
							Status = (Status)ToInt16(payload, 0), 
							Severity = (Severity)ToInt16(payload, 2), 
							EGU = egu, 
							HighDisplayLimit = ToInt16(payload, 4 + offset), 
							LowDisplayLimit = ToInt16(payload, 6 + offset), 
							HighAlertLimit = ToInt16(payload, 8 + offset), 
							HighWarnLimit = ToInt16(payload, 10 + offset), 
							LowWarnLimit = ToInt16(payload, 12 + offset), 
							LowAlertLimit = ToInt16(payload, 14 + offset), 
							Value = ToInt16(payload, 16 + offset)
						};
				case EpicsType.Display_Float:
					egu = ToString(payload, 8);
					offset = 1 + egu.Length;

					return new ExtGraphic<float> {
							Status = (Status)ToInt16(payload, 0), 
							Severity = (Severity)ToInt16(payload, 2), 
							Precision = ToInt16(payload, 4), 
							EGU = egu, 
							HighDisplayLimit = ToFloat(payload, 8 + offset), 
							LowDisplayLimit = ToFloat(payload, 12 + offset), 
							HighAlertLimit = ToFloat(payload, 16 + offset), 
							HighWarnLimit = ToFloat(payload, 20 + offset), 
							LowWarnLimit = ToFloat(payload, 24 + offset), 
							LowAlertLimit = ToFloat(payload, 28 + offset), 
							Value = ToFloat(payload, 32 + offset)
						};
				case EpicsType.Display_Double:
					egu = ToString(payload, 8);
					offset = 1 + egu.Length;

					return new ExtGraphic<double> {
							Status = (Status)ToInt16(payload, 0), 
							Severity = (Severity)ToInt16(payload, 2), 
							Precision = ToInt16(payload, 4), 
							EGU = egu, 
							HighDisplayLimit = ToDouble(payload, 8 + offset), 
							LowDisplayLimit = ToDouble(payload, 16 + offset), 
							HighAlertLimit = ToDouble(payload, 24 + offset), 
							HighWarnLimit = ToDouble(payload, 32 + offset), 
							LowWarnLimit = ToDouble(payload, 40 + offset), 
							LowAlertLimit = ToDouble(payload, 48 + offset), 
							Value = ToDouble(payload, 56 + offset)
						};

				case EpicsType.Display_String:
					return new ExtGraphic<string> { Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = ToString(payload, 4) };
				case EpicsType.Display_Int:
					egu = ToString(payload, 4);
					offset = 1 + egu.Length;

					return new ExtGraphic<int> {
							Status = (Status)ToInt16(payload, 0), 
							Severity = (Severity)ToInt16(payload, 2), 
							EGU = egu, 
							HighDisplayLimit = ToInt32(payload, 4 + offset), 
							LowDisplayLimit = ToInt32(payload, 8 + offset), 
							HighAlertLimit = ToInt32(payload, 12 + offset), 
							HighWarnLimit = ToInt32(payload, 16 + offset), 
							LowWarnLimit = ToInt32(payload, 20 + offset), 
							LowAlertLimit = ToInt32(payload, 24 + offset), 
							Value = ToInt32(payload, 28 + offset)
						};

					// -> extended Controltypes
				case EpicsType.Control_Short:
					egu = ToString(payload, 4);
					offset = 1 + egu.Length;

					return new ExtControl<short> {
							Status = (Status)ToInt16(payload, 0), 
							Severity = (Severity)ToInt16(payload, 2), 
							EGU = egu, 
							HighDisplayLimit = ToInt16(payload, 4 + offset), 
							LowDisplayLimit = ToInt16(payload, 6 + offset), 
							HighAlertLimit = ToInt16(payload, 8 + offset), 
							HighWarnLimit = ToInt16(payload, 10 + offset), 
							LowWarnLimit = ToInt16(payload, 12 + offset), 
							LowAlertLimit = ToInt16(payload, 14 + offset), 
							HighControlLimit = ToInt16(payload, 16 + offset), 
							LowControlLimit = ToInt16(payload, 18 + offset), 
							Value = ToInt16(payload, 20 + offset)
						};
				case EpicsType.Control_Float:
					egu = ToString(payload, 8);
					offset = 1 + egu.Length;

					return new ExtControl<float> {
							Status = (Status)ToInt16(payload, 0), 
							Severity = (Severity)ToInt16(payload, 2), 
							Precision = ToInt16(payload, 4), 
							EGU = egu, 
							HighDisplayLimit = ToFloat(payload, 8 + offset), 
							LowDisplayLimit = ToFloat(payload, 12 + offset), 
							HighAlertLimit = ToFloat(payload, 16 + offset), 
							HighWarnLimit = ToFloat(payload, 20 + offset), 
							LowWarnLimit = ToFloat(payload, 24 + offset), 
							LowAlertLimit = ToFloat(payload, 28 + offset), 
							HighControlLimit = ToFloat(payload, 32 + offset), 
							LowControlLimit = ToFloat(payload, 36 + offset), 
							Value = ToFloat(payload, 40 + offset)
						};
				case EpicsType.Control_Double:
					egu = ToString(payload, 8);
					offset = 1 + egu.Length;

					return new ExtControl<double> {
							Status = (Status)ToInt16(payload, 0), 
							Severity = (Severity)ToInt16(payload, 2), 
							Precision = ToInt16(payload, 4), 
							EGU = egu, 
							HighDisplayLimit = ToDouble(payload, 8 + offset), 
							LowDisplayLimit = ToDouble(payload, 16 + offset), 
							HighAlertLimit = ToDouble(payload, 24 + offset), 
							HighWarnLimit = ToDouble(payload, 32 + offset), 
							LowWarnLimit = ToDouble(payload, 40 + offset), 
							LowAlertLimit = ToDouble(payload, 48 + offset), 
							HighControlLimit = ToDouble(payload, 56 + offset), 
							LowControlLimit = ToDouble(payload, 64 + offset), 
							Value = ToDouble(payload, 72 + offset)
						};

				case EpicsType.Control_String:
					return new ExtControl<string> { Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = ToString(payload, 4) };
				case EpicsType.Control_Int:
					egu = ToString(payload, 4);
					offset = 1 + egu.Length;

					return new ExtControl<int> {
							Status = (Status)ToInt16(payload, 0), 
							Severity = (Severity)ToInt16(payload, 2), 
							EGU = egu, 
							HighDisplayLimit = ToInt32(payload, 4 + offset), 
							LowDisplayLimit = ToInt32(payload, 8 + offset), 
							HighAlertLimit = ToInt32(payload, 12 + offset), 
							HighWarnLimit = ToInt32(payload, 16 + offset), 
							LowWarnLimit = ToInt32(payload, 20 + offset), 
							LowAlertLimit = ToInt32(payload, 24 + offset), 
							HighControlLimit = ToInt32(payload, 28 + offset), 
							LowControlLimit = ToInt32(payload, 32 + offset), 
							Value = ToInt32(payload, 36 + offset)
						};
				case EpicsType.Control_Enum:
					var listSize = ToInt16(payload, 4);
					var list = new string[listSize];
					short counter = 0;
					while (counter < listSize)
					{
						list[counter] = ToString(payload, 6 + (26 * counter), 26);
						counter++;
					}

					return new ExtEnumType
						{
							Status = (Status)ToInt16(payload, 0), 
							Severity = (Severity)ToInt16(payload, 2), 
							EnumArray = list, 
							Value = ToInt16(payload, 6 + (26 * counter))
						};

				default:
					return null;
			}
		}

		/// <summary>
		/// The byte to object.
		/// </summary>
		/// <param name="payload">
		/// The payload.
		/// </param>
		/// <param name="epicsType">
		/// The epics type.
		/// </param>
		/// <param name="datacount">
		/// The datacount.
		/// </param>
		/// <returns>
		/// The byte to object.
		/// </returns>
		internal static object byteToObject(byte[] payload, EpicsType epicsType, int datacount)
		{
			string egu;
			var offset = 0;
			var i = 0;

			switch (epicsType)
			{
					// -> basic types
				case EpicsType.String:
					{
						var arr = new string[datacount];
						for (; i < datacount; i++)
						{
							var tmp = ToString(payload, i * 40);
							arr[i] = tmp;

							/*offset += tmp.Length + 1;*/
						}

						return arr;
					}

				case EpicsType.Short:
					{
						var arr = new ushort[datacount];
						for (; i < datacount; i++)
						{
							arr[i] = ToUInt16(payload, i * 2);
						}

						return arr;
					}

				case EpicsType.Float:
					{
						var arr = new float[datacount];
						for (; i < datacount; i++)
						{
							arr[i] = ToFloat(payload, i * 4);
						}

						return arr;
					}

				case EpicsType.Enum:
					return null;
					break;
				case EpicsType.SByte:
					{
						var arr = new sbyte[datacount];
						for (; i < datacount; i++)
						{
							arr[i] = ToSByte(payload, i);
						}

						return arr;
					}

				case EpicsType.Int:
					{
						var arr = new int[datacount];
						for (; i < datacount; i++)
						{
							arr[i] = ToInt32(payload, i * 4);
						}

						return arr;
					}

				case EpicsType.Double:
					{
						var arr = new double[datacount];
						for (; i < datacount; i++)
						{
							arr[i] = ToDouble(payload, i * 8);
						}

						return arr;
					}

					// -> extended Statustypes
					// 2 bytes status, 2 bytes severity, x bytes value, optional x bytes padding
				case EpicsType.Status_Short:
					{
						var tResult = new short[datacount];
						for (; i < datacount; i++)
						{
							tResult[i] = (short)ToUInt16(payload, 4 + (i * 2));
						}

						return new ExtType<short[]> { Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = tResult };
					}

				case EpicsType.Status_Float:
					{
						var tResult = new float[datacount];
						for (; i < datacount; i++)
						{
							tResult[i] = ToFloat(payload, 4 + (i * 4));
						}

						return new ExtType<float[]> { Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = tResult };
					}

				case EpicsType.Status_Double:
					{
						var tResult = new double[datacount];
						for (; i < datacount; i++)
						{
							tResult[i] = ToDouble(payload, 4 + (i * 8));
						}

						return new ExtType<double[]> { Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = tResult };
					}

				case EpicsType.Status_String:
					{
						var tResult = new string[datacount];
						for (; i < datacount; i++)
						{
							tResult[i] = ToString(payload, 4 + (i * 40));
						}

						return new ExtType<string[]> { Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = tResult };
					}

				case EpicsType.Status_Int:
					{
						var tResult = new int[datacount];
						for (; i < datacount; i++)
						{
							tResult[i] = ToInt32(payload, 4 + (i * 4));
						}

						return new ExtType<int[]> { Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = tResult };
					}

				case EpicsType.Status_Enum:
					return new ExtType<string> { Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = ToString(payload, 4) };

					// -> extended Timetypes
					// 2 bytes status, 2 bytes severity, optional 8 bytes timestamp, 2 bytes gap
					// x bytes value, optional padding
				case EpicsType.Time_Short:
					{
						var expectedLength = 14 + (datacount * 2);
						if ((expectedLength % 8) != 0)
						{
							expectedLength += 8 - expectedLength % 8;
						}

						if (payload.Length == expectedLength)
						{
							var tResult = new short[datacount];
							for (; i < datacount; i++)
							{
								tResult[i] = (short)ToUInt16(payload, 14 + (i * 2));
							}

							return new ExtTimeType<short[]>(ToInt32(payload, 4), ToInt32(payload, 8))
								{
           Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = tResult 
        };
						}
						else
						{
							var tResult = new short[datacount];
							for (; i < datacount; i++)
							{
								tResult[i] = (short)ToUInt16(payload, 6 + (i * 2));
							}

							return new ExtTimeType<short[]>(0, 0)
								{
           Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = tResult 
        };
						}
					}

				case EpicsType.Time_Float:
					{
						var expectedLength = 12 + (datacount * 4);
						if ((expectedLength % 8) != 0)
						{
							expectedLength += 8 - expectedLength % 8;
						}

						if (payload.Length == expectedLength)
						{
							var tResult = new float[datacount];
							for (; i < datacount; i++)
							{
								tResult[i] = ToFloat(payload, 12 + (i * 4));
							}

							return new ExtTimeType<float[]>(ToInt32(payload, 4), ToInt32(payload, 8))
								{
           Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = tResult 
        };
						}
						else
						{
							var tResult = new float[datacount];
							for (; i < datacount; i++)
							{
								tResult[i] = ToFloat(payload, 6 + (i * 4));
							}

							return new ExtTimeType<float[]>(0, 0)
								{
           Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = tResult 
        };
						}
					}

				case EpicsType.Time_Double:
					if (payload.Length == (16 + (datacount * 8)))
					{
						var tResult = new double[datacount];
						for (; i < datacount; i++)
						{
							tResult[i] = ToDouble(payload, 16 + (i * 8));
						}

						return new ExtTimeType<double[]>(ToInt32(payload, 4), ToInt32(payload, 8))
							{
          Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = tResult 
       };
					}
					else
					{
						var tResult = new double[datacount];
						for (; i < datacount; i++)
						{
							tResult[i] = ToDouble(payload, 6 + (i * 8));
						}

						return new ExtTimeType<double[]>(0, 0)
							{
          Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = tResult 
       };
					}

				case EpicsType.Time_String:
					if (payload.Length == (16 + (datacount * 40)))
					{
						var tResult = new string[datacount];
						for (; i < datacount; i++)
						{
							tResult[i] = ToString(payload, 12 + (i * 40));
						}

						return new ExtTimeType<string[]>(ToInt32(payload, 4), ToInt32(payload, 8))
							{
          Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = tResult 
       };
					}
					else
					{
						var tResult = new string[datacount];
						for (; i < datacount; i++)
						{
							tResult[i] = ToString(payload, 6 + (i * 40));
						}

						return new ExtTimeType<string[]>(0, 0)
							{
          Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = tResult 
       };
					}

				case EpicsType.Time_Int:
					{
						var expectedLength = 12 + (datacount * 4);
						if ((expectedLength % 8) != 0)
						{
							expectedLength += 8 - expectedLength % 8;
						}

						if (payload.Length == expectedLength)
						{
							var tResult = new int[datacount];
							for (; i < datacount; i++)
							{
								tResult[i] = ToInt32(payload, 12 + (i * 4));
							}

							return new ExtTimeType<int[]>(ToInt32(payload, 4), ToInt32(payload, 8))
								{
           Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = tResult 
        };
						}
						else
						{
							var tResult = new int[datacount];
							for (; i < datacount; i++)
							{
								tResult[i] = ToInt32(payload, 6 + (i * 4));
							}

							return new ExtTimeType<int[]>(0, 0)
								{
           Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = tResult 
        };
						}
					}

				case EpicsType.Time_Enum:
					return new ExtTimeType<string>(ToInt32(payload, 4), ToInt32(payload, 8))
						{
         Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = ToString(payload, 12) 
      };

					// -> extended-Graphictypes
				case EpicsType.Display_Short:
					{
						egu = ToString(payload, 4);
						offset = 1 + egu.Length;

						var tResult = new short[datacount];
						for (; i < datacount; i++)
						{
							tResult[i] = ToInt16(payload, (offset + 16) + (i * 2));
						}

						return new ExtGraphic<short[]> {
								Status = (Status)ToInt16(payload, 0), 
								Severity = (Severity)ToInt16(payload, 2), 
								EGU = egu, 
								HighDisplayLimit = ToInt16(payload, 4 + offset), 
								LowDisplayLimit = ToInt16(payload, 6 + offset), 
								HighAlertLimit = ToInt16(payload, 8 + offset), 
								HighWarnLimit = ToInt16(payload, 10 + offset), 
								LowWarnLimit = ToInt16(payload, 12 + offset), 
								LowAlertLimit = ToInt16(payload, 14 + offset), 
								Value = tResult
							};
					}

				case EpicsType.Display_Float:
					{
						egu = ToString(payload, 8);
						offset = 1 + egu.Length;

						var tResult = new float[datacount];
						for (; i < datacount; i++)
						{
							tResult[i] = ToFloat(payload, (offset + 32) + (i * 4));
						}

						return new ExtGraphic<float[]> {
								Status = (Status)ToInt16(payload, 0), 
								Severity = (Severity)ToInt16(payload, 2), 
								Precision = ToInt16(payload, 4), 
								EGU = egu, 
								HighDisplayLimit = ToFloat(payload, 8 + offset), 
								LowDisplayLimit = ToFloat(payload, 12 + offset), 
								HighAlertLimit = ToFloat(payload, 16 + offset), 
								HighWarnLimit = ToFloat(payload, 20 + offset), 
								LowWarnLimit = ToFloat(payload, 24 + offset), 
								LowAlertLimit = ToFloat(payload, 28 + offset), 
								Value = tResult
							};
					}

				case EpicsType.Display_Double:
					{
						egu = ToString(payload, 8);
						offset = 1 + egu.Length;

						var tResult = new double[datacount];
						for (; i < datacount; i++)
						{
							tResult[i] = ToDouble(payload, (offset + 56) + (i * 8));
						}

						return new ExtGraphic<double[]> {
								Status = (Status)ToInt16(payload, 0), 
								Severity = (Severity)ToInt16(payload, 2), 
								Precision = ToInt16(payload, 4), 
								EGU = egu, 
								HighDisplayLimit = ToDouble(payload, 8 + offset), 
								LowDisplayLimit = ToDouble(payload, 16 + offset), 
								HighAlertLimit = ToDouble(payload, 24 + offset), 
								HighWarnLimit = ToDouble(payload, 32 + offset), 
								LowWarnLimit = ToDouble(payload, 40 + offset), 
								LowAlertLimit = ToDouble(payload, 48 + offset), 
								Value = tResult
							};
					}

				case EpicsType.Display_String:
					{
						var tResult = new string[datacount];
						for (; i < datacount; i++)
						{
							tResult[i] = ToString(payload, 4 + (i * 40));
						}

						return new ExtType<string[]> { Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = tResult };
					}

				case EpicsType.Display_Int:
					{
						egu = ToString(payload, 4);
						offset = 1 + egu.Length;

						var tResult = new int[datacount];
						for (; i < datacount; i++)
						{
							tResult[i] = ToInt32(payload, (offset + 28) + (i * 4));
						}

						return new ExtGraphic<int[]> {
								Status = (Status)ToInt16(payload, 0), 
								Severity = (Severity)ToInt16(payload, 2), 
								EGU = egu, 
								HighDisplayLimit = ToInt32(payload, 4 + offset), 
								LowDisplayLimit = ToInt32(payload, 8 + offset), 
								HighAlertLimit = ToInt32(payload, 12 + offset), 
								HighWarnLimit = ToInt32(payload, 16 + offset), 
								LowWarnLimit = ToInt32(payload, 20 + offset), 
								LowAlertLimit = ToInt32(payload, 24 + offset), 
								Value = tResult
							};
					}

					// -> extended Controltypes
				case EpicsType.Control_Short:
					{
						egu = ToString(payload, 4);
						offset = 1 + egu.Length;

						var tResult = new short[datacount];
						for (; i < datacount; i++)
						{
							tResult[i] = ToInt16(payload, (offset + 20) + (i * 2));
						}

						return new ExtControl<short[]> {
								Status = (Status)ToInt16(payload, 0), 
								Severity = (Severity)ToInt16(payload, 2), 
								EGU = egu, 
								HighDisplayLimit = ToInt16(payload, 4 + offset), 
								LowDisplayLimit = ToInt16(payload, 6 + offset), 
								HighAlertLimit = ToInt16(payload, 8 + offset), 
								HighWarnLimit = ToInt16(payload, 10 + offset), 
								LowWarnLimit = ToInt16(payload, 12 + offset), 
								LowAlertLimit = ToInt16(payload, 14 + offset), 
								HighControlLimit = ToInt16(payload, 16 + offset), 
								LowControlLimit = ToInt16(payload, 18 + offset), 
								Value = tResult
							};
					}

				case EpicsType.Control_Float:
					{
						egu = ToString(payload, 8);
						offset = 1 + egu.Length;

						var tResult = new float[datacount];
						for (; i < datacount; i++)
						{
							tResult[i] = ToFloat(payload, (offset + 40) + (i * 4));
						}

						return new ExtControl<float[]> {
								Status = (Status)ToInt16(payload, 0), 
								Severity = (Severity)ToInt16(payload, 2), 
								Precision = ToInt16(payload, 4), 
								EGU = egu, 
								HighDisplayLimit = ToFloat(payload, 8 + offset), 
								LowDisplayLimit = ToFloat(payload, 12 + offset), 
								HighAlertLimit = ToFloat(payload, 16 + offset), 
								HighWarnLimit = ToFloat(payload, 20 + offset), 
								LowWarnLimit = ToFloat(payload, 24 + offset), 
								LowAlertLimit = ToFloat(payload, 28 + offset), 
								HighControlLimit = ToFloat(payload, 32 + offset), 
								LowControlLimit = ToFloat(payload, 36 + offset), 
								Value = tResult
							};
					}

				case EpicsType.Control_Double:
					{
						egu = ToString(payload, 8);
						offset = 1 + egu.Length;

						var tResult = new double[datacount];
						for (; i < datacount; i++)
						{
							tResult[i] = ToDouble(payload, (offset + 72) + (i * 8));
						}

						return new ExtControl<double[]> {
								Status = (Status)ToInt16(payload, 0), 
								Severity = (Severity)ToInt16(payload, 2), 
								Precision = ToInt16(payload, 4), 
								EGU = egu, 
								HighDisplayLimit = ToDouble(payload, 8 + offset), 
								LowDisplayLimit = ToDouble(payload, 16 + offset), 
								HighAlertLimit = ToDouble(payload, 24 + offset), 
								HighWarnLimit = ToDouble(payload, 32 + offset), 
								LowWarnLimit = ToDouble(payload, 40 + offset), 
								LowAlertLimit = ToDouble(payload, 48 + offset), 
								HighControlLimit = ToDouble(payload, 56 + offset), 
								LowControlLimit = ToDouble(payload, 64 + offset), 
								Value = tResult
							};
					}

				case EpicsType.Control_String:
					{
						var tResult = new string[datacount];
						for (; i < datacount; i++)
						{
							tResult[i] = ToString(payload, 4 + (i * 40));
						}

						return new ExtType<string[]> { Status = (Status)ToInt16(payload, 0), Severity = (Severity)ToInt16(payload, 2), Value = tResult };
					}

				case EpicsType.Control_Int:
					{
						egu = ToString(payload, 4);
						offset = 1 + egu.Length;

						var tResult = new int[datacount];
						for (; i < datacount; i++)
						{
							tResult[i] = ToInt32(payload, (offset + 36) + (i * 4));
						}

						return new ExtControl<int[]> {
								Status = (Status)ToInt16(payload, 0), 
								Severity = (Severity)ToInt16(payload, 2), 
								EGU = egu, 
								HighDisplayLimit = ToInt32(payload, 4 + offset), 
								LowDisplayLimit = ToInt32(payload, 8 + offset), 
								HighAlertLimit = ToInt32(payload, 12 + offset), 
								HighWarnLimit = ToInt32(payload, 16 + offset), 
								LowWarnLimit = ToInt32(payload, 20 + offset), 
								LowAlertLimit = ToInt32(payload, 24 + offset), 
								HighControlLimit = ToInt32(payload, 28 + offset), 
								LowControlLimit = ToInt32(payload, 32 + offset), 
								Value = tResult
							};
					}

				default:
					return null;
			}
		}

		/// <summary>
		/// The object to byte.
		/// </summary>
		/// <param name="src">
		/// The src.
		/// </param>
		/// <param name="epicsType">
		/// The epics type.
		/// </param>
		/// <returns>
		/// </returns>
		internal static byte[] objectToByte(object src, EpicsType epicsType)
		{
			switch (epicsType)
			{
				case EpicsType.String:
					return ToByteArray((string)src);
				case EpicsType.Short:
					return ToByteArray((short)src);
				case EpicsType.Float:
					return ToByteArray((float)src);
				case EpicsType.SByte:
					return ToByteArray((sbyte)src);
				case EpicsType.Int:
					return ToByteArray((int)src);
				case EpicsType.Bool:
					return ToByteArray((bool)src);
				case EpicsType.Double:
					return ToByteArray((double)src);
				default:
					return null;
			}
		}

		/// <summary>
		/// The object to byte.
		/// </summary>
		/// <param name="src">
		/// The src.
		/// </param>
		/// <param name="epicsType">
		/// The epics type.
		/// </param>
		/// <param name="record">
		/// The record.
		/// </param>
		/// <returns>
		/// </returns>
		internal static byte[] objectToByte(object src, EpicsType epicsType, EpicsRecord record)
		{
			return objectToByteCaller(src, epicsType, record, record.dataCount);
		}

		/// <summary>
		/// The object to byte.
		/// </summary>
		/// <param name="src">
		/// The src.
		/// </param>
		/// <param name="epicsType">
		/// The epics type.
		/// </param>
		/// <param name="record">
		/// The record.
		/// </param>
		/// <param name="dataCount">
		/// The data count.
		/// </param>
		/// <returns>
		/// </returns>
		internal static byte[] objectToByte(object src, EpicsType epicsType, EpicsRecord record, int dataCount)
		{
			return objectToByteCaller(src, epicsType, record, dataCount);
		}

		/// <summary>
		/// Function for full change from an Object to Byte.
		/// </summary>
		/// <typeparam name="dataType">
		/// Datatype of the source
		/// </typeparam>
		/// <param name="src">
		/// Src object which shall be transferred
		/// </param>
		/// <param name="epicsType">
		/// Target epics type
		/// </param>
		/// <param name="record">
		/// Record from where the value comes
		/// </param>
		/// <param name="dataCount">
		/// Count of data requested
		/// </param>
		/// <returns>
		/// </returns>
		internal static byte[] objectToByte<dataType>(object src, EpicsType epicsType, EpicsRecord record, int dataCount)
		{
			var i = 0;
			dataType[] source;
			byte[] result;

			if (src.GetType().IsGenericType)
			{
				source = ((EpicsArray<dataType>)src).Get(dataCount);
			}
			else
			{
				source = new[] { (dataType)Convert.ChangeType(src, typeof(dataType)) };
			}

			// reset mem to 0 so we start from scratch
			mem.Seek(0, SeekOrigin.Begin);
			mem.SetLength(0);

			switch (epicsType)
			{
				case EpicsType.String:
					if (dataCount == 1)
					{
						return ToByteArray(Convert.ToString(source[0]));
					}
					else
					{
						mem.Capacity = dataCount * 40;
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToString(source[i])));
						}

						return mem.GetBuffer();
					}

				case EpicsType.Short:
					if (dataCount == 1)
					{
						return ToByteArray(Convert.ToUInt16(source[0]));
					}
					else
					{
						mem.Capacity = dataCount * 2;
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToUInt16(source[i])));
						}

						return mem.GetBuffer();
					}

				case EpicsType.Float:
					if (dataCount == 1)
					{
						return ToByteArray(Convert.ToSingle(source[0]));
					}
					else
					{
						mem.Capacity = dataCount * 4;
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToSingle(source[i])));
						}

						return mem.GetBuffer();
					}

				case EpicsType.SByte:
					if (dataCount == 1)
					{
						return ToByteArray(Convert.ToSByte(source[0]));
					}
					else
					{
						mem.Capacity = dataCount;
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToSByte(source[i])));
						}

						return mem.GetBuffer();
					}

				case EpicsType.Int:
					if (dataCount == 1)
					{
						return ToByteArray(Convert.ToUInt32(source[0]));
					}
					else
					{
						mem.Capacity = dataCount * 4;
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToUInt32(source[i])));
						}

						return mem.GetBuffer();
					}

				case EpicsType.Double:
					if (dataCount == 1)
					{
						return ToByteArray(Convert.ToDouble(source[0]));
					}
					else
					{
						mem.Capacity = dataCount * 8;
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToDouble(source[i])));
						}

						return mem.GetBuffer();
					}

				default:
					return null;

				case EpicsType.Status_Double:
					mem.Capacity = 2 + 2 + (dataCount * 8);
					writer.Write(ToByteArray((short)record.STAT));
					writer.Write(ToByteArray((short)record.SEVR));
					if (dataCount == 1)
					{
						writer.Write(ToByteArray(Convert.ToDouble(source[0])));
					}
					else
					{
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToDouble(source[i])));
						}
					}

					return mem.GetBuffer();
				case EpicsType.Status_Float:
					mem.Capacity = 2 + 2 + (dataCount * 4);
					writer.Write(ToByteArray((short)record.STAT));
					writer.Write(ToByteArray((short)record.SEVR));
					if (dataCount == 1)
					{
						writer.Write(ToByteArray(Convert.ToSingle(source[0])));
					}
					else
					{
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToSingle(source[i])));
						}
					}

					return mem.GetBuffer();
				case EpicsType.Status_Int:
					mem.Capacity = 2 + 2 + (dataCount * 4);
					writer.Write(ToByteArray((short)record.STAT));
					writer.Write(ToByteArray((short)record.SEVR));
					if (dataCount == 1)
					{
						writer.Write(ToByteArray(Convert.ToUInt32(source[0])));
					}
					else
					{
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToUInt32(source[i])));
						}
					}

					return mem.GetBuffer();
				case EpicsType.Status_SByte:
					mem.Capacity = 2 + 2 + dataCount;
					writer.Write(ToByteArray((short)record.STAT));
					writer.Write(ToByteArray((short)record.SEVR));
					if (dataCount == 1)
					{
						writer.Write(ToByteArray(Convert.ToSByte(source[0])));
					}
					else
					{
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToSByte(source[i])));
						}
					}

					return mem.GetBuffer();
				case EpicsType.Status_Short:
					mem.Capacity = 2 + 2 + (dataCount * 2);
					writer.Write(ToByteArray((short)record.STAT));
					writer.Write(ToByteArray((short)record.SEVR));
					if (dataCount == 1)
					{
						writer.Write(ToByteArray(Convert.ToUInt16(source[0])));
					}
					else
					{
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToUInt16(source[i])));
						}
					}

					return mem.GetBuffer();
				case EpicsType.Status_String:
					mem.Capacity = 2 + 2 + (dataCount * 40);
					writer.Write(ToByteArray((short)record.STAT));
					writer.Write(ToByteArray((short)record.SEVR));
					if (dataCount == 1)
					{
						writer.Write(ToByteArray(Convert.ToString(source[0]), true));
					}
					else
					{
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToString(source[i]), true));
						}
					}

					return mem.GetBuffer();

				case EpicsType.Time_Double:
					mem.Capacity = 2 + 2 + (dataCount * 8) + 4 + (record.TIME == null ? 0 : 8);
					writer.Write(ToByteArray((short)record.STAT));
					writer.Write(ToByteArray((short)record.SEVR));
					writer.Write(ToByteArray(record.TIME));
					writer.Write(ToByteArray((UInt32)record.PREC));
					if (dataCount == 1)
					{
						writer.Write(ToByteArray(Convert.ToDouble(source[0])));
					}
					else
					{
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToDouble(source[i])));
						}
					}

					return mem.GetBuffer();
				case EpicsType.Time_Float:
					mem.Capacity = 2 + 2 + (dataCount * 4) + (record.TIME == null ? 0 : 8);
					writer.Write(ToByteArray((short)record.STAT));
					writer.Write(ToByteArray((short)record.SEVR));
					writer.Write(ToByteArray(record.TIME));
					if (dataCount == 1)
					{
						writer.Write(ToByteArray(Convert.ToSingle(source[0])));
					}
					else
					{
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToSingle(source[i])));
						}
					}

					return mem.GetBuffer();
				case EpicsType.Time_Int:
					mem.Capacity = 2 + 2 + (dataCount * 4) + (record.TIME == null ? 0 : 8);
					writer.Write(ToByteArray((short)record.STAT));
					writer.Write(ToByteArray((short)record.SEVR));
					writer.Write(ToByteArray(record.TIME));
					if (dataCount == 1)
					{
						writer.Write(ToByteArray(Convert.ToUInt32(source[0])));
					}
					else
					{
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToUInt32(source[i])));
						}
					}

					return mem.GetBuffer();
				case EpicsType.Time_Short:
					mem.Capacity = 2 + 2 + 2 + (dataCount * 2) + (record.TIME == null ? 0 : 8);
					writer.Write(ToByteArray((short)record.STAT));
					writer.Write(ToByteArray((short)record.SEVR));
					writer.Write(ToByteArray(record.TIME));
					writer.Write(new byte[2]);
					if (dataCount == 1)
					{
						writer.Write(ToByteArray(Convert.ToUInt16(source[0])));
					}
					else
					{
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToUInt16(source[i])));
						}
					}

					return mem.GetBuffer();
				case EpicsType.Time_String:
					mem.Capacity = 2 + 2 + (dataCount * 40);
					writer.Write(ToByteArray((short)record.STAT));
					writer.Write(ToByteArray((short)record.SEVR));
					if (dataCount == 1)
					{
						writer.Write(ToByteArray(Convert.ToString(source[0]), true));
					}
					else
					{
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToString(source[i]), true));
						}
					}

					return mem.GetBuffer();
				case EpicsType.Time_SByte:
					mem.Capacity = 2 + 2 + dataCount + (record.TIME == null ? 0 : 8);
					writer.Write(ToByteArray((short)record.STAT));
					writer.Write(ToByteArray((short)record.SEVR));
					writer.Write(ToByteArray(record.TIME));
					if (dataCount == 1)
					{
						writer.Write(ToByteArray(Convert.ToSByte(source[0])));
					}
					else
					{
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToSByte(source[i])));
						}
					}

					return mem.GetBuffer();

				case EpicsType.Control_Double:
					mem.Capacity = 2 + 2 + 4 + (record.EGU.Length + 1) + (8 * 8) + (dataCount * 8);
					writer.Write(ToByteArray((short)record.STAT));
					writer.Write(ToByteArray((short)record.SEVR));
					writer.Write(ToByteArray(record.PREC));
					writer.Write(ToByteArray(record.EGU));

					// stop egu
					writer.Write(new byte[0]);
					writer.Write(ToByteArray(record.HighDisplayLimit));
					writer.Write(ToByteArray(record.LowDisplayLimit));
					writer.Write(ToByteArray(record.HIHI));
					writer.Write(ToByteArray(record.HIGH));
					writer.Write(ToByteArray(record.LOW));
					writer.Write(ToByteArray(record.LOLO));
					writer.Write(ToByteArray(record.HOPR));
					writer.Write(ToByteArray(record.LOPR));
					if (dataCount == 1)
					{
						writer.Write(ToByteArray(Convert.ToDouble(source[0])));
					}
					else
					{
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToDouble(source[i])));
						}
					}

					return mem.GetBuffer();
				case EpicsType.Control_Float:
					mem.Capacity = 2 + 2 + 4 + (record.EGU.Length + 1) + (8 * 4) + (dataCount * 4);
					writer.Write(ToByteArray((short)record.STAT));
					writer.Write(ToByteArray((short)record.SEVR));
					writer.Write(ToByteArray(record.PREC));
					writer.Write(ToByteArray(record.EGU));

					// stop egu
					writer.Write(new byte[0]);
					writer.Write(ToByteArray((float)record.HighDisplayLimit));
					writer.Write(ToByteArray((float)record.LowDisplayLimit));
					writer.Write(ToByteArray((float)record.HIHI));
					writer.Write(ToByteArray((float)record.HIGH));
					writer.Write(ToByteArray((float)record.LOW));
					writer.Write(ToByteArray((float)record.LOLO));
					writer.Write(ToByteArray((float)record.HOPR));
					writer.Write(ToByteArray((float)record.LOPR));
					if (dataCount == 1)
					{
						writer.Write(ToByteArray(Convert.ToSingle(source[0])));
					}
					else
					{
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToSingle(source[i])));
						}
					}

					return mem.GetBuffer();
				case EpicsType.Control_Int:
					mem.Capacity = 2 + 2 + 4 + (record.EGU.Length + 1) + (8 * 4) + (dataCount * 4);
					writer.Write(ToByteArray((short)record.STAT));
					writer.Write(ToByteArray((short)record.SEVR));
					writer.Write(ToByteArray(record.PREC));
					writer.Write(ToByteArray(record.EGU));

					// stop egu
					writer.Write(new byte[0]);
					writer.Write(ToByteArray((int)record.HighDisplayLimit));
					writer.Write(ToByteArray((int)record.LowDisplayLimit));
					writer.Write(ToByteArray((int)record.HIHI));
					writer.Write(ToByteArray((int)record.HIGH));
					writer.Write(ToByteArray((int)record.LOW));
					writer.Write(ToByteArray((int)record.LOLO));
					writer.Write(ToByteArray((int)record.HOPR));
					writer.Write(ToByteArray((int)record.LOPR));
					if (dataCount == 1)
					{
						writer.Write(ToByteArray(Convert.ToUInt32(source[0])));
					}
					else
					{
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToUInt32(source[i])));
						}
					}

					return mem.GetBuffer();
				case EpicsType.Control_Short:
					mem.Capacity = 2 + 2 + 4 + (record.EGU.Length + 1) + (8 * 2) + (dataCount * 2);
					writer.Write(ToByteArray((short)record.STAT));
					writer.Write(ToByteArray((short)record.SEVR));
					writer.Write(ToByteArray(record.PREC));
					writer.Write(ToByteArray(record.EGU));

					// stop egu
					writer.Write(new byte[0]);
					writer.Write(ToByteArray((short)record.HighDisplayLimit));
					writer.Write(ToByteArray((short)record.LowDisplayLimit));
					writer.Write(ToByteArray((short)record.HIHI));
					writer.Write(ToByteArray((short)record.HIGH));
					writer.Write(ToByteArray((short)record.LOW));
					writer.Write(ToByteArray((short)record.LOLO));
					writer.Write(ToByteArray((short)record.HOPR));
					writer.Write(ToByteArray((short)record.LOPR));
					if (dataCount == 1)
					{
						writer.Write(ToByteArray(Convert.ToUInt16(source[0])));
					}
					else
					{
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToUInt16(source[i])));
						}
					}

					return mem.GetBuffer();
				case EpicsType.Control_SByte:
					throw new Exception("NOT IMPLEMENTED");
				case EpicsType.Control_String:
					mem.Capacity = 2 + 2 + (dataCount * 40);
					writer.Write(ToByteArray((short)record.STAT));
					writer.Write(ToByteArray((short)record.SEVR));
					if (dataCount == 1)
					{
						writer.Write(ToByteArray(Convert.ToString(source[0]), true));
					}
					else
					{
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToString(source[i]), true));
						}
					}

					return mem.GetBuffer();

				case EpicsType.Display_Double:
					mem.Capacity = 2 + 2 + 4 + (record.EGU.Length + 1) + (6 * 8) + (dataCount * 8);
					writer.Write(ToByteArray((short)record.STAT));
					writer.Write(ToByteArray((short)record.SEVR));
					writer.Write(ToByteArray(record.PREC));
					writer.Write(ToByteArray(record.EGU));

					// stop egu
					writer.Write(new byte[0]);
					writer.Write(ToByteArray(record.HighDisplayLimit));
					writer.Write(ToByteArray(record.LowDisplayLimit));
					writer.Write(ToByteArray(record.HIHI));
					writer.Write(ToByteArray(record.HIGH));
					writer.Write(ToByteArray(record.LOW));
					writer.Write(ToByteArray(record.LOLO));
					if (dataCount == 1)
					{
						writer.Write(ToByteArray(Convert.ToDouble(source[0])));
					}
					else
					{
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToDouble(source[i])));
						}
					}

					return mem.GetBuffer();
				case EpicsType.Display_Float:
					mem.Capacity = 2 + 2 + 4 + (record.EGU.Length + 1) + (6 * 4) + (dataCount * 4);
					writer.Write(ToByteArray((short)record.STAT));
					writer.Write(ToByteArray((short)record.SEVR));
					writer.Write(ToByteArray(record.PREC));
					writer.Write(ToByteArray(record.EGU));

					// stop egu
					writer.Write(new byte[0]);
					writer.Write(ToByteArray((float)record.HighDisplayLimit));
					writer.Write(ToByteArray((float)record.LowDisplayLimit));
					writer.Write(ToByteArray((float)record.HIHI));
					writer.Write(ToByteArray((float)record.HIGH));
					writer.Write(ToByteArray((float)record.LOW));
					writer.Write(ToByteArray((float)record.LOLO));
					if (dataCount == 1)
					{
						writer.Write(ToByteArray(Convert.ToSingle(source[0])));
					}
					else
					{
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToSingle(source[i])));
						}
					}

					return mem.GetBuffer();
				case EpicsType.Display_Int:
					mem.Capacity = 2 + 2 + 4 + (record.EGU.Length + 1) + (6 * 4) + (dataCount * 4);
					writer.Write(ToByteArray((short)record.STAT));
					writer.Write(ToByteArray((short)record.SEVR));
					writer.Write(ToByteArray(record.PREC));
					writer.Write(ToByteArray(record.EGU));

					// stop egu
					writer.Write(new byte[0]);
					writer.Write(ToByteArray((int)record.HighDisplayLimit));
					writer.Write(ToByteArray((int)record.LowDisplayLimit));
					writer.Write(ToByteArray((int)record.HIHI));
					writer.Write(ToByteArray((int)record.HIGH));
					writer.Write(ToByteArray((int)record.LOW));
					writer.Write(ToByteArray((int)record.LOLO));
					if (dataCount == 1)
					{
						writer.Write(ToByteArray(Convert.ToUInt32(source[0])));
					}
					else
					{
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToUInt32(source[i])));
						}
					}

					return mem.GetBuffer();
				case EpicsType.Display_Short:
					mem.Capacity = 2 + 2 + 4 + (record.EGU.Length + 1) + (6 * 2) + (dataCount * 2);
					writer.Write(ToByteArray((short)record.STAT));
					writer.Write(ToByteArray((short)record.SEVR));
					writer.Write(ToByteArray(record.PREC));
					writer.Write(ToByteArray(record.EGU));

					// stop egu
					writer.Write(new byte[0]);
					writer.Write(ToByteArray((short)record.HighDisplayLimit));
					writer.Write(ToByteArray((short)record.LowDisplayLimit));
					writer.Write(ToByteArray((short)record.HIHI));
					writer.Write(ToByteArray((short)record.HIGH));
					writer.Write(ToByteArray((short)record.LOW));
					writer.Write(ToByteArray((short)record.LOLO));
					if (dataCount == 1)
					{
						writer.Write(ToByteArray(Convert.ToUInt16(source[0])));
					}
					else
					{
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToUInt16(source[i])));
						}
					}

					return mem.GetBuffer();
				case EpicsType.Display_SByte:
					throw new Exception("NOT IMPLEMENTED");
				case EpicsType.Display_String:
					mem.Capacity = 2 + 2 + (dataCount * 40);
					writer.Write(ToByteArray((short)record.STAT));
					writer.Write(ToByteArray((short)record.SEVR));
					if (dataCount == 1)
					{
						writer.Write(ToByteArray(Convert.ToString(source[0]), true));
					}
					else
					{
						for (; i < dataCount; i++)
						{
							writer.Write(ToByteArray(Convert.ToString(source[i]), true));
						}
					}

					return mem.GetBuffer();
			}
		}

		/// <summary>
		/// Recalls the object to Byte function with the right datatype-struct for easy coding
		/// </summary>
		private static byte[] objectToByteCaller(object src, EpicsType epicsType, EpicsRecord record, int dataCount)
		{
			var valueType = record.GetValueType();

			if (valueType == typeof(string))
			{
				return objectToByte<string>(src, epicsType, record, dataCount);
			}
			else if (valueType == typeof(int))
			{
				return objectToByte<int>(src, epicsType, record, dataCount);
			}
			else if (valueType == typeof(short))
			{
				return objectToByte<short>(src, epicsType, record, dataCount);
			}
			else if (valueType == typeof(float))
			{
				return objectToByte<float>(src, epicsType, record, dataCount);
			}
			else if (valueType == typeof(double))
			{
				return objectToByte<double>(src, epicsType, record, dataCount);
			}
			else if (valueType == typeof(sbyte))
			{
				return objectToByte<sbyte>(src, epicsType, record, dataCount);
			}
			else
			{
				throw new Exception("Wrong DataType defined");
			}
		}
	}
}