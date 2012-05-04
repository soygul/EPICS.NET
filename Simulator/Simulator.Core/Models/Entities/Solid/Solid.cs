// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Solid.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Simulator.Core.Models.Entities.Solid
{
	using Epics.Simulator.Core.Models.Primitives;

	internal abstract class Solid
	{
		protected Location Location { get; set; }

		protected Orientation Orientation { get; set; }
	}
}
