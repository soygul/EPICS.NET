// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Electron.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Simulator.Core.Models.Entities.Subatomic
{
	using Epics.Simulator.Core.Models.Primitives;

	/// <summary>
	/// Models a single electron. This class uses SI units for all parameters.
	/// </summary>
	public class Electron
	{
		/// <summary>
		/// Gets the charge of an electron in Coulombs (C).
		/// </summary>
		public const double Charge = 1.602176487E-19;

		/// <summary>
		/// Electron mass in kilograms (kg).
		/// </summary>
		public const double Mass = 9.10938215E-31;

		/// <summary>
		/// Electron radius in meters (m).
		/// </summary>
		public const double Radius = 2.8179402894E-15;

		public Velocity Velocity { get; set; }

		public Location Location { get; set; }
	}
}
