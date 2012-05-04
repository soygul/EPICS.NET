// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Velocity.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Simulator.Core.Models.Primitives
{
	/// <summary>
	/// Models the velocity of a rigid body with separate vector components. This structure uses SI units for all parameters.
	/// </summary>
	/// <remarks>See Resources\3D Cartesian Coordinate System.gif for the visualization of the coordinate system.</remarks>
	public struct Velocity
	{
		private readonly double dx;
		private readonly double dy;
		private readonly double dz;

		public Velocity(double dx, double dy, double dz)
		{
			this.dx = dx;
			this.dy = dy;
			this.dz = dz;
		}

		/// <summary>
		/// Gets the X components of the velocity vector. Measured in meters per second (m/s).
		/// </summary>
		public double Dx
		{
			get
			{
				return this.dx;
			}
		}

		/// <summary>
		/// Gets the Y components of the velocity vector. Measured in meters per second (m/s).
		/// </summary>
		public double Dy
		{
			get
			{
				return this.dy;
			}
		}

		/// <summary>
		/// Gets the Z components of the velocity vector. Measured in meters per second (m/s).
		/// </summary>
		public double Dz
		{
			get
			{
				return this.dz;
			}
		}
	}
}
