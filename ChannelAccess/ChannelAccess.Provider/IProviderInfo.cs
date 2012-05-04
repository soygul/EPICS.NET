// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProviderInfo.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.ChannelAccess.Provider
{
	public interface IProviderInfo
	{
		string PlugInAuthor { get; }

		string ProviderName { get; }
	}
}
