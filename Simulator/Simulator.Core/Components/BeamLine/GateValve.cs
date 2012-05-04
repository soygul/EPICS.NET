// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GateValve.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Simulator.Core.Components.BeamLine
{
	using Epics.ChannelAccess.Provider;
	using Epics.Simulator.Core.Models.Entities.Subatomic;
	using Epics.Simulator.Core.Models.Primitives;

	public class GateValve : BeamLineComponent
	{
		/// <summary>
		/// Determines if the gate valve is open or closed at the specified location. Gate valve allows or blocks traveling electron beams.
		/// </summary>
		public readonly IServerChannel<int> Closed;

		public GateValve(int componentNo, Location location)
			: base(componentNo, location)
		{
			this.Closed = this.CreateChannel<int>(PredefinedChannels.GateValve.Closed);
		}

		protected override void InternalInteract(ElectronBeam electronBeam)
		{
			if (this.Closed.Value == 1)
			{
				// Cut the beam if the valve is closed
				electronBeam.ElectronDensity = 0;
				//electronBeam.Velocity = new Velocity(0, 0, 0);
			}
		}
	}
}
