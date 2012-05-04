// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChannelsForm.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.Simulator
{
	using System;
	using System.Linq;
	using System.Windows.Forms;

	using Epics.ChannelAccess.Provider;

	public partial class ChannelsForm : Form
	{
		public ChannelsForm()
		{
			this.InitializeComponent();
			this.channelsTextBox.Text = Provider.Server.Channels
				.Aggregate(string.Empty, (current, channel) => current + (channel.Key + Environment.NewLine))
				.TrimEnd(Environment.NewLine.ToCharArray());
			this.channelsLabel.Text = Provider.Server.Channels.Count.ToString();
		}

		private void CloseButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
