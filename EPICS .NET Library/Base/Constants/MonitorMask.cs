// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MonitorMask.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base.Constants
{
	/// <summary>
	/// Monitor Mask allows to define what a Monitor shall monitor
	/// </summary>
	public enum MonitorMask : ushort
	{
		/// <summary>
		///   Value type
		/// </summary>
		VALUE = 0x01, 

		/// <summary>
		///   Log type
		/// </summary>
		LOG = 0x02, 

		/// <summary>
		///   Value and log together
		/// </summary>
		VALUE_LOG = VALUE | LOG, 

		/// <summary>
		///   Alarm status type
		/// </summary>
		ALARM = 0x04, 

		/// <summary>
		///   Value and alarm together
		/// </summary>
		VALUE_ALARM = VALUE | ALARM, 

		/// <summary>
		///   Log and alarm together
		/// </summary>
		LOG_ALARM = LOG | ALARM, 

		/// <summary>
		///   All three (value, log and alarm) together
		/// </summary>
		ALL = VALUE | LOG | ALARM
	}
}