// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChannelAccess.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.UserControls.Panels
{
	using System.ComponentModel;
	using System.Drawing;

	[ToolboxBitmap(typeof(ToolboxBitmapBugFix), "Epics.UserControls.Panels.Resources.ChannelAccess.png")]
	[ToolboxItem(false)]
	public partial class ChannelAccess : Component
	{
		public ChannelAccess()
		{
			this.InitializeComponent();
		}

		public ChannelAccess(IContainer container)
		{
			container.Add(this);
			this.InitializeComponent();
		}
	}
}
