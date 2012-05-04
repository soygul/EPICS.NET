// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Orientation.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Simulator.Core.Models.Primitives
{
	using System;

	/// <summary>
	/// Models 3D orientation with separate rotation angles. This immutable structure uses SI units for all parameters.
	/// All rotations happen on surfaces defined at Resources\3D Cartesian Coordinate Surfaces.png.
	/// </summary>
	/// <remarks>See Resources\3D Orientation.png for the measurement of the orientation angles.
	/// Also note that rotation on a surface means rotation around the perpendicular axis. So if you are in doubt about the
	/// positive direction, use the right hand rule on the perpendicularly penetrating axis (i.e. z-axis for x-y surface).</remarks>
	internal struct Orientation
	{
		private readonly double xy;

		private readonly double yz;

		private readonly double zx;

		/// <summary>
		/// Initializes a new instance of the Orientation struct. All parameters are in radians (rad).
		/// </summary>
		/// <param name="yz">Orientation or the solid with respect to y-axis. Measured in radians (rad). Should be 
		/// in the -PI/2 { T { PI/2 range.</param>
		/// <param name="xy">Orientation or the solid with respect to x-axis. Measured in radians (rad). Should be 
		/// in the -PI/2 { T { PI/2 range.</param>
		/// <param name="zx">Orientation or the solid with respect to z-axis. Measured in radians (rad). Should be 
		/// in the -PI/2 { T { PI/2 range.</param>
		/// <exception cref="ArgumentOutOfRangeException"><c>tx, ty, or tz</c> is out of range.</exception>
		internal Orientation(double yz, double xy, double zx)
		{
			if (Math.Abs(yz) <= Math.PI / 2 || Math.Abs(xy) <= Math.PI / 2 || Math.Abs(zx) <= Math.PI / 2)
			{
				this.yz = yz;
				this.xy = xy;
				this.zx = zx;
			}
			else
			{
				throw new ArgumentOutOfRangeException("Angle (rad)", "All the arguments should be in the -PI/2 < T < PI/2 range.");
			}

		}

		/// <summary>
		/// Gets the rotation angle on the x-y surface. Rotation angle is measured with respect to x-axis. Measured in radians (rad).
		/// </summary>
		/// <remarks>Currently this property is limited to -PI/2 {= T {= PI/2.</remarks>
		internal double XY
		{
			get
			{
				return this.xy;
			}
		}

		/// <summary>
		/// Gets the rotation angle on the y-z surface. Rotation angle is measured with respect to y-axis. Measured in radians (rad).
		/// </summary>
		/// <remarks>Currently this property is limited to -PI/2 {= T {= PI/2.</remarks>
		internal double YZ
		{
			get
			{
				return this.yz;
			}
		}

		/// <summary>
		/// Gets the rotation angle on the z-x surface. Rotation angle is measured with respect to z-axis. Measured in radians (rad).
		/// </summary>
		/// <remarks>Currently this property is limited to -PI/2 {= T {= PI/2.</remarks>
		internal double ZX
		{
			get
			{
				return this.zx;
			}
		}
	}
}
