// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChannelStatus.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base.Constants
{
	/// <summary>
	/// Current connection status of a channel
	/// </summary>
	public enum ChannelStatus
	{
		/// <summary>
		///   Channel was just created and is trying to be established.
		/// </summary>
		REQUESTED, 

		/// <summary>
		///   Channel is connected to an IOC and able to work.
		/// </summary>
		CONNECTED, 

		/// <summary>
		///   Channel was connected, lost connection, and is not working now. But will try to reconnect automaticly
		/// </summary>
		DISCONNECTED
	}
}