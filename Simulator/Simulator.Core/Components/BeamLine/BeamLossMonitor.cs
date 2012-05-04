// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BeamLossMonitor.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Simulator.Core.Components.BeamLine
{
	using Epics.ChannelAccess.Provider;
	using Epics.Simulator.Core.Attributes;
	using Epics.Simulator.Core.Enums;
	using Epics.Simulator.Core.Models.Entities.Subatomic;
	using Epics.Simulator.Core.Models.Primitives;
	
	public class BeamLossMonitor : BeamLineComponent
	{
		/// <summary>
		/// Beam loss monitor. Issues a warning when a beam loss occurs at this location.
		/// </summary>
		public readonly IServerChannel<int> BeamLoss;

		/// <summary>
		/// Link to the previous beam loss monitor, to get the last beam current measurement.
		/// </summary>
		[Link(Link.SteeringMagnet, -1)] private IServerChannel<int> beamLoss = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="BeamLossMonitor"/> class.
		/// </summary>
		/// <param name="componentNo">Identifier number for the common component types.</param>
		/// <param name="location">Location of this component relative to the origin of the vector space.</param>
		public BeamLossMonitor(int componentNo, Location location)
			: base(componentNo, location)
		{
			this.BeamLoss = this.CreateChannel<int>(PredefinedChannels.BeamLossMonitor.BeamLoss);
		}

		protected override void InternalInteract(ElectronBeam electronBeam)
		{
			// If the current is lower than that of the previous blm measurement by a predefined limit, there is a beam loss
			// ToDo: Below is a temporary solution, rather use previous beamLoss measurement & Parameters.BeamLossThreshold
			if (electronBeam.ElectronDensity == 0)
			{
				this.BeamLoss.Value = 1;
			}
			else
			{
				this.BeamLoss.Value = 0;
			}
		}
	}
}