// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestApp.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Tests
{
	using System;
	using System.Windows.Forms;

	using Epics.ChannelAccess.Provider;

	public partial class TestApp : Form
	{
		public TestApp()
		{
			InitializeComponent();

			// Initialize the channel access provider before anything else
			Provider.ProviderType = "ChannelAccess.EPICS";
			var simulator = new Simulator.MainForm();
			simulator.InitiateSimulator();
		}

		private void CloseButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
