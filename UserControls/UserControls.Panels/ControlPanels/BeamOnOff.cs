// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BeamOnOff.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.UserControls.Panels.ControlPanels
{
	using System;
	using System.Drawing;

	using DevExpress.LookAndFeel;

	using Epics.ChannelAccess.Provider;

	public partial class BeamOnOff : ControlPanel
	{
		public BeamOnOff()
		{
			this.InitializeComponent();

			this.AddControlForDataBinding(this.beamOnOffCheckButton, PredefinedChannels.Cathode.BeamOn);
		}

		private void BeamOnOffCheckButton_CheckedChanged(object sender, EventArgs e)
		{
			if (this.beamOnOffCheckButton.Checked)
			{
				this.beamOnOffCheckButton.Text = "Beam On";
				this.beamOnOffCheckButton.ForeColor = Color.MediumSeaGreen;
				this.beamOnOffCheckButton.Font = new Font(this.beamOnOffCheckButton.Font, FontStyle.Bold);
				this.beamOnPanelControl.BackColor = Color.Transparent;
				this.beamOnPanelControl.LookAndFeel.UseDefaultLookAndFeel = false;
				this.beamOnPanelControl.LookAndFeel.Style = LookAndFeelStyle.Office2003;
			}
			else
			{
				this.beamOnOffCheckButton.Text = "Beam Off";
				this.beamOnOffCheckButton.Font = new Font(this.beamOnOffCheckButton.Font, FontStyle.Regular);
				this.beamOnOffCheckButton.ForeColor = Color.FromArgb(50, 50, 50);
				this.beamOnOffCheckButton.LookAndFeel.Reset();
				this.beamOnPanelControl.LookAndFeel.Reset();
			}
		}
	}
}
