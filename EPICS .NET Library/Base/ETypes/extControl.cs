// --------------------------------------------------------------------------------------------------------------------
// <copyright file="extControl.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base.ETypes
{
	/// <summary>
	/// extended epics control type <br/> serves severity, status, value, precision (for double and float), unittype 
	///   and a bunch of limits.
	/// </summary>
	/// <typeparam name="dType">
	/// generic datatype for value
	/// </typeparam>
	public class ExtControl<dType> : ExtGraphic<dType>
	{
		/// <summary>
		///   Highest Value which can be set
		/// </summary>
		public double HighControlLimit { get; internal set; }

		/// <summary>
		///   Lowest Value which can be set
		/// </summary>
		public double LowControlLimit { get; internal set; }

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
					+ "HighWarnLimit:{8},HighAlertLimit:{9},HighDisplayLimit:{10}," + "LowControlLimit:{11},HighControlLimit:{12}", 
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
					this.HighDisplayLimit, 
					this.LowControlLimit, 
					this.HighControlLimit);
		}
	}
}