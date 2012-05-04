// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlNode.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
/*
namespace Simulator.ControlNodes
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	using Epics.ChannelAccess.Provider;
	using Epics.Simulator.Controller.Bindings;
	using Epics.Simulator.Controller.Controllers;
	using Epics.Simulator.Controller.Enums;

	public class ControlNode
	{
		private readonly Epics.Simulator.Core.Simulator simulator = new Epics.Simulator.Core.Simulator();
		private readonly List<Controller> controllers = new List<Controller>();

		public ControlNode()
		{
			// Get all controller types
			var controllerTypes = Assembly.GetAssembly(typeof(Controller)).GetTypes()
				.Where(type => type.Namespace == typeof(Controller).Namespace && type != typeof(Controller));

			// Match the components with respective controllers
			foreach (var component in this.simulator.BeamLine)
			{
				// Get component type and initiate respective controller
				var copyComponent = component; // This is to avoid accessing modified closure in the below LINQ query
				var controller = (Controller)controllerTypes.Where(c => c.Name == copyComponent.GetType().Name).First()
					.GetConstructor(new[] { typeof(int) }).Invoke(new object[] { component.ComponentNo });

				// Resolve all controller links
				var controllerType = controller.GetType();

				// First resolve controller->component links for the same types (i.e. bpm to bpm)
				var methods = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
				foreach (var method in methods)
				{
					// Get methods with LinkAttribute
					var attributes = method.GetCustomAttributes(false);
					if (attributes.Length != 0 && attributes.First() is LinkAttribute)
					{
						// Bind the controller method to component delegate
						var property = component.GetType().GetProperty(method.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
						var action = Delegate.CreateDelegate(property.PropertyType, controller, method);
						property.SetValue(component, action, null);
					}
				}

				// Now resolve controller->component links for dissimilar types (i.e. bpm to steering magnet)
				var controllerFields = controllerType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
				foreach (var controllerField in controllerFields)
				{
					var attributes = controllerField.GetCustomAttributes(false);
					if (attributes.Length != 0 && attributes.First() is LinkAttribute)
					{
						var attribute = (LinkAttribute)attributes.First();
						if (attribute.Link != Link.Self && attribute.Link != Link.Global)
						{
							var matchingComponent = this.simulator.BeamLine
								.Where(c => c.GetType().Name == attribute.Link.ToString() && c.ComponentNo == controller.ComponentNo + 1).FirstOrDefault();

							// Match the link controller->matchingComponent
							if (matchingComponent != null)
							{
								var copyControllerField = controllerField; // LINQ modified closure access protection
								var matchingComponentField = 
									matchingComponent.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase)
									.Where(f => f.Name == copyControllerField.Name && f.FieldType == copyControllerField.FieldType).FirstOrDefault();

								if (matchingComponentField != null)
								{
									controllerField.SetValue(controller, matchingComponentField.GetValue(matchingComponent));
								}
							}
						}
						else if (attribute.Link == Link.Self)
						{
							// Now resolve controller auto-channels
							// ToDo: Resovel auto-channels (important!)
						}
						else if (attribute.Link == Link.Global)
						{
							// Now resolve global links
							// ToDo: Resovel global links
						}
					}
					else if (attributes.Length != 0 && attributes.First() is ChannelAttribute)
					{
						// Now resolve controller channels
						// ToDo: Resovel channels (important!)
					}
				}

				// Finally store the controller in the list for later access)
				this.controllers.Add(controller);
			}
		}

		protected IServerChannel<T> CreateChannel<T>(PredefinedChannel predefinedChannel, int componentNo)
		{
			// Using the singleton instance of the channel access server
			return Provider.Server.CreateChannel<T>(predefinedChannel.SetId(componentNo));
		}
	}
}
*/