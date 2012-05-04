// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubnetMask.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Net;
	using System.Net.NetworkInformation;
	using System.Text.RegularExpressions;

	#endregion

	/// <summary>
	/// The subnet mask.
	/// </summary>
	public class SubnetMask
	{
		/// <summary>
		///   The ip subnet dictionary.
		/// </summary>
		private static readonly Dictionary<long, long> IpSubnetDictionary = new Dictionary<long, long>();

		/// <summary>
		///   Initializes static members of the <see cref = "SubnetMask" /> class.
		/// </summary>
		static SubnetMask()
		{
			var Os = Environment.OSVersion.Platform.ToString();

			if (Os == "Unix")
			{
				GetUnixTable();
			}
			else
			{
				var interfaces = NetworkInterface.GetAllNetworkInterfaces();
				long longIp = 0;
				IPAddress ip = null;

				foreach (var iface in interfaces)
				{
					var prop = iface.GetIPProperties();
					if (prop.UnicastAddresses.Count == 0)
					{
						continue;
					}

					try
					{
						IpSubnetDictionary.Add(prop.UnicastAddresses[0].Address.Address, prop.UnicastAddresses[0].IPv4Mask.Address);
					}
					catch (Exception e)
					{
						continue;
					}
				}
			}
		}

		/// <summary>
		/// The mask by ip.
		/// </summary>
		/// <param name="ip">
		/// The ip.
		/// </param>
		/// <returns>
		/// The mask by ip.
		/// </returns>
		public static long MaskByIp(long ip)
		{
			if (IpSubnetDictionary.ContainsKey(ip))
			{
				return IpSubnetDictionary[ip];
			}
			else
			{
				return 0;
			}
		}

		/// <summary>
		/// The get unix table.
		/// </summary>
		private static void GetUnixTable()
		{
			var ifConfig = new Process();
			ifConfig.StartInfo.RedirectStandardOutput = true;
			ifConfig.StartInfo.UseShellExecute = false;

			ifConfig.StartInfo.FileName = "/sbin/ifconfig";

			ifConfig.Start();
			var reader = ifConfig.StandardOutput;
			ifConfig.WaitForExit();

			var output = reader.ReadToEnd();

			var regExp =
				new Regex(
					"inet addr:([0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3})" + ".*"
					+ "mask:([0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3})", 
					RegexOptions.IgnoreCase);
			var coll = regExp.Matches(output);

			for (var i = 0; i < coll.Count; i++)
			{
				var line = coll[i].Value;
				var ip =
					Regex.Match(
						Regex.Match(line, "inet addr:([0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3})").Value, 
						"([0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3})").Value;
				var mask =
					Regex.Match(
						Regex.Match(line, "(Mask:)([0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3})").Value, 
						"([0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3})").Value;

				IpSubnetDictionary.Add(IPAddress.Parse(ip).Address, IPAddress.Parse(mask).Address);
			}
		}
	}
}