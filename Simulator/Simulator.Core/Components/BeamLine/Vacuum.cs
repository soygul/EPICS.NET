// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Vacuum.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Simulator.Core.Components.BeamLine
{
	using Epics.ChannelAccess.Provider;
	using Epics.Simulator.Core.Models.Entities.Subatomic;
	using Epics.Simulator.Core.Models.Primitives;

	/// <summary>
	/// This class does not use SI unit for pressure (Pa) but uses Bar instead so be cautious.
	/// </summary>
	public class Vacuum : BeamLineComponent
	{
		/// <summary>
		/// Pressure measured at the location of this vacuum, in bars (bar). Average pressure is 1.3E-9 bar.
		/// </summary>
		public readonly IServerChannel<int> On;

		/// <summary>
		/// Pressure measured at the location of this vacuum, in bars (bar). Average pressure is 1.3E-9 bar.
		/// </summary>
		public readonly IServerChannel<double> Pressure;

		/// <summary>
		/// The desired minimum pressure for the drift tube. Measured in bars (bar).
		/// </summary>
		private const double DesiredPressure = 1.2E-9;

		/// <summary>
		/// The critical maximum pressure for normal operation the drift tube. Measured in bars (bar).
		/// </summary>
		private const double CriticalPressure = 2.9E-9;
		
		/// <summary>
		/// The gradient for how fast the pressure rises or falls. This depends on the vacuum type.
		/// </summary>
		private const double VacuumGradient = 0.1;

		/// <summary>
		/// This factor extends the pressure ranges so that the vacuum pump will have a chance to rest for a longer period of time.
		/// </summary>
		private const double VacuumRestFactor = 1.2;
		private double vacuumRest = 1;

		public Vacuum(int componentNo, Location location)
			: base(componentNo, location)
		{
			this.Pressure = this.CreateChannel<double>(PredefinedChannels.Vacuum.Pressure);
			this.On = this.CreateChannel<int>(PredefinedChannels.Vacuum.On);

			// Default channel value
			this.Pressure.Value = 6E-9;
			this.On.Value = 1;
		}

		protected override void InternalInteract(ElectronBeam electronBeam)
		{
			if (this.Pressure.Value <= DesiredPressure * this.vacuumRest)
			{
				this.On.Value = 0;
				this.vacuumRest = VacuumRestFactor;
				this.Pressure.Value += this.Pressure.Value * VacuumGradient * 0.1 * this.LastInteractionTimeDelta;
			}
			else if (this.Pressure.Value > CriticalPressure)
			{
				this.On.Value = 2;
				var pressure = this.Pressure.Value - (this.Pressure.Value * VacuumGradient * 2 * this.LastInteractionTimeDelta);
				this.Pressure.Value = pressure < DesiredPressure ? DesiredPressure : pressure;
			}
			else if (this.Pressure.Value > DesiredPressure * this.vacuumRest)
			{
				this.On.Value = 1;
				this.vacuumRest = 1;
				var pressure = this.Pressure.Value - (this.Pressure.Value * VacuumGradient * this.LastInteractionTimeDelta);
				this.Pressure.Value = pressure < DesiredPressure ? DesiredPressure : pressure;
			}
		}
	}
}