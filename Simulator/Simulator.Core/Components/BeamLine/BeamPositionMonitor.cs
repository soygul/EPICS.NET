// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BeamPositionMonitor.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Simulator.Core.Components.BeamLine
{
	using System;

	using Epics.ChannelAccess.Provider;
	using Epics.Simulator.Core.Attributes;
	using Epics.Simulator.Core.Enums;
	using Epics.Simulator.Core.Models.Entities.Subatomic;
	using Epics.Simulator.Core.Models.Primitives;

	/// <summary>
	/// This class uses SI units in all measurements.
	/// </summary>
	public class BeamPositionMonitor : BeamLineComponent
	{
		/// <summary>
		/// Electron beam current, in Amperes (1A = 1C/s).
		/// </summary>
		public readonly IServerChannel<double> Current;

		/// <summary>
		/// Horizontal position of the electron beam relative to the origin of the drift tube in Meters.
		/// </summary>
		public readonly IServerChannel<double> XPosition;

		/// <summary>
		/// Vertical position of the electron beam relative to the origin of the drift tube in Meters.
		/// </summary>
		public readonly IServerChannel<double> YPosition;

		[Link(Link.SteeringMagnet)] private IServerChannel<double> xCurrent = null;
		[Link(Link.SteeringMagnet)] private IServerChannel<int> xCurrentOn = null;
		[Link(Link.SteeringMagnet)] private IServerChannel<double> yCurrent = null;
		[Link(Link.SteeringMagnet)] private IServerChannel<int> yCurrentOn = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="BeamPositionMonitor"/> class.
		/// </summary>
		/// <param name="componentNo">Identifier number for the common component types.</param>
		/// <param name="location">Location of this component relative to the origin of the vector space.</param>
		public BeamPositionMonitor(int componentNo, Location location)
			: base(componentNo, location)
		{
			this.Current = this.CreateChannel<double>(PredefinedChannels.BeamPositionMonitor.Current);
			this.XPosition = this.CreateChannel<double>(PredefinedChannels.BeamPositionMonitor.XPosition);
			this.YPosition = this.CreateChannel<double>(PredefinedChannels.BeamPositionMonitor.YPosition);
		}

		/// <summary>
		/// Takes measurements on the electron beam.
		/// </summary>
		/// <param name="electronBeam">Electron beam going through the beam position monitor.</param>
		protected override void InternalInteract(ElectronBeam electronBeam)
		{
			// Check to see if the beams are getting dangerously close to the drift tube walls and if so, correct the beam path
			this.XPosition.Value = electronBeam.Location.X - this.Location.X;
			if (this.xCurrent != null)
			{
				if (this.XPosition.Value >= Parameters.DesiredBeamPositionRange)
				{
					// Directly putting the current on value to prevent I/O overhead for checking if it is already on (which EPICS does already)
					this.xCurrentOn.Value = 1;
					this.xCurrent.Value += this.XPosition.Value * -2;
				}
				else if (this.XPosition.Value <= -Parameters.DesiredBeamPositionRange)
				{
					this.xCurrentOn.Value = 1;
					this.xCurrent.Value += this.XPosition.Value * -2;
				}
			}

			this.YPosition.Value = electronBeam.Location.Y - this.Location.Y;
			if (this.yCurrent != null)
			{
				if (this.YPosition.Value >= Parameters.DesiredBeamPositionRange)
				{
					// Directly putting the current on value to prevent I/O overhead for checking if it is already on (which EPICS does already)
					this.yCurrentOn.Value = 1;
					this.yCurrent.Value += this.YPosition.Value * -2;
				}
				else if (this.YPosition.Value <= -Parameters.DesiredBeamPositionRange)
				{
					this.yCurrentOn.Value = 1;
					this.yCurrent.Value += this.YPosition.Value * -2;
				}
			}

			// Calculate beam loss and make adjustments. If the beam path overflows, electron density drops to zero
			if (this.XPosition.Value >= Parameters.DriftTubeRadius || this.XPosition.Value <= -Parameters.DriftTubeRadius
			    || this.YPosition.Value >= Parameters.DriftTubeRadius || this.YPosition.Value <= -Parameters.DriftTubeRadius)
			{
				electronBeam.ElectronDensity = 0;
			}

			// Measure electron beam current using I=nAvQ (http://en.wikipedia.org/wiki/Electric_current#Drift_speed)
			this.Current.Value = electronBeam.ElectronDensity * (Math.PI * Math.Pow(electronBeam.Radius, 2)) * electronBeam.Velocity.Dz * Electron.Charge;
		}
	}
}
