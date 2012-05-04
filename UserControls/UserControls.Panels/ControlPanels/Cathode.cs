// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Cathode.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.UserControls.Panels.ControlPanels
{
	using System;

	using Epics.ChannelAccess.Provider;

	public partial class Cathode : ControlPanel
	{
		public Cathode()
		{
			this.InitializeComponent();

			this.AddControlForDataBinding(this.cathodeTempValueLabel, PredefinedChannels.Cathode.Temperature);
			this.AddControlForDataBinding(this.cathodeTempProgressBar, PredefinedChannels.Cathode.Temperature);
			this.AddControlForDataBinding(this.cathodeCurrentValueSpinEdit, PredefinedChannels.Cathode.Current);
			this.AddControlForDataBinding(this.cathodeCurrentTrackBar, PredefinedChannels.Cathode.Current);
		}

		public override void InitializeDataBindingConverters()
		{
			// The format event fires when pulling data from a source into a control and the Parse event fires when pulling data from a control back into the datasource
			
			// Convert Kelvin to Celcius
			this.cathodeTempValueLabel.DataBindings[0].Format += (s, e) => e.Value = (double)e.Value - 273 < 0 ? 0 : Math.Round((double)e.Value - 273, 4);
			this.cathodeTempProgressBar.DataBindings[0].Format += (s, e) => e.Value = (double)e.Value - 273 < 0 ? "0" : ((int)(double)e.Value - 273).ToString();

			// Convert A to uA and vice versa
			this.cathodeCurrentValueSpinEdit.DataBindings[0].Format += (s, e) => e.Value = (double)e.Value * 1E6;
			this.cathodeCurrentValueSpinEdit.DataBindings[0].Parse += (s, e) => e.Value = (decimal)e.Value * 0.000001m;
			this.cathodeCurrentTrackBar.DataBindings[0].Format += (s, e) => e.Value = (double)e.Value * 1E6;
			this.cathodeCurrentTrackBar.DataBindings[0].Parse += (s, e) => e.Value = (int)e.Value * 1E-6;
		}
	}
}
