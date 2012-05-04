// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Parameters.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Simulator.Core.Components
{
	public static class Parameters
	{
		/// <summary>
		/// Gets the electron beam drift tube radius (not the diameter!), measured in meters (m).
		/// </summary>
		public const double DriftTubeRadius = 25E-3; // 25mm

		/// <summary>
		/// Gets the overflow radius for electron beam that can get as close as to the wall of the drift tube, before it gets
		/// too dangerous. Measured in meters (m).
		/// </summary>
		public const double DesiredBeamPositionRange = 4E-4; // 0.4mm from center

		/// <summary>
		/// Gets the room temperature in Kelvins (K), which is Celsius + 273.
		/// </summary>
		public const double AmbientTemperature = 20 + 273;

		/// <summary>
		/// Gets the threshold as a percentage, on which the beam loss gets alarming and needs attention. Measured as a percentage (%).
		/// </summary>
		public const int BeamLossThreshold = 15;
	}
}
