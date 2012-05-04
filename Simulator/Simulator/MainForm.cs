// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainForm.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Simulator
{
	using System;
	using System.ComponentModel;
	using System.Linq;
	using System.Windows.Forms;

	using Epics.ChannelAccess.Provider;
	using Epics.Simulator.Core;
	using Epics.Simulator.Properties;

	public partial class MainForm : Form
	{
		private readonly SimulatorCore simulator = new SimulatorCore();
		private AsyncOperation asyncOperation;
		private bool simulatorInitiated;

		public MainForm()
		{
			this.InitializeComponent();

			if (Provider.ProviderType == null)
			{
				Provider.ProviderType = "ChannelAccess.EPICS";
			}
		}

		public void InitiateSimulator()
		{
			if (!this.simulatorInitiated)
			{
				this.simulatorInitiated = true;
				this.startStopButton.Text = "&Stop";
				this.startStopButton.Image = Resources.stop_16;

				// Initiate the simulator and register a callback to update the simulator time display (within the UI thread context)
				this.asyncOperation = AsyncOperationManager.CreateOperation(null);
				this.simulator.Initiate(
					time => this.asyncOperation.Post(
						state =>
							{
								if (this.IsHandleCreated)
								{
									this.simulatorTimeLabel.Text = Math.Round(time, 2).ToString() + " Seconds";
								}
							},
						null));
			}
		}

		public new void Dispose()
		{
			this.simulator.Terminate();
			this.notifyIcon.Dispose();
			base.Dispose();
		}


		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.Dispose();
		}

		private void StartStopButton_Click(object sender, EventArgs e)
		{
			if (!this.simulatorInitiated)
			{
				this.InitiateSimulator();
				this.simulatorInitiated = true;
				this.startStopButton.Text = "&Stop";
				this.startStopButton.Image = Resources.stop_16;
			}
			else
			{
				this.simulator.Terminate();
				this.simulatorInitiated = false;
				this.startStopButton.Text = "&Start";
				this.startStopButton.Image = Resources.play_16;
			}
		}

		private void ExitButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (this.WindowState == FormWindowState.Minimized)
			{
				this.WindowState = FormWindowState.Normal;
			}

			this.Show();
			this.Activate();
		}

		private void HideButton_Click(object sender, EventArgs e)
		{
			this.Hide();
		}

		private void ViewChannelsButton_Click(object sender, EventArgs e)
		{
			using (var dialog = new ChannelsForm())
			{
				dialog.ShowDialog();
			}
		}
	}
}