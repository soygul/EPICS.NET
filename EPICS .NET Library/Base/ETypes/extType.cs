// --------------------------------------------------------------------------------------------------------------------
// <copyright file="extType.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base.ETypes
{
	using Epics.Base.Constants;

	/// <summary>
	/// extended epics type <br/> serves severity, status and value
	/// </summary>
	/// <typeparam name="dType">
	/// generic datatype for value
	/// </typeparam>
	public class ExtType<dType>
	{
		/// <summary>
		///   Severity of the channel serving this value
		/// </summary>
		public Severity Severity { get; internal set; }

		/// <summary>
		///   Status of the channel serving this value
		/// </summary>
		public Status Status { get; internal set; }

		/// <summary>
		///   current value, type transformation made by epics not c#
		/// </summary>
		public dType Value { get; internal set; }

		/// <summary>
		/// builds a string line of all properties
		/// </summary>
		/// <returns>
		/// The to string.
		/// </returns>
		public string ToString()
		{
			return string.Format("Value:{0},Status:{1},Severity:{2}", this.Value, this.Status, this.Severity);
		}
	}
}