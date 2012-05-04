// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccessRights.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base.Constants
{
	/// <summary>
	/// Channel access rights
	/// </summary>
	public enum AccessRights
	{
		/// <summary>
		///   no write nor reade access
		/// </summary>
		NoAccess = 0, 

		/// <summary>
		///   it means you can only read.
		/// </summary>
		ReadOnly = 1, 

		/// <summary>
		///   you can only write
		/// </summary>
		WriteOnly = 2, 

		/// <summary>
		///   Read and Write Rights
		/// </summary>
		ReadAndWrite = 3
	}
}