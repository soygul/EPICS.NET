// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HighVoltageSupply.cs" company="Turkish Accelerator Center">
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
	public class HighVoltageSupply : BeamLineComponent
	{
		/// <summary>
		/// -300kV high voltage on/off.
		/// </summary>
		public readonly IServerChannel<int> HighVoltageSupplyOn;

		/// <summary>
		/// Preset (desired) high voltage in the range of 0 to -350kV, measured in Volts (V).
		/// </summary>
		public readonly IServerChannel<double> HighVoltageSupplyPreset;
		
		/// <summary>
		/// Measured (actual) voltage in the range of 0 to -350kV, measured in Volts (V). Measured voltage steadily rises or
		/// fall getting closer to the preset voltage with respect to the preset gradient. 
		/// </summary>
		public readonly IServerChannel<double> HighVoltageSupplyMeasured;

		/// <summary>
		/// Slope (m) of the measured/preset voltage curve. This is a unit-less identity and determines the multiplication factor that
		/// the measured voltage rises per second.
		/// </summary>
		private const double HighVoltageGradient = 0.1;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="HighVoltageSupply"/> class.
		/// </summary>
		/// <param name="componentNo">Identifier number for the common component types.</param>
		/// <param name="location">Location of this component relative to the origin of the vector space.</param>
		public HighVoltageSupply(int componentNo, Location location)
			: base(componentNo, location)
		{
			this.HighVoltageSupplyOn = this.CreateChannel<int>(PredefinedChannels.HighVoltageSupply.On);
			this.HighVoltageSupplyPreset = this.CreateChannel<double>(PredefinedChannels.HighVoltageSupply.PresetVoltage);
			this.HighVoltageSupplyMeasured = this.CreateChannel<double>(PredefinedChannels.HighVoltageSupply.MeasuredVoltage);

			// Default values
			this.HighVoltageSupplyOn.Value = 1;
			this.HighVoltageSupplyPreset.Value = -250E3;
			this.HighVoltageSupplyMeasured.Value = -50E3;
		}

		protected override void InternalInteract(ElectronBeam electronBeam)
		{
			if (this.HighVoltageSupplyOn.Value == 0)
			{
				this.HighVoltageSupplyMeasured.Value = 0;
				return;
			}

			// Increase/decrease measured high voltage with respect to time delta and gradient
			if (this.HighVoltageSupplyPreset.Value < this.HighVoltageSupplyMeasured.Value)
			{
				// Up gradient
				var voltage = this.HighVoltageSupplyMeasured.Value + (this.HighVoltageSupplyPreset.Value * HighVoltageGradient * this.LastInteractionTimeDelta);
				this.HighVoltageSupplyMeasured.Value = voltage <= this.HighVoltageSupplyPreset.Value ? this.HighVoltageSupplyPreset.Value : voltage;
			}
			else if (this.HighVoltageSupplyPreset.Value > this.HighVoltageSupplyMeasured.Value)
			{
				// Down gradient
				var voltage = this.HighVoltageSupplyMeasured.Value - ((-350E3 - this.HighVoltageSupplyPreset.Value) * HighVoltageGradient * this.LastInteractionTimeDelta);
				this.HighVoltageSupplyMeasured.Value = voltage >= this.HighVoltageSupplyPreset.Value ? this.HighVoltageSupplyPreset.Value : voltage;
			}

			// Calculating the new velocity of the accelerated electron beam using F=qE=mA & E=Volt/d & V^2=Vo^2+2Ax => V=Sqrt(2.q.Volt/m + Vo^2)
			var velocity = Math.Round(Math.Sqrt(Math.Pow(electronBeam.Velocity.Dz, 2) + (2 * Electron.Charge * -this.HighVoltageSupplyMeasured.Value / Electron.Mass)));
			
			// Electron beam density decreases as much as the beam speed increases thus keeping beam current constant
			// ToDo: Electron speed exceeds the speed of light in case of V = -350kV so something is wrong
			// electronBeam.ElectronDensity = Math.Round(electronBeam.ElectronDensity * (electronBeam.Velocity.Dz / velocity));
			// electronBeam.Velocity = new Velocity(0, 0, velocity);
		}
	}
}
