// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandID.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base.Constants
{
	/// <summary>
	/// The command id.
	/// </summary>
	/// <remarks>
	/// Channel access command IDs
	/// </remarks>
	internal static class CommandID
	{
		/// <summary>
		///   Channel access rights
		/// </summary>
		public const ushort CA_PROTO_ACCESS_RIGHTS = 0x16;

		/// <summary>
		///   Invalid response
		/// </summary>
		public const ushort CA_PROTO_BAD_RESPONSE = 0xFFFF; // unofficial

		/// <summary>
		///   Release channel resources
		/// </summary>
		public const ushort CA_PROTO_CLEAR_CHANNEL = 0x0C;

		/// <summary>
		///   Client user name
		/// </summary>
		public const ushort CA_PROTO_CLIENT_NAME = 0x14;

		/// <summary>
		///   Create channel
		/// </summary>
		public const ushort CA_PROTO_CREATE_CHAN = 0x12;

		/// <summary>
		///   Channel creation failed
		/// </summary>
		public const ushort CA_PROTO_CREATE_CH_FAIL = 0x1A;

		/// <summary>
		///   Ping CA server
		/// </summary>
		public const ushort CA_PROTO_ECHO = 0x17;

		/// <summary>
		///   Error during operation
		/// </summary>
		public const ushort CA_PROTO_ERROR = 0x0B;

		/// <summary>
		///   Disable monitor events
		/// </summary>
		public const ushort CA_PROTO_EVENTS_OFF = 0x08;

		/// <summary>
		///   Enable monitor events
		/// </summary>
		public const ushort CA_PROTO_EVENTS_ON = 0x09;

		/// <summary>
		///   Register monitor
		/// </summary>
		public const ushort CA_PROTO_EVENT_ADD = 0x01;

		/// <summary>
		///   Unregister monitor
		/// </summary>
		public const ushort CA_PROTO_EVENT_CANCEL = 0x02;

		/// <summary>
		///   Client host name
		/// </summary>
		public const ushort CA_PROTO_HOST_NAME = 0x15;

		/// <summary>
		///   Channel not found
		/// </summary>
		public const ushort CA_PROTO_NOT_FOUND = 0x0E;

		/// <summary>
		///   Read channel value (without notification)
		/// </summary>
		public const ushort CA_PROTO_READ = 0x03;

		/// <summary>
		///   Read channel value (with notification)
		/// </summary>
		public const ushort CA_PROTO_READ_NOTIFY = 0x0F;

		/// <summary>
		///   Repeater registration confirmation
		/// </summary>
		public const ushort CA_PROTO_REPEATER_CONFIRM = 0x11;

		/// <summary>
		///   Register client on repeater
		/// </summary>
		public const ushort CA_PROTO_REPEATER_REGISTER = 0x18;

		/// <summary>
		///   Server beacon
		/// </summary>
		public const ushort CA_PROTO_RSRV_IS_UP = 0x0D;

		/// <summary>
		///   Search for a channel
		/// </summary>
		public const ushort CA_PROTO_SEARCH = 0x06;

		/// <summary>
		///   Server is going down
		/// </summary>
		public const ushort CA_PROTO_SERVER_DISCONN = 0x1B;

		/// <summary>
		///   CA protocol version
		/// </summary>
		public const ushort CA_PROTO_VERSION = 0x00;

		/// <summary>
		///   Write channel value (without notification)
		/// </summary>
		public const ushort CA_PROTO_WRITE = 0x04;

		/// <summary>
		///   Write channel value (with notification)
		/// </summary>
		public const ushort CA_PROTO_WRITE_NOTIFY = 0x13;
	}
}