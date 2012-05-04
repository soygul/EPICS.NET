// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProviderTests.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Tests.Integration.ChannelAccess.ProviderTests
{
	using System;
	using System.IO;
	using System.Reflection;

	using Epics.ChannelAccess.Provider;

	using Xunit;

	public class ProviderTests
	{
		[Fact]
		public void GetProviders()
		{
			var plugins = Provider.GetProviders(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
			foreach (var plugin in plugins)
			{
				Assert.True(plugin.Contains("ChannelAccess.") && !plugin.EndsWith(".dll"));
			}
		}
	}
}
