// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobHandle.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.GateWay
{
	/// <summary>
	/// The job handle.
	/// </summary>
	internal struct JobHandle
	{
		/// <summary>
		///   The empty one.
		/// </summary>
		public static JobHandle emptyOne = new JobHandle(string.Empty, 0, 0, 0);

		/// <summary>
		///   The address.
		/// </summary>
		public string Address;

		/// <summary>
		///   The channel client id.
		/// </summary>
		public uint ChannelClientId;

		/// <summary>
		///   The client id.
		/// </summary>
		public uint ClientId;

		/// <summary>
		///   The io id.
		/// </summary>
		public uint IoId;

		/// <summary>
		/// Initializes a new instance of the <see cref="JobHandle"/> struct.
		/// </summary>
		/// <param name="address">
		/// The address.
		/// </param>
		/// <param name="clientId">
		/// The client id.
		/// </param>
		/// <param name="gatewayId">
		/// The gateway id.
		/// </param>
		/// <param name="ioId">
		/// The io id.
		/// </param>
		internal JobHandle(string address, uint clientId, uint gatewayId, uint ioId)
		{
			this.Address = address;
			this.ClientId = clientId;
			this.IoId = ioId;
			this.ChannelClientId = gatewayId;
		}
	}
}