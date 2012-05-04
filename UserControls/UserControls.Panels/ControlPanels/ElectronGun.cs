// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElectronGun.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.UserControls.Panels.ControlPanels
{
	using System;

	using Epics.ChannelAccess.Provider;

	public partial class ElectronGun : ControlPanel
	{
		public ElectronGun()
		{
			this.InitializeComponent();

			this.AddControlForDataBinding(this.highVoltageOnCheckButton, PredefinedChannels.HighVoltageSupply.On);
			this.AddControlForDataBinding(this.highVoltageMeasuredVoltageValueLabel, PredefinedChannels.HighVoltageSupply.MeasuredVoltage);
			this.AddControlForDataBinding(this.highVoltagePresetVoltageValueSpinEdit, PredefinedChannels.HighVoltageSupply.PresetVoltage);
			this.AddControlForDataBinding(this.highVoltagePresetVoltageValueTrackBar, PredefinedChannels.HighVoltageSupply.PresetVoltage);

			this.AddDevExpressControlForBugFixes(this.highVoltageOnCheckButton, this.highVoltageOffCheckButton);
			// ToDo: Add other controls
		}

		public override void InitializeDataBindingConverters()
		{
			// The format event fires when pulling data from a source into a control and the Parse event fires when pulling data from a control back into the datasource

			// Convert V to kV
			this.highVoltageMeasuredVoltageValueLabel.DataBindings[0].Format += (s, e) => e.Value = Math.Round((double)e.Value * 1E-3, 3);
			this.highVoltagePresetVoltageValueSpinEdit.DataBindings[0].Format += (s, e) => e.Value = (decimal)Math.Round((double)e.Value * 1E-3, 3);
			this.highVoltagePresetVoltageValueSpinEdit.DataBindings[0].Parse += (s, e) => e.Value = (int)e.Value * 1000;
			this.highVoltagePresetVoltageValueTrackBar.DataBindings[0].Format += (s, e) => e.Value = (int)((double)e.Value * 1E-3);
			this.highVoltagePresetVoltageValueTrackBar.DataBindings[0].Parse += (s, e) => e.Value = (int)e.Value * 1E3;
		}
	}
}
