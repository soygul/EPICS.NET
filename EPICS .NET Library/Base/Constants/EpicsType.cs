// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpicsType.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Base.Constants
{
	/// <summary>
	/// This enum represents the different CA value types and their corresponding intval
	/// </summary>
	internal enum EpicsType : ushort
	{
		/// <summary>
		///   Plain string
		/// </summary>
		String, 

		/// <summary>
		///   Plain 16bit integer
		/// </summary>
		Short, 

		/// <summary>
		///   Plain 16bit integer
		/// </summary>
		Bool, 

		/// <summary>
		///   Plain 32bit floating point
		/// </summary>
		Float, 

		/// <summary>
		///   Plain enumeration (using 16bit unsigned integer)
		/// </summary>
		Enum, 

		/// <summary>
		///   Plain signed byte
		/// </summary>
		SByte, 

		/// <summary>
		///   Plain 32bit integer
		/// </summary>
		Int, 

		/// <summary>
		///   Plain 64bit floating point
		/// </summary>
		Double, 

		/// <summary>
		///   Extends plain string by status and severity
		/// </summary>
		Status_String, 

		/// <summary>
		///   Extends plain 16bit integer by status and severity
		/// </summary>
		Status_Short, 

		/// <summary>
		///   Extends plain 32bit floating point by status and severity
		/// </summary>
		Status_Float, 

		/// <summary>
		///   Extends plain enumeration by status and severity
		/// </summary>
		Status_Enum, 

		/// <summary>
		///   Extends plain signed byte by status and severity
		/// </summary>
		Status_SByte, 

		/// <summary>
		///   Extends plain 32bit integer by status and severity
		/// </summary>
		Status_Int, 

		/// <summary>
		///   Extends plain 64bit floating point by status and severity
		/// </summary>
		Status_Double, 

		/// <summary>
		///   Extends Status_String by timestamp
		/// </summary>
		Time_String, 

		/// <summary>
		///   Extends Status_Short by timestamp
		/// </summary>
		Time_Short, 

		/// <summary>
		///   Extends Status_Float by timestamp
		/// </summary>
		Time_Float, 

		/// <summary>
		///   Extends Status_Enum by timestamp
		/// </summary>
		Time_Enum, 

		/// <summary>
		///   Extends Status_SByte by timestamp
		/// </summary>
		Time_SByte, 

		/// <summary>
		///   Extends Status_Int by timestamp
		/// </summary>
		Time_Int, 

		/// <summary>
		///   Extends Status_Double by timestamp
		/// </summary>
		Time_Double, 

		/// <summary>
		///   Extends Status_String by display bounds (not used since
		///   a string cannot have display bounds)
		/// </summary>
		Display_String, 

		/// <summary>
		///   Extends Status_Short by display bounds
		/// </summary>
		Display_Short, 

		/// <summary>
		///   Extends Status_Float by display bounds
		/// </summary>
		Display_Float, 

		/// <summary>
		///   Extends Status_Enum by a list of enumeration labels
		/// </summary>
		Labeled_Enum, 

		/// <summary>
		///   Extends Status_SByte by display bounds
		/// </summary>
		Display_SByte, 

		/// <summary>
		///   Extends Status_Int by display bounds
		/// </summary>
		Display_Int, 

		/// <summary>
		///   Extends Status_Double by display bounds
		/// </summary>
		Display_Double, 

		/// <summary>
		///   Extends Display_String by control bounds (not used since
		///   a string cannot have control bounds)
		/// </summary>
		Control_String, 

		/// <summary>
		///   Extends Display_Short by control bounds
		/// </summary>
		Control_Short, 

		/// <summary>
		///   Extends Display_Float by control bounds
		/// </summary>
		Control_Float, 

		/// <summary>
		///   Not used since parent type is Labeled_Enum
		/// </summary>
		Control_Enum, 

		/// <summary>
		///   Extends Display_SByte by control bounds
		/// </summary>
		Control_SByte, 

		/// <summary>
		///   Extends Display_Int by control bounds
		/// </summary>
		Control_Int, 

		/// <summary>
		///   Extends Display_Double by control bounds
		/// </summary>
		Control_Double, 

		/// <summary>
		///   Defines an invalid data type
		/// </summary>
		Invalid = 0xFFFF
	}
}