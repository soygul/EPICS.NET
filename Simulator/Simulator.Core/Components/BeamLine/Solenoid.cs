// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Solenoid.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Simulator.Core.Components.BeamLine
{
	using System;

	using Epics.ChannelAccess.Provider;
	using Epics.Simulator.Core.Models.Entities.Subatomic;
	using Epics.Simulator.Core.Models.Primitives;

	/// <summary>
	/// This class uses SI units in all measurements.
	/// </summary>
	public class Solenoid : BeamLineComponent
	{
		/// <summary>
		/// Determines if aperture magnet current is on or off.
		/// </summary>
		public readonly IServerChannel<int> CurrentOn;

		/// <summary>
		/// Determines the aperture magnet current, measured in amperes (A).
		/// </summary>
		public readonly IServerChannel<double> Current;

		public Solenoid(int componentNo, Location location)
			: base(componentNo, location)
		{
			this.CurrentOn = this.CreateChannel<int>(PredefinedChannels.Solenoid.CurrentOn);
			this.Current = this.CreateChannel<double>(PredefinedChannels.Solenoid.Current);
			
			// Default values
			this.Current.Value = 1500E-3; // 1500mA
		}

		protected override void InternalInteract(ElectronBeam electronBeam)
		{
			if (this.CurrentOn.Value == 1)
			{
				// Focus the beam (narrow down the beam radius while increasing the density thus keeping electron flow constant)
				// Using solenoid focal lenght formula: 1/f = q^2/8Tm . Integral(Bz^2, dz) ; T=longitudinal kinetic energy, Bz=Field strength
				// ToDo: Now simply assuming f=25cm with 400G magnetic field @300keV beam energy or else calculating I->G->Bz/T->f will take ages
				electronBeam.Divergence = -(Math.PI * this.Current.Value) / (4 * 2E4);
			}
		}
	}
}
