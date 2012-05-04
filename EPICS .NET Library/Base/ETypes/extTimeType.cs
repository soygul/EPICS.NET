// --------------------------------------------------------------------------------------------------------------------
// <copyright file="extTimeType.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base.ETypes
{
	#region Using Directives

	using System;

	#endregion

	/// <summary>
	/// extended time epics type <br/> serves severity,status,value and time of last change.
	/// </summary>
	/// <typeparam name="dType">
	/// generic datatype for value
	/// </typeparam>
	public class ExtTimeType<dType> : ExtType<dType>
	{
		/// <summary>
		///   The timestamp base.
		/// </summary>
		private static DateTime TimestampBase;

		/// <summary>
		///   Initializes static members of the <see cref = "ExtTimeType" /> class.
		/// </summary>
		static ExtTimeType()
		{
			TimestampBase = new DateTime(1990, 1, 1, 0, 0, 0);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExtTimeType{dType}"/> class.
		/// </summary>
		/// <param name="secs">
		/// The secs.
		/// </param>
		/// <param name="nanoSecs">
		/// The nano secs.
		/// </param>
		internal ExtTimeType(long secs, long nanoSecs)
		{
			this.Time = (new DateTime(TimestampBase.Ticks + (secs * 10000000) + (nanoSecs / 100))).ToLocalTime();
		}

		/// <summary>
		///   Time of the last change on channel as local datetime
		/// </summary>
		public DateTime Time { get; private set; }

		/// <summary>
		/// builds a string line of all properties
		/// </summary>
		/// <returns>
		/// The to string.
		/// </returns>
		public new string ToString()
		{
			return string.Format("Value:{0},Status:{1},Severity:{2},Time:{3}", this.Value, this.Status, this.Severity, this.Time);
		}
	}
}