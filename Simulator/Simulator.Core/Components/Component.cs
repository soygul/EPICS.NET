// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Component.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Simulator.Core.Components
{
	using Epics.ChannelAccess.Provider;
	using Epics.Simulator.Core.Models.Primitives;

	public abstract class Component
	{
		/// <summary>
		/// Initializes a new instance of the Component class. This parameterized constructor provides common facilities for 
		/// all beam line components. Protected modifier ensures that this class cannot be instantiated directly with new keyword 
		/// but can only serve as base type in lists (i.e. List(Component) components != new List(Component);) for derived types. 
		/// There is no default constructor provided to prevent derived types omitting call to the base constructor.
		/// </summary>
		/// <param name="componentNo">Identifier number for the common component types.</param>
		/// <param name="location">Location of this component relative to the origin of the vector space.</param>
		protected Component(int componentNo, Location location)
		{
			this.ComponentNo = componentNo;
			this.Location = location;
		}

		/// <summary>
		/// Gets or sets the component number for the common component types (i.e. unique for BPMs).
		/// </summary>
		public int ComponentNo { get; set; }
		
		/// <summary>
		/// Gets or sets the location of this component with reference to electron source taken as 0 meters. Measured in meters (m).
		/// </summary>
		protected Location Location { get; set; }

		protected IServerChannel<T> CreateChannel<T>(PredefinedChannel predefinedChannel)
		{
			// Using the singleton instance of the channel access server
			return Provider.Server.CreateChannel<T>(predefinedChannel.SetId(this.ComponentNo));
		}
	}
}
