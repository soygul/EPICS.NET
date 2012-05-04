// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CAConstants.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base.Constants
{
	/// <summary>
	/// The ca constants.
	/// </summary>
	internal static class CAConstants
	{
		/// <summary>
		///   Default priority for channel connections
		/// </summary>
		public const ushort CA_DEFAULT_PRIORITY = 0;

		/// <summary>
		///   Timeout for echo request
		/// </summary>
		public const uint CA_ECHO_TIMEOUT = 5000;

		/// <summary>
		///   Size of extended channel access message header
		/// </summary>
		public const ushort CA_EXTENDED_MESSAGE_HEADER_SIZE = CA_MESSAGE_HEADER_SIZE + 8;

		/// <summary>
		///   Major revision of channel access protocol implemented in this library
		/// </summary>
		public const ushort CA_MAJOR_PROTOCOL_REVISION = 4;

		/// <summary>
		///   Size of channel access message header
		/// </summary>
		public const ushort CA_MESSAGE_HEADER_SIZE = 16;

		/// <summary>
		///   Minor revision of channel access protocol implemented in this library
		/// </summary>
		public const ushort CA_MINOR_PROTOCOL_REVISION = 11;

		/// <summary>
		///   Network port base
		/// </summary>
		public const ushort CA_PORT_BASE = 5064;

		/// <summary>
		///   Bitmask for read access
		/// </summary>
		public const uint CA_PROTO_ACCESS_RIGHT_READ = 0x00000001;

		/// <summary>
		///   Bitmask for write access
		/// </summary>
		public const uint CA_PROTO_ACCESS_RIGHT_WRITE = 0x00000002;

		/// <summary>
		///   Default UDP port of channel access repeater
		/// </summary>
		public const ushort CA_REPEATER_PORT = CA_PORT_BASE + 2 * CA_MAJOR_PROTOCOL_REVISION + 1;

		/// <summary>
		///   Response after channel search request is not required
		///   (usually set for UDP)
		/// </summary>
		public const ushort CA_SEARCH_DONTREPLY = 0x0005;

		/// <summary>
		///   Response after channel search request is required
		///   (usually set for TCP)
		/// </summary>
		public const ushort CA_SEARCH_DOREPLY = 0x0010;

		/// <summary>
		///   Default TCP port of channel access server
		/// </summary>
		public const ushort CA_SERVER_PORT = CA_PORT_BASE + 2 * CA_MAJOR_PROTOCOL_REVISION;

		/// <summary>
		///   Unknown minor protocol revision
		/// </summary>
		public const ushort CA_UNKNOWN_MINOR_PROTOCOL_REVISION = 0;

		/// <summary>
		///   The don t_ reply.
		/// </summary>
		public const ushort DONT_REPLY = 5;

		/// <summary>
		///   The d o_ reply.
		/// </summary>
		public const ushort DO_REPLY = 10;
	}
}