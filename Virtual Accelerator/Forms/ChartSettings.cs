// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChartSettings.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.VirtualAccelerator.Forms
{
	using System;

	using DevExpress.XtraEditors;

	public partial class ChartSettings : XtraForm
	{
		public ChartSettings(int updateInterval)
		{
			this.InitializeComponent();
			this.updateIntervalSpinEdit.Value = updateInterval;
		}

		public new int ShowDialog()
		{
			base.ShowDialog();
			return (int)this.updateIntervalSpinEdit.Value;
		}

		private void OKButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}