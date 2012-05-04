// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElectronBeam.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Simulator.Core.Models.Entities.Subatomic
{
	/// <summary>
	/// Simulates an electron bunch with all the significant parameters. All the parameters are in SI or SI derived units.
	/// </summary>
	public class ElectronBeam : Electron
	{
		/// <summary>
		/// Gets or sets the number of electrons per unit volume (or charge carrier density), measured in particles per m3 (e/m3).
		/// </summary>
		public double ElectronDensity { get; set; }

		/// <summary>
		/// Gets or sets the cross sectional radius (r) (not the diameter!) of the electron beam, measured in meters (m).
		/// </summary>
		public new double Radius { get; set; }

		/// <summary>
		/// Gets or sets the length of the electron beam (bunch), measured in meters (m). Electron beam length is positive infinity
		/// if this is a continuous beam.
		/// </summary>
		public double Length { get; set; }

		/// <summary>
		/// Gets or sets the beam divergence, mostly effected by lenses/solenoids. This divergence may be outer or inner with respect to
		/// the beam center. Measured in radians (rad) which gives the angle of inclination for the beam.
		/// </summary>
		public double Divergence { get; set; }
	}
}
