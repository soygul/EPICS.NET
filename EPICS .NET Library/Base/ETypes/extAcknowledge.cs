// --------------------------------------------------------------------------------------------------------------------
// <copyright file="extAcknowledge.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base.ETypes
{
	/// <summary>
	/// extended epics Acknowledge type <br/> serves severity, status, value, precision (for double and float), unittype 
	///   and a bunch of limits.
	/// </summary>
	/// <typeparam name="dType">
	/// generic datatype for value
	/// </typeparam>
	public class ExtAcknowledge<dType> : ExtType<dType>
	{
		/// <summary>
		///   Severity of the acknowledge serverity
		/// </summary>
		public short AcknowledgeSeverity { get; internal set; }

		/// <summary>
		///   transient of the acknowledge message
		/// </summary>
		public short AcknowledgeTransient { get; internal set; }
	}
}