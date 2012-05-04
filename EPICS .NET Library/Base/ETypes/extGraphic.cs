// --------------------------------------------------------------------------------------------------------------------
// <copyright file="extGraphic.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base.ETypes
{
	/// <summary>
	/// extended epics graphic type <br/> serves severity, status, value, precision (for double and float), unittype 
	///   and a bunch of limits.
	/// </summary>
	/// <typeparam name="dType">
	/// generic datatype for value
	/// </typeparam>
	public class ExtGraphic<dType> : ExtType<dType>
	{
		/// <summary>
		///   EnGineer Unit of the value
		/// </summary>
		public string EGU { get; internal set; }

		/// <summary>
		///   High limit for incorrect operation
		/// </summary>
		public double HighAlertLimit { get; internal set; }

		/// <summary>
		///   Highest possible value which is visible
		/// </summary>
		public double HighDisplayLimit { get; internal set; }

		/// <summary>
		///   High limit for correct operation
		/// </summary>
		public double HighWarnLimit { get; internal set; }

		/// <summary>
		///   Low limit for incorrect operation
		/// </summary>
		public double LowAlertLimit { get; internal set; }

		/// <summary>
		///   Lowest possible value which is visible
		/// </summary>
		public double LowDisplayLimit { get; internal set; }

		/// <summary>
		///   Low limit for correct operation.
		/// </summary>
		public double LowWarnLimit { get; internal set; }

		/// <summary>
		///   Epics defined precision of the valuem, only set for double or float
		/// </summary>
		public short Precision { get; internal set; }

		/// <summary>
		/// builds a string line of all properties
		/// </summary>
		/// <returns>
		/// comma seperated string of keys and values
		/// </returns>
		public new string ToString()
		{
			return
				string.Format(
					"Value:{0},Status:{1},Severity:{2},EGU:{3},Precision:{4},"
					+ "LowDisplayLimit:{5},LowAlertLimit:{6},LowWarnLimit:{7},"
					+ "HighWarnLimit:{8},HighAlertLimit:{9},HighDisplayLimit:{10}", 
					this.Value, 
					this.Status, 
					this.Severity, 
					this.EGU, 
					this.Precision, 
					this.LowDisplayLimit, 
					this.LowAlertLimit, 
					this.LowWarnLimit, 
					this.HighWarnLimit, 
					this.HighAlertLimit, 
					this.HighDisplayLimit);
		}
	}
}