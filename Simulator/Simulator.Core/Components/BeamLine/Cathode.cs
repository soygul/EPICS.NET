// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Cathode.cs" company="Turkish Accelerator Center">
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
	public class Cathode : BeamLineComponent
	{
		/// <summary>
		/// Beam current on/off. This directly controls the beam on/off state thus named this was.
		/// </summary>
		public readonly IServerChannel<int> BeamOn;

		/// <summary>
		/// Cathode filament current. Controls the filament temperature.
		/// </summary>
		public readonly IServerChannel<double> CathodeCurrent;

		/// <summary>
		/// Cathode filament temperature. Minimum is 273 degrees Kelvin, maximum is 473. Desired range is 413-433 degrees Kelvin
		/// (or 140-160 degrees Celsius). This temperature is directly controlled by the cathode current and effects the number of
		/// electrons leaving the filament, right through the high voltage plates.
		/// </summary>
		public readonly IServerChannel<double> CathodeTemperature;

		/// <summary>
		/// Filament mass in kilograms (kg). Assuming 25W typical bulb.
		/// Reference: http://www.skk-banjaluckapivara.com/invent/flame_retarding_polyester_composition/mass_current_inrush_limiters.html
		/// </summary>
		private const double FilamentMass = 8.2E-6;

		/// <summary>
		/// Total resistivity (R) of filament in Ohms assuming that it is made of tungsten and its total resistivity is calculated
		/// as its total length was already known.
		/// </summary>
		private const double FilamentResistivity = 576;

		/// <summary>
		/// Specific heat capacity (c) of the filament in Joules per Kilogram Kelvin (J/kg.K) assuming that it is made of tungsten.
		/// </summary>
		private const double FilamentSpecificHeatCapacity = 130;

		/// <summary>
		/// Initializes a new instance of the <see cref="Cathode"/> class. 
		/// </summary>
		/// <param name="componentNo">Identifier number for the common component types.</param>
		/// <param name="location">Location of this component relative to the origin of the vector space.</param>
		public Cathode(int componentNo, Location location)
			: base(componentNo, location)
		{
			this.BeamOn = this.CreateChannel<int>(PredefinedChannels.Cathode.BeamOn);
			this.CathodeCurrent = this.CreateChannel<double>(PredefinedChannels.Cathode.Current);
			this.CathodeTemperature = this.CreateChannel<double>(PredefinedChannels.Cathode.Temperature);

			// Default channel values
			this.BeamOn.Value = 1;
			this.CathodeCurrent.Value = 700E-6; // 700uA
			this.CathodeTemperature.Value = Parameters.AmbientTemperature + 130;
		}

		protected override void InternalInteract(ElectronBeam electronBeam)
		{
			// Using Joule heating equation to calculate filament temperature: I2.R.t = m.c.Delta(T) => Delta(T) = I2.R.t / m.c
			var tempDelta = (Math.Pow(this.CathodeCurrent.Value, 2) * FilamentResistivity * this.LastInteractionTimeDelta) / (FilamentMass * FilamentSpecificHeatCapacity);
			var newTemp = this.CathodeTemperature.Value + tempDelta;

			// Some of the heat is radiated away each second, proportionally to the current filament temperature and the cooling constant
			// Formula for cooling is T(t) = TA + (To - TA)e^(-kt) ; T(t) = temp delta, TA = ambient temperature, To = object's temperature @ t=0, k = positive constant (should be around 0.00351 water - 0.00481 tube weld)
			this.CathodeTemperature.Value = Parameters.AmbientTemperature + ((newTemp - Parameters.AmbientTemperature) * Math.Pow(Math.E, -1 * 0.00205 * this.LastInteractionTimeDelta));
			
			if (this.BeamOn.Value == 0)
			{
				// Beam is cut-off
				electronBeam.Velocity = new Velocity(0, 0, 0);
			}
			else
			{
				// ToDo: Calculating thermionic emission density using Richardson/Dushman equation
				electronBeam.ElectronDensity = this.CathodeTemperature.Value * 45E11;

				// Same as the drift tube (in meters)
				electronBeam.Radius = Parameters.DriftTubeRadius;

				// ToDO: This should actually use tugnsten metal's work function to calculate leaving electrons' intial velocity
				electronBeam.Velocity = new Velocity(0, 0, this.CathodeTemperature.Value * 3);

				// ToDo: Set 100V barrier to 77ns interval & 500ps lenght (also set the 100V bias control)
				// Uses X = v.t formula to calculate the lenght in meters
				electronBeam.Length = electronBeam.Velocity.Dz * 77E-9;
			}
		}
	}
}
