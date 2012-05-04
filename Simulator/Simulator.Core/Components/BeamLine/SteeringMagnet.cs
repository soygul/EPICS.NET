// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SteeringMagnet.cs" company="Turkish Accelerator Center">
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
	public class SteeringMagnet : BeamLineComponent
	{
		/// <summary>
		/// X-axis current, in Amperes (A).
		/// </summary>
		public readonly IServerChannel<double> XCurrent;

		/// <summary>
		/// X-axis current on/off state.
		/// </summary>
		public readonly IServerChannel<int> XCurrentOn;

		/// <summary>
		///  Y-axis current, in Amperes (A).
		/// </summary>
		public readonly IServerChannel<double> YCurrent;

		/// <summary>
		/// Y-axis current on/off state.
		/// </summary>
		public readonly IServerChannel<int> YCurrentOn;

		/// <summary>
		/// Initializes a new instance of the <see cref="SteeringMagnet"/> class. 
		/// </summary>
		/// <param name="componentNo">Identifier number for the common component types.</param>
		/// <param name="location">Location of this component relative to the origin of the vector space.</param>
		public SteeringMagnet(int componentNo, Location location)
			: base(componentNo, location)
		{
			this.XCurrentOn = this.CreateChannel<int>(PredefinedChannels.SteeringMagnet.XCurrentOn);
			this.XCurrent = this.CreateChannel<double>(PredefinedChannels.SteeringMagnet.XCurrent);
			this.YCurrentOn = this.CreateChannel<int>(PredefinedChannels.SteeringMagnet.YCurrentOn);
			this.YCurrent = this.CreateChannel<double>(PredefinedChannels.SteeringMagnet.YCurrent);

			// Default channel values
			this.XCurrentOn.Value = 1;
			this.XCurrent.Value = 0.5;
			this.YCurrentOn.Value = 1;
			this.YCurrent.Value = -0.5;
		}

		protected override void InternalInteract(ElectronBeam electronBeam)
		{
			if (this.XCurrentOn.Value == 1)
			{
				// Steering magnet has no effect on drift velocity
				electronBeam.Velocity = new Velocity(electronBeam.Velocity.Dx + (this.XCurrent.Value * 5), electronBeam.Velocity.Dy, electronBeam.Velocity.Dz);
			}

			if (this.YCurrentOn.Value == 1)
			{
				electronBeam.Velocity = new Velocity(electronBeam.Velocity.Dx, electronBeam.Velocity.Dy + (this.YCurrent.Value * 5), electronBeam.Velocity.Dz);
			}
		}
	}
}
