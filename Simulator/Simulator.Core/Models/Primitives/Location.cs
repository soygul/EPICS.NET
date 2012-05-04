// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Location.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Simulator.Core.Models.Primitives
{
	/// <summary>
	/// Models 3D location with separate vector components. This immutable structure uses SI units for all parameters.
	/// </summary>
	/// <remarks>See Resources\3D Cartesian Coordinate System.gif for the visualization of the coordinate system.</remarks>
	public struct Location
	{
		private readonly double x;
		private readonly double y;
		private readonly double z;

		public Location(double x, double y, double z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		/// <summary>
		/// Gets the X coordinate of the electron. Measured in meters (m).
		/// </summary>
		public double X
		{
			get
			{
				return this.x;
			}
		}

		/// <summary>
		/// Gets the Y coordinate of the electron. Measured in meters (m).
		/// </summary>
		public double Y
		{
			get
			{
				return this.y;
			}
		}

		/// <summary>
		/// Gets the Z coordinate of the electron. Measured in meters (m).
		/// </summary>
		public double Z
		{
			get
			{
				return this.z;
			}
		}
	}
}
