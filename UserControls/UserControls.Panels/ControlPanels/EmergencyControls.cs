// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmergencyControls.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.UserControls.Panels.ControlPanels
{
	using System.Drawing;

	using DevExpress.XtraEditors;

	using Epics.ChannelAccess.Provider;

	public partial class EmergencyControls : ControlPanel
	{
		public EmergencyControls()
		{
			this.InitializeComponent();

			this.AddControlForDataBinding(this.beamCheckButton, PredefinedChannels.Cathode.BeamOn);
			this.AddControlForDataBinding(this.hVCheckButton, PredefinedChannels.HighVoltageSupply.On);
		}

		public override void InitializeCustomDataBindings()
		{
			Provider.Client.CreateChannel<int>(PredefinedChannels.BeamLossMonitor.BeamLoss.SetId(5)).Changed += loss =>
			{
				switch (loss)
				{
					case 0:
						this.beamLossLabel.BackColor = Color.White;
						this.beamLossLabel.ForeColor = Color.LightGray;
						break;
					case 1:
						this.beamLossLabel.BackColor = Color.Red;
						this.beamLossLabel.ForeColor = Color.Black;
						break;
				}
			};
		}

		/// <summary>
		/// Makes sure that all the emergency controls are toggled at once if the latch action is selected.
		/// </summary>
		/// <param name="senderButton">Defines the sender button which called the method. Required to avoid an infinite loop.</param>
		/// <remarks>This function must be called by any button that belongs to the <see cref="EmergencyControls"/> container.</remarks>
		private void EmergencyLatchAction(CheckButton senderButton)
		{
			// ToDo: This needs a better fix (i.e. sensing if the click source is data binding or an actual click)
			var latch = senderButton.Focused && this.emergencyLatchCheckEdit.Checked;

			if (latch || senderButton == this.beamCheckButton)
			{
				this.beamCheckButton.Text = this.beamCheckButton.Checked ? "Beam On" : "Beam Off";
				this.beamCheckButton.Checked = senderButton.Checked;
			}

			if (latch || senderButton == this.hVCheckButton)
			{
				this.hVCheckButton.Text = this.hVCheckButton.Checked ? "HV On" : "HV Off";
				this.hVCheckButton.Checked = senderButton.Checked;
			}

			if (latch || senderButton == this.gateCheckButton)
			{
				this.gateCheckButton.Text = this.gateCheckButton.Checked ? "Open" : "Closed";
				this.gateCheckButton.Checked = senderButton.Checked;
			}

			if (latch || senderButton == this.pulseGeneratorCheckButton)
			{
				this.pulseGeneratorCheckButton.Text = this.pulseGeneratorCheckButton.Checked ? "PG On" : "PG Off";
				this.pulseGeneratorCheckButton.Checked = senderButton.Checked;
			}
		}

		private void BeamCheckButton_CheckedChanged(object sender, System.EventArgs e)
		{
			this.EmergencyLatchAction(this.beamCheckButton);
		}

		private void HVCheckButton_CheckedChanged(object sender, System.EventArgs e)
		{
			this.EmergencyLatchAction(this.hVCheckButton);
		}

		private void GateCheckButton_CheckedChanged(object sender, System.EventArgs e)
		{
			this.EmergencyLatchAction(this.gateCheckButton);
		}

		private void PulseGeneratorCheckButton_CheckedChanged(object sender, System.EventArgs e)
		{
			this.EmergencyLatchAction(this.pulseGeneratorCheckButton);
		}
	}
}