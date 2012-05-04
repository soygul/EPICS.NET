// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MonitorHandle.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.GateWay
{
	/// <summary>
	/// The monitor handle.
	/// </summary>
	internal struct MonitorHandle
	{
		/// <summary>
		///   The client id.
		/// </summary>
		internal uint ClientId;

		/// <summary>
		///   The net address.
		/// </summary>
		internal string NetAddress;

		/// <summary>
		///   The subscription id.
		/// </summary>
		internal uint SubscriptionId;

		/// <summary>
		/// Initializes a new instance of the <see cref="MonitorHandle"/> struct.
		/// </summary>
		/// <param name="netAddress">
		/// The net address.
		/// </param>
		/// <param name="clientId">
		/// The client id.
		/// </param>
		/// <param name="subscriptionId">
		/// The subscription id.
		/// </param>
		internal MonitorHandle(string netAddress, uint clientId, uint subscriptionId)
		{
			this.NetAddress = netAddress;
			this.ClientId = clientId;
			this.SubscriptionId = subscriptionId;
		}
	}
}