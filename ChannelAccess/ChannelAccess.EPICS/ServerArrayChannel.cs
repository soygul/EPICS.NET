// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServerArrayChannel.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.ChannelAccess.EPICS
{
	using System;

	using Epics.ChannelAccess.Provider;
	using Epics.Server;

	[Obsolete("This class is here just for legacy EPICS v1 compatibility.")]
	public class ServerArrayChannel<T> : Channel, IServerArrayChannel<T>
	{
		internal ServerArrayChannel(EpicsArrayRecord<T> epicsArrayRecord)
		{
			
		}
	}
}
