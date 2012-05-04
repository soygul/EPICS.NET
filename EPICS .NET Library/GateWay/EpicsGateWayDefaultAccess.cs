// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsGateWayDefaultAccess.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.GateWay
{
	#region Using Directives

	using System.Net;

	using Epics.Base;
	using Epics.Base.Constants;

	#endregion

	/// <summary>
	/// The epics gate way default access.
	/// </summary>
	internal class EpicsGateWayDefaultAccess : IRuleSet
	{
		/// <summary>
		///   The rules changed.
		/// </summary>
		public event RulesChangedDelegate RulesChanged;

		/// <summary>
		/// The get access rights.
		/// </summary>
		/// <param name="RemoteEndPoint">
		/// The remote end point.
		/// </param>
		/// <param name="Username">
		/// The username.
		/// </param>
		/// <param name="ChannelName">
		/// The channel name.
		/// </param>
		/// <returns>
		/// </returns>
		public AccessRights GetAccessRights(EndPoint RemoteEndPoint, string Username, string ChannelName)
		{
			return AccessRights.ReadAndWrite;
		}
	}
}