// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Cylinder.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Simulator.Core.Models.Entities.Solid
{
	internal class Cylinder : Solid
	{
		internal Cylinder(double radius, double lenght)
		{
			this.Radius = radius;
			this.Length = lenght;
		}

		internal double Length { get; set; }

		internal double Radius { get; set; }
	}
}
