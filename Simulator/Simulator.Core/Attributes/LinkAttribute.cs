// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkAttribute.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Simulator.Core.Attributes
{
	using System;

	using Epics.Simulator.Core.Components;
	using Epics.Simulator.Core.Enums;

	public class LinkAttribute : Attribute
	{
		public LinkAttribute()
		{
			this.Link = Link.Self;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LinkAttribute"/> class and sets a link to the specified
		/// component with same <see cref="Component.ComponentNo"/>.
		/// </summary>
		/// <param name="link">Link to a specific component.</param>
		public LinkAttribute(Link link)
		{
			this.Link = link;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LinkAttribute"/> class and sets a link to the specified
		/// component with same <see cref="Component.ComponentNo"/>.
		/// </summary>
		/// <param name="link">Link to a specific component.</param>
		/// <param name="linkOffset">The target link component number offset. Default behavior is to look for the same component
		/// no in the target but this behavior can be modified with an offset value like -1, +2 etc.</param>
		public LinkAttribute(Link link, int linkOffset)
		{
			this.Link = link;
			this.LinkOffset = linkOffset;
		}

		public Link Link { get; set; }

		/// <summary>
		/// Gets or sets the target link component number offset. Default behavior is to look for the same component no in the target
		/// but this behavior can be modified with an offset value like -1, +2 etc.
		/// </summary>
		public int LinkOffset { get; set; }
	}
}
