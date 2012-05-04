// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Status.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base.Constants
{
	/// <summary>
	/// Informs about the status of the device behind this Channel
	/// </summary>
	public enum Status : ushort
	{
		/// <summary>
		///   Device is working properly correctly
		/// </summary>
		NO_ALARM = 0, 

		/// <summary>
		///   The read.
		/// </summary>
		READ = 1, 

		/// <summary>
		///   The write.
		/// </summary>
		WRITE = 2, 

		/// <summary>
		///   Device is malfunctioning, and hit the upper Alarm Limit
		/// </summary>
		HIHI = 3, 

		/// <summary>
		///   Device is missbehaving, and hit the upper Warn Limit
		/// </summary>
		HIGH = 4, 

		/// <summary>
		///   Device is malfunctioning, and hit the lower Alarm Limit
		/// </summary>
		LOLO = 5, 

		/// <summary>
		///   Device is missbehaving, and hit theu lower Warn Limit
		/// </summary>
		LOW = 6, 

		/// <summary>
		///   The state.
		/// </summary>
		STATE = 7, 

		/// <summary>
		///   The cos.
		/// </summary>
		COS = 8, 

		/// <summary>
		///   The comm.
		/// </summary>
		COMM = 9, 

		/// <summary>
		///   The timeout.
		/// </summary>
		TIMEOUT = 10, 

		/// <summary>
		///   The hardwar e_ limit.
		/// </summary>
		HARDWARE_LIMIT = 11, 

		/// <summary>
		///   The calc.
		/// </summary>
		CALC = 12, 

		/// <summary>
		///   The scan.
		/// </summary>
		SCAN = 13, 

		/// <summary>
		///   The link.
		/// </summary>
		LINK = 14, 

		/// <summary>
		///   The soft.
		/// </summary>
		SOFT = 15, 

		/// <summary>
		///   The ba d_ sub.
		/// </summary>
		BAD_SUB = 16, 

		/// <summary>
		///   Undefined Status
		/// </summary>
		UDF = 17, 

		/// <summary>
		///   The disable.
		/// </summary>
		DISABLE = 18, 

		/// <summary>
		///   The simm.
		/// </summary>
		SIMM = 19, 

		/// <summary>
		///   The rea d_ access.
		/// </summary>
		READ_ACCESS = 20, 

		/// <summary>
		///   The writ e_ access.
		/// </summary>
		WRITE_ACCESS = 21
	}
}