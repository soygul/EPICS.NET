// --------------------------------------------------------------------------------------------------------------------
// <copyright file="extEnumType.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base.ETypes
{
	/// <summary>
	/// The ext enum type.
	/// </summary>
	internal class ExtEnumType : ExtType<short>
	{
		/// <summary>
		///   Gets or sets EnumArray.
		/// </summary>
		public string[] EnumArray { get; internal set; }
	}
}