// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Provider.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.ChannelAccess.Provider
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Linq;
	using System.Reflection;

	/// <summary>
	/// Provides a singleton object of <see cref="IClient"/> and <see cref="IServer"/> interfaces for applications that needs
	/// just a single instance of one of them. Objects are lazy-loaded and thread safe.
	/// </summary>
	public sealed class Provider
	{
		static readonly object padlock = new object();

		private static volatile Assembly addInAssembly;

		private static volatile IClient client;

		private static volatile IServer server;

		static Provider()
		{
			if (addInAssembly == null)
			{
				lock (padlock)
				{
					if (addInAssembly == null)
					{
						// Load the default assembly before IServer/IClient is first accessed
						// addInAssembly = Assembly.Load("ChannelAccess.EPICS");
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the channel access provider type before using any client or server interfaces.
		/// </summary>
		/// <remarks>Providing the assembly name is sufficient: <c>Provider.ProviderType = "ChannelAccess.EPICS";</c></remarks>
		public static string ProviderType
		{
			get
			{
				return addInAssembly == null ? null : addInAssembly.GetName().Name;
			}

			set
			{
				// Assembly.LoadFile doesn't work due to different CWD at design time so taking some precautions
				if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
				{
					addInAssembly = Assembly.Load(value);
				}
				else
				{
					var directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) ?? Environment.CurrentDirectory;
					var fullPath = GetProviders(directory).Contains(value) ? Path.Combine(directory, value + ".dll")
						: Path.Combine(directory, "Plugins", value + ".dll");
					addInAssembly = Assembly.LoadFile(fullPath);
				}
			}
		}

		public static IClient Client
		{
			get
			{
				if (client == null)
				{
					lock (padlock)
					{
						if (client == null)
						{
							if (addInAssembly == null)
							{
								throw new Exception("Channel access provider type should be specified before first accessing any of the IClient/IServer interfaces through the UI components, simulators, or by other means.");
							}

							IEnumerable<Type> theClientType = from t in addInAssembly.GetTypes() where t.IsClass && (t.GetInterface(typeof(IClient).Name) != null) select t;
							client = (IClient)addInAssembly.CreateInstance(theClientType.First().FullName, true);
						}
					}
				}

				return client;
			}
		}

		public static IServer Server
		{
			get
			{
				if (server == null)
				{
					lock (padlock)
					{
						if (server == null)
						{
							if (addInAssembly == null)
							{
								throw new Exception("Channel access provider type should be specified before first accessing any of the IClient/IServer interfaces through the UI components, simulators, or by other means.");
							}

							IEnumerable<Type> theServerType = from t in addInAssembly.GetTypes() where t.IsClass && (t.GetInterface(typeof(IServer).Name) != null) select t;
							server = (IServer)addInAssembly.CreateInstance(theServerType.First().FullName, true);
						}
					}
				}

				return server;
			}
		}

		/// <summary>
		/// Gets all channel access provider plugins.
		/// Any channel access provider implements <see cref="IClient"/> and <see cref="IServer"/> interfaces.
		/// </summary>
		/// <returns>Enumerable list of channel access provider assembly names (i.e. 'ChannelAccess.EPICS');</returns>
		public static IEnumerable<string> GetProviders()
		{
			return GetProviders(null);
		}

		/// <summary>
		/// Gets the all available channel access provider plugins from a specified path.
		/// Any channel access provider implements <see cref="IClient"/> and <see cref="IServer"/> interfaces.
		/// </summary>
		/// <param name="path">Full path to directory where all plugins are kept.</param>
		/// <returns>Enumerable list of channel access provider assembly names (i.e. 'ChannelAccess.EPICS');</returns>
		public static IEnumerable<string> GetProviders(string path)
		{
			var fullPath = string.IsNullOrEmpty(path) ? Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) : path;
			return Directory.EnumerateFiles(fullPath, "ChannelAccess.*.dll").Where(file => !file.Contains("ChannelAccess.Provider.dll")).Select(Path.GetFileNameWithoutExtension);
		}
	}
}
