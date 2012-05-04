// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BeamLineComponent.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Simulator.Core.Components.BeamLine
{
	using System;
	using System.Threading;

	using Epics.Simulator.Core.Models.Entities.Subatomic;
	using Epics.Simulator.Core.Models.Primitives;

	/// <summary>
	/// This class uses SI units in all measurements.
	/// </summary>
	public abstract class BeamLineComponent : Component
	{
		/// <summary>
		/// Gets the last interaction time of this component with the electron beam. This is useful for time specific calculations
		/// for components (i.e. heath dissipated since the last interaction etc.).
		/// </summary>
		private double lastInteractionTime;

		/// <summary>
		/// Initializes a new instance of the BeamLineComponent class. This parameterized constructor provides common facilities for 
		/// all beam line components. Protected modifier ensures that this class cannot be instantiated directly with new keyword 
		/// but can only serve as base type in lists (i.e. List(Component) components != new List(Component);) for derived types. 
		/// There is no default constructor provided to prevent derived types omitting call to the base constructor.
		/// </summary>
		/// <param name="componentNo">Identifier number for the common component types.</param>
		/// <param name="location">Location of this component relative to the origin of the vector space.</param>
		protected BeamLineComponent(int componentNo, Location location)
			: base(componentNo, location)
		{
		}

		/// <summary>
		/// Gets or sets the total elapsed time since the simulator started (or to be exact, since the first time any component
		/// in the simulator did interact with the electron beam). This property is shared between all components.
		/// </summary>
		public static double SimulatorTime { get; set; }
		
		/// <summary>
		/// Gets the time elapsed since the last time that this component interacted with the electron beam.
		/// This is useful for time specific calculations for components (i.e. heath dissipated since the last interaction etc.).
		/// </summary>
		protected double LastInteractionTimeDelta { get; private set; }
		
		/// <summary>
		/// Initiates electron beam - components interactions where the electron beam may come out with a simple measurement
		/// or may be affected by the component in one way or the other.
		/// </summary>
		/// <param name="electronBeam">Electron beam going through the beam line components.</param>
		public void Interact(ElectronBeam electronBeam)
		{
			// Calculate the time delta (seconds) from forward movement (Z) distance and speed
			// This measures the time elapsed during the electron beam's travel from one component to the next
			var locationDelta = this.Location.Z - electronBeam.Location.Z;
			var timeDelta = locationDelta / electronBeam.Velocity.Dz;
			if (!double.IsNaN(timeDelta))
			{
				SimulatorTime += timeDelta;
			}
			
			this.LastInteractionTimeDelta = SimulatorTime - this.lastInteractionTime;

			/* 1 tick is 100 nanoseconds
			Stopwatch _stopwatch = Stopwatch.StartNew();
			DateTime highresDT = _starttime.AddTicks(_stopwatch.Elapsed.Ticks);
			Synchronization & last interaction time
			*/

			// Beam velocity and density fades away as the beam gets farther away from the source (0.05% per meter of distance)
			// ToDo: electronBeam.Velocity = new Velocity(electronBeam.Velocity.Dx, electronBeam.Velocity.Dy, electronBeam.Velocity.Dz - ??);
			electronBeam.ElectronDensity = electronBeam.ElectronDensity * (1 - (locationDelta / 2000));

			// Update beam radius with respect to beam divergence while keeping electron flow constant
			if (electronBeam.Divergence != 0)
			{
				var oldRadius = electronBeam.Radius;
				electronBeam.Radius += locationDelta * Math.Sin(electronBeam.Divergence);
				electronBeam.ElectronDensity = electronBeam.ElectronDensity * Math.Pow(oldRadius, 2) / Math.Pow(electronBeam.Radius, 2);
			}

			// Update beam location))
			if (!double.IsNaN(timeDelta))
			{
				electronBeam.Location = new Location(
					electronBeam.Location.X + (electronBeam.Velocity.Dx * timeDelta),
					electronBeam.Location.Y + (electronBeam.Velocity.Dy * timeDelta),
					this.Location.Z);
			}
			else
			{
				electronBeam.Location = new Location(electronBeam.Location.X, electronBeam.Location.Y, this.Location.Z);
			}

			// Component specific interactions
			this.InternalInteract(electronBeam);

			// Update last interaction time
			this.lastInteractionTime = SimulatorTime;
		}

		protected abstract void InternalInteract(ElectronBeam electronBeam);
	}
}
