// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainForm.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.VirtualAccelerator
{
	using System;
	using System.Drawing;
	using System.Windows.Forms;

	using DevExpress.LookAndFeel;
	using DevExpress.Skins;
	using DevExpress.Utils;
	using DevExpress.XtraBars;
	using DevExpress.XtraEditors;
	using DevExpress.XtraEditors.Controls;

	using Epics.ChannelAccess.Provider;
	using Epics.VirtualAccelerator.Forms;
	using Epics.VirtualAccelerator.Properties;

	public partial class MainForm : XtraForm
	{
		private readonly Color defaultButtonForeColor;
		private readonly Color defaultFormBackColor;
		private readonly Simulator.MainForm simulator = new Simulator.MainForm();

		private Point minimapMouseDownPosition;

		public MainForm()
		{
			// Initialize the channel access provider before anything else
			Provider.ProviderType = Settings.Default.ChannelAccessProvider;
			
			this.InitializeComponent();

			// Get default colors for the active skin (a precaution for future skin changes)
			Skin activeSkin = CommonSkins.GetSkin(this.defaultLookAndFeel.LookAndFeel);
			this.defaultFormBackColor = activeSkin["Form"].Color.GetBackColor();
			this.defaultButtonForeColor = activeSkin["Button"].Color.GetForeColor();

			// Disable the borders of the mainTabControl which is only possible via disabling the
			// skinning since skins have built in borders (as images) for some controls like TabControl.
			this.mainTabControl.LookAndFeel.Style = LookAndFeelStyle.Flat;
			this.mainTabControl.LookAndFeel.UseDefaultLookAndFeel = false;
			this.mainTabControl.BorderStyle = BorderStyles.NoBorder;
			this.mainTabControl.ShowTabHeader = DefaultBoolean.False;

			// BackColor of each tab page has to be set manually to match the current skin BackColor
			for (int i = 0; i < this.mainTabControl.TabPages.Count; i++)
			{
				this.mainTabControl.TabPages[i].BackColor = this.defaultFormBackColor;
			}

			// Sync the current selected ribbon page to the main tab control
			this.TopRibbonControl_SelectedPageChanged(this, EventArgs.Empty);

			// Make sure that release build main form gets correctly sized up
			this.Width = 1152;
			this.Height = 864;

			// Update component default values with user settings
			this.beamLineChart.UpdateInterval = Settings.Default.ChartsUpdateInterval;
		}

		private void ScrollLabelControl_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this.minimapMouseDownPosition = e.Location;
			}
		}

		private void ScrollLabelControl_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				// If: First check for left boundary overflow then for the right boundary overflow then do the scrolling
				// Else If: Then check to see whether the scrollbox overflows the left boundary. If so, reset the box location to X=0
				// Else If: Finally check to see whether the scrollbox overflows the right boundary. If so, reset the box location to X=Max
				if ((this.scrollLabelControl.Location.X + (e.X - this.minimapMouseDownPosition.X)) >= 0
					&& ((this.scrollLabelControl.Location.X + this.scrollLabelControl.Width) + (e.X - this.minimapMouseDownPosition.X)) <= this.minimapPopupControlContainer.Width)
				{
					this.scrollLabelControl.Left += e.X - this.minimapMouseDownPosition.X;
					this.beamLineTabPageScrollable.HorizontalScroll.Value = ((this.beamLineTabPageScrollable.HorizontalScroll.Maximum + 1) * this.scrollLabelControl.Left) /
																																	this.minimapPopupControlContainer.Width;
				}
				else if ((this.scrollLabelControl.Location.X + (e.X - this.minimapMouseDownPosition.X)) < 0)
				{
					this.scrollLabelControl.Left = 0;
					this.beamLineTabPageScrollable.HorizontalScroll.Value = 0;
				}
				else if (((this.scrollLabelControl.Location.X + this.scrollLabelControl.Width) + (e.X - this.minimapMouseDownPosition.X)) > this.minimapPopupControlContainer.Width)
				{
					this.scrollLabelControl.Left = this.minimapPopupControlContainer.Width - this.scrollLabelControl.Width;
					this.beamLineTabPageScrollable.HorizontalScroll.Value = this.beamLineTabPageScrollable.HorizontalScroll.Maximum;
				}

				// If: First check for top boundary overflow then for the bottom boundary overflow then do the scrolling
				// Else If: Then check to see whether the scrollbox overflows the top boundary. If so, reset the box location to Y=0
				// Else If: Finally check to see whether the scrollbox overflows the bottom boundary. If so, reset the box location to Y=Max
				if ((this.scrollLabelControl.Location.Y + (e.Y - this.minimapMouseDownPosition.Y)) >= 0
					&& ((this.scrollLabelControl.Location.Y + this.scrollLabelControl.Height) + (e.Y - this.minimapMouseDownPosition.Y)) <= this.minimapPopupControlContainer.Height)
				{
					this.scrollLabelControl.Top += e.Y - this.minimapMouseDownPosition.Y;
					this.beamLineTabPageScrollable.VerticalScroll.Value = ((this.beamLineTabPageScrollable.VerticalScroll.Maximum + 1) * this.scrollLabelControl.Top) /
																												this.minimapPopupControlContainer.Height;
				}
				else if ((this.scrollLabelControl.Location.Y + (e.Y - this.minimapMouseDownPosition.Y)) < 0)
				{
					this.scrollLabelControl.Top = 0;
					this.beamLineTabPageScrollable.VerticalScroll.Value = 0;
				}
				else if (((this.scrollLabelControl.Location.Y + this.scrollLabelControl.Height) + (e.Y - this.minimapMouseDownPosition.Y)) > this.minimapPopupControlContainer.Height)
				{
					this.scrollLabelControl.Top = this.minimapPopupControlContainer.Height - this.scrollLabelControl.Height;
					this.beamLineTabPageScrollable.VerticalScroll.Value = this.beamLineTabPageScrollable.VerticalScroll.Maximum;
				}
			}
		}

		private void TopRibbonControl_SelectedPageChanged(object sender, EventArgs e)
		{
			// Sync the current selected ribbon page to the main tab control
			this.mainTabControl.SelectedTabPageIndex = this.topRibbon.SelectedPage.PageIndex;

			// Select the minimap image to match the current tab page
			this.zoomBarButtonItem.Enabled = true;
			switch (this.mainTabControl.SelectedTabPage.Name)
			{
				case "beamLineTabPage":
					this.minimapPopupControlContainer.ContentImage = Resources.BeamLineMinimap;
					break;
				default:
					this.zoomBarButtonItem.Enabled = false;
					break;
			}
		}

		private void ZoomBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
		{
			// Sync the minimap scroll box to match the current tab page scroll size & location
			if (this.beamLineTabPageScrollable.HorizontalScroll.Visible)
			{
				this.scrollLabelControl.Left = (this.minimapPopupControlContainer.Width * this.beamLineTabPageScrollable.HorizontalScroll.Value) /
				                               (this.beamLineTabPageScrollable.HorizontalScroll.Maximum + 1);
				this.scrollLabelControl.Width = (this.minimapPopupControlContainer.Width * this.beamLineTabPageScrollable.Width) /
				                                (this.beamLineTabPageScrollable.HorizontalScroll.Maximum + 1);
			}
			else
			{
				this.scrollLabelControl.Left = 0;
				this.scrollLabelControl.Width = this.minimapPopupControlContainer.Width;
			}

			if (this.beamLineTabPageScrollable.VerticalScroll.Visible)
			{
				this.scrollLabelControl.Top = (this.minimapPopupControlContainer.Height * this.beamLineTabPageScrollable.VerticalScroll.Value) /
				                              (this.beamLineTabPageScrollable.VerticalScroll.Maximum + 1);
				this.scrollLabelControl.Height = (this.minimapPopupControlContainer.Height * this.beamLineTabPageScrollable.Height) /
				                                 (this.beamLineTabPageScrollable.VerticalScroll.Maximum + 1);
			}
			else
			{
				this.scrollLabelControl.Top = 0;
				this.scrollLabelControl.Height = this.minimapPopupControlContainer.Height;
			}
		}

		private void BLHLegendBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
		{
			new LegendForm().Show();
		}

		private void BLHChannelAccessBarButton_ItemClick(object sender, ItemClickEventArgs e)
		{
			using (var dialog = new ProviderSelector())
			{
				var result = dialog.Display();
				if (!string.IsNullOrEmpty(result) && Settings.Default.ChannelAccessProvider != result)
				{
					Settings.Default.ChannelAccessProvider = result;
					MessageBox.Show(
						"Changes were saved an will take affect after the application restart.",
						"Changes saved",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information);
				}
			}
		}

		private void BLHChannelsBarButton_ItemClick(object sender, ItemClickEventArgs e)
		{
			new ChannelsForm().Show();
		}

		private void BLSChartsBarButton_ItemClick(object sender, ItemClickEventArgs e)
		{
			using (var dialog = new ChartSettings(Settings.Default.ChartsUpdateInterval))
			{
				Settings.Default.ChartsUpdateInterval = dialog.ShowDialog();
				Settings.Default.Save();
				this.beamLineChart.UpdateInterval = Settings.Default.ChartsUpdateInterval;
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.notifyIcon.Dispose();
			this.simulator.Dispose();
		}

		private void SimulatorTimer_Tick(object sender, EventArgs e)
		{
			this.simulatorTimer.Stop();
			this.simulator.InitiateSimulator();
		}
	}
}