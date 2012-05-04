// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimulatorCore.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Simulator.Core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Threading;
	using System.Threading.Tasks;

	using Epics.Simulator.Core.Attributes;
	using Epics.Simulator.Core.Components;
	using Epics.Simulator.Core.Components.BeamLine;
	using Epics.Simulator.Core.Models.Entities.Subatomic;
	using Epics.Simulator.Core.Models.Primitives;

	public class SimulatorCore
	{
		private bool simulatorTerminated;

		public List<BeamLineComponent> BeamLine { get; private set; }

		public void Initiate(Action<double> simulatorTime)
		{
			this.simulatorTerminated = false;

			// Long running task with task continuation at the main thread
			Task.Factory.StartNew(
				() => this.InternalInitiate(simulatorTime), TaskCreationOptions.LongRunning).ContinueWith(
					task =>
						{
							if (task.IsFaulted)
							{
								// ToDo: Add logging here
								throw task.Exception.InnerException;
							}
					},
				TaskScheduler.FromCurrentSynchronizationContext());
		}

		public void Terminate()
		{
			var synchronizationToken = new object();
			lock (synchronizationToken)
			{
				this.simulatorTerminated = true;
			}
		}

		private void InternalInitiate(Action<double> simulatorTime)
		{
			// Optional 6 seconds of waittime (for proper initialization)
			// Thread.Sleep(6000);

			// Create the beam line and fill it with components
			this.BeamLine = new List<BeamLineComponent>
				{
					// ToDo: Appoint component numbers automatically via reflection
					new Cathode(1, new Location(0, 0, 0)),
					new HighVoltageSupply(1, new Location(0, 0, 10)),
					new Vacuum(1, new Location(0, 0, 15)),
					new SteeringMagnet(1, new Location(0, 0, 20)),
					new BeamPositionMonitor(1, new Location(0, 0, 30)),
					new BeamLossMonitor(1, new Location(0, 0, 31)),
					new Aperture(1, new Location(0, 0, 31)),
					new BeamProfileMonitor(1, new Location(0, 0, 32)),
					new GateValve(1, new Location(0, 0, 33)),
					new Solenoid(1, new Location(0, 0, 38)),
					new SteeringMagnet(2, new Location(0, 0, 40)),
					new BeamPositionMonitor(2, new Location(0, 0, 50)),
					new BeamLossMonitor(2, new Location(0, 0, 51)),
					new Aperture(2, new Location(0, 0, 51)),
					new BeamProfileMonitor(2, new Location(0, 0, 52)),
					new GateValve(2, new Location(0, 0, 53)),
					new Vacuum(2, new Location(0, 0, 55)),
					new Solenoid(2, new Location(0, 0, 58)),
					new SteeringMagnet(3, new Location(0, 0, 60)),
					new BeamPositionMonitor(3, new Location(0, 0, 70)),
					new BeamLossMonitor(3, new Location(0, 0, 71)),
					new Aperture(3, new Location(0, 0, 71)),
					new BeamProfileMonitor(3, new Location(0, 0, 72)),
					new GateValve(3, new Location(0, 0, 73)),
					new Vacuum(3, new Location(0, 0, 75)),
					new Solenoid(3, new Location(0, 0, 78)),
					new SteeringMagnet(4, new Location(0, 0, 80)),
					new BeamPositionMonitor(4, new Location(0, 0, 90)),
					new BeamLossMonitor(4, new Location(0, 0, 931)),
					new Aperture(4, new Location(0, 0, 91)),
					new BeamProfileMonitor(4, new Location(0, 0, 92)),
					new GateValve(4, new Location(0, 0, 93)),
					new Solenoid(4, new Location(0, 0, 98)),
					new SteeringMagnet(5, new Location(0, 0, 100)),
					new BeamPositionMonitor(5, new Location(0, 0, 110)),
					new BeamLossMonitor(5, new Location(0, 0, 110)),
					new BeamProfileMonitor(5, new Location(0, 0, 112)),
					new Solenoid(5, new Location(0, 0, 118))
				};

			this.ResolveLinks(this.BeamLine);

			// ToDo: Timer is better suited for this job! var timer = new Timer(o => {}, null, 0, 0);

			// Start sending electron beams through the beam line
			var componentCount = this.BeamLine.Count;
			while (true)
			{
				// If the simulator is terminated, break out of the loop
				if (this.simulatorTerminated)
				{
					BeamLineComponent.SimulatorTime = 0;
					break;
				}

				// Initially the electron beam is a null beam which gets shaped by the initial component on the beamline: the electron gun
				var electronBeam = new ElectronBeam();
				
				foreach (var beamLineComponent in this.BeamLine)
				{
					beamLineComponent.Interact(electronBeam);
				}

				// Update simulator time display after every beamline transversal
				simulatorTime(BeamLineComponent.SimulatorTime);
				Thread.Sleep(80);
			}
		}

		private void ResolveLinks(IEnumerable<Component> components)
		{
			foreach (var component in components)
			{
				// LINQ modified closure protection
				var sourceComponent = component;

				// Get component fields
				foreach (var componentField in sourceComponent.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase))
				{
					// LINQ modified closure protection
					var sourceComponentField = componentField;

					// Get fields with LinkAttribute
					var sourceFieldAttributes = sourceComponentField.GetCustomAttributes(typeof(LinkAttribute), false);
					if (sourceFieldAttributes.Length != 0)
					{
						// Get target component which the link points to (default is to look for the same component no in the target)
						var targetComponent =
							components.Where(
								c => c.GetType().Name == ((LinkAttribute)sourceFieldAttributes[0]).Link.ToString()
								&& c.ComponentNo == sourceComponent.ComponentNo + ((LinkAttribute)sourceFieldAttributes[0]).LinkOffset)
								.FirstOrDefault();

						if (targetComponent != null)
						{
							// Get the target field of the target component which the link points to
							// ToDo: targetComponentField search is not that accurate (search public fields and get the name match)
							var targetComponentField = targetComponent.GetType()
								.GetField(sourceComponentField.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

							if (targetComponentField != null)
							{
								// Make link: sourceComponentField->targetComponentField
								sourceComponentField.SetValue(sourceComponent, targetComponentField.GetValue(targetComponent));
							}
						}
					}
				}
			}
		}
	}
}
