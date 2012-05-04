// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Vacuum.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.UserControls.Panels.ControlPanels.Parts
{
	using System;

	using Epics.ChannelAccess.Provider;

	public partial class Vacuum : ControlPanel
	{
		public Vacuum()
		{
			this.InitializeComponent();

			this.AddControlForSectorIdTextSettings(this.vAPressureLabel);
			
			this.AddControlForDataBinding(this.vAPressureValueLabel, PredefinedChannels.Vacuum.Pressure);
		}

		public override void InitializeCustomDataBindings()
		{
			Provider.Client.CreateChannel<int>(PredefinedChannels.Vacuum.On.SetId(this.SectorId)).Changed += on =>
			{
				this.vAPressureValueLabel.Appearance.ImageIndex = on;
			};
		}

		public override void InitializeDataBindingConverters()
		{
			// The format event fires when pulling data from a source into a control and the Parse event fires when pulling data from a control back into the datasource

			// Convert bar to nbar
			this.vAPressureValueLabel.DataBindings[0].Format += (s, e) => e.Value = Math.Round((double)e.Value * 1E9, 3).ToString();
		}
	}
}