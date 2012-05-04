// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Aperture.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Simulator.Core.Components.BeamLine
{
	using Epics.ChannelAccess.Provider;
	using Epics.Simulator.Core.Models.Entities.Subatomic;
	using Epics.Simulator.Core.Models.Primitives;

	/// <summary>
	/// This class uses SI units in all measurements.
	/// </summary>
	public class Aperture : BeamLineComponent
	{
		/// <summary>
		/// Determines if the aperture disc/arm is inside the beam pipe.
		/// </summary>
		public readonly IServerChannel<int> In;

		/// <summary>
		/// Determines the radius of the aperture diaphragm. Measured in meters (m).
		/// </summary>
		public readonly IServerChannel<double> Radius;

		public Aperture(int componentNo, Location location)
			: base(componentNo, location)
		{
			this.In = this.CreateChannel<int>(PredefinedChannels.Aperture.In);
			this.Radius = this.CreateChannel<double>(PredefinedChannels.Aperture.Radius);

			// Default values
			this.Radius.Value = 20E-3; // 20mm
		}

		protected override void InternalInteract(ElectronBeam electronBeam)
		{
			if (this.In.Value == 1)
			{
				// Aperture cuts the beam radius down to the aperture size
				electronBeam.Radius = this.Radius.Value;
			}
		}
	}
}
