// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProviderInfo.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.ChannelAccess.EPICS
{
	using Epics.ChannelAccess.Provider;

	public class ProviderInfo : IProviderInfo
	{
		public string PlugInAuthor
		{
			get
			{
				return "Teoman Soygul (www.soygul.com)";
			}
		}

		public string ProviderName
		{
			get
			{
				return "EPICS Channel Access (Experimental Physics and Industrial Control System)";
			}
		}
	}
}
