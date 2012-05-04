// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Severity.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base.Constants
{
	/// <summary>
	/// Defines the severity of the current alarm
	/// </summary>
	public enum Severity : ushort
	{
		/// <summary>
		///   there is no alarm (value is betweend LowWarnLimit and HighWarnLimit)
		/// </summary>
		NO_ALARM = 0, 

		/// <summary>
		///   the alarm is minor (value is between LowWarnLimit and LowAlarmLimit or HighWarnLimit and HighAlarmLimit)
		/// </summary>
		MINOR = 1, 

		/// <summary>
		///   the alarm is major. its lower then the LowAlarmLimit or higher den the HighAlarmLimit
		/// </summary>
		MAJOR = 2, 

		/// <summary>
		///   Invalid Status
		/// </summary>
		INVALID = 3
	}
}