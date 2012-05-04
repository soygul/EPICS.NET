// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProviderSelector.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.ChannelAccess.Provider
{
	using System.Linq;
	using System.Windows.Forms;

	public partial class ProviderSelector : Form
	{
		private string providerType;

		public ProviderSelector()
		{
			this.InitializeComponent();
			this.providersComboBox.Items.AddRange(Provider.GetProviders().ToArray());

			if (this.providersComboBox.Items.Count != 0)
			{
				this.providersComboBox.SelectedIndex = 0;
			}
		}

		public string Display()
		{
			this.ShowDialog();
			return this.providerType;
		}

		private void SelectButton_Click(object sender, System.EventArgs e)
		{
			this.providerType = this.providersComboBox.SelectedItem.ToString();
			Provider.ProviderType = this.providerType;
			this.Close();
		}
	}
}
