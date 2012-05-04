// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRuleSet.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base
{
	#region Using Directives

	using System.Net;

	using Epics.Base.Constants;

	#endregion

	/// <summary>
	/// The rules changed delegate.
	/// </summary>
	public delegate void RulesChangedDelegate();

	/// <summary>
	/// Interface for a set of rules to allow a EpicsServer or EpicsGateWay to 
	///   determ if the Channel is read or writeable
	/// </summary>
	public interface IRuleSet
	{
		/// <summary>
		///   The rules changed.
		/// </summary>
		event RulesChangedDelegate RulesChanged;

		/// <summary>
		/// The get access rights.
		/// </summary>
		/// <param name="RemoteEndPoint">
		/// </param>
		/// <param name="Username">
		/// </param>
		/// <param name="ChannelName">
		/// </param>
		/// <returns>
		/// </returns>
		AccessRights GetAccessRights(EndPoint RemoteEndPoint, string Username, string ChannelName);
	}
}