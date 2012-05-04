// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Dimensions.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Simulator.Core.Models.Primitives
{
	/// <summary>
	/// Models 3D dimensions of a solid with separate vector components. This immutable structure uses SI units for all parameters.
	/// </summary>
	/// <remarks>See Resources\3D Cartesian Coordinate Surfaces.png for the visualization of the coordinate surfaces for solids.</remarks>
	internal struct Dimensions
	{
		private readonly double height;

		private readonly double lenght;

		private readonly double width;

		internal Dimensions(double lenght, double width, double height)
		{
			this.lenght = lenght;
			this.width = width;
			this.height = height;
		}

		/// <summary>
		/// Gets the lenght of the solid with respect to z-axis. Measured in meters (m).
		/// </summary>
		internal double Height
		{
			get
			{
				return this.height;
			}
		}

		/// <summary>
		/// Gets the lenght of the solid with respect to y-axis. Measured in meters (m).
		/// </summary>
		internal double Lenght
		{
			get
			{
				return this.lenght;
			}
		}

		/// <summary>
		/// Gets the lenght of the solid with respect to x-axis. Measured in meters (m).
		/// </summary>
		internal double Width
		{
			get
			{
				return this.width;
			}
		}
	}
}
