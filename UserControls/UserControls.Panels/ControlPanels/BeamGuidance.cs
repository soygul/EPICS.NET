// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BeamGuidance.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.UserControls.Panels.ControlPanels
{
	using System;
	using System.Drawing;
	using System.IO;
	using System.Text;

	using Epics.ChannelAccess.Provider;

	public partial class BeamGuidance : ControlPanel
	{
		public BeamGuidance()
		{
			this.InitializeComponent();

			this.AddControlForSectorIdTextSettings(this.beamGuidanceGroup);
			this.AddControlForSectorIdTextSettings(this.bpmLabel);
			this.AddControlForSectorIdTextSettings(this.steeringMagnetXCurrentLabel);
			this.AddControlForSectorIdTextSettings(this.steeringMagnetYCurrentLabel);
			this.AddControlForSectorIdTextSettings(this.solenoidCurrentLabel);
			this.AddControlForSectorIdTextSettings(this.apertureRadiusLabel);
			this.AddControlForSectorIdTextSettings(this.beamProfileMonitorLabel);
			this.AddControlForSectorIdTextSettings(this.cameraLabel);
			this.AddControlForSectorIdTextSettings(this.laserLabel);
			this.AddControlForSectorIdTextSettings(this.viewScreenLabel);

			this.AddControlForDataBinding(this.bpmCurrentValueLabel, PredefinedChannels.BeamPositionMonitor.Current);
			this.AddControlForDataBinding(this.bpmXPositionValueLabel, PredefinedChannels.BeamPositionMonitor.XPosition);
			this.AddControlForDataBinding(this.bpmYPositionValueLabel, PredefinedChannels.BeamPositionMonitor.YPosition);
			this.AddControlForDataBinding(this.steeringMagnetXCurrentOnCheckButton, PredefinedChannels.SteeringMagnet.XCurrentOn);
			this.AddControlForDataBinding(this.steeringMagnetXCurrentValueSpinEdit, PredefinedChannels.SteeringMagnet.XCurrent);
			this.AddControlForDataBinding(this.steeringMagnetXCurrentValueTrackBar, PredefinedChannels.SteeringMagnet.XCurrent);
			this.AddControlForDataBinding(this.steeringMagnetYCurrentOnCheckButton, PredefinedChannels.SteeringMagnet.YCurrentOn);
			this.AddControlForDataBinding(this.steeringMagnetYCurrentValueSpinEdit, PredefinedChannels.SteeringMagnet.YCurrent);
			this.AddControlForDataBinding(this.steeringMagnetYCurrentValueTrackBar, PredefinedChannels.SteeringMagnet.YCurrent);
			this.AddControlForDataBinding(this.solenoidCurrentOnCheckButton, PredefinedChannels.Solenoid.CurrentOn);
			this.AddControlForDataBinding(this.solenoidCurrentValueSpinEdit, PredefinedChannels.Solenoid.Current);
			this.AddControlForDataBinding(this.solenoidCurrentValueTrackBar, PredefinedChannels.Solenoid.Current);
			this.AddControlForDataBinding(this.apertureInCheckButton, PredefinedChannels.Aperture.In);
			this.AddControlForDataBinding(this.apertureRadiusValueSpinEdit, PredefinedChannels.Aperture.Radius);
			this.AddControlForDataBinding(this.apertureRadiusValueTrackBar, PredefinedChannels.Aperture.Radius);
			this.AddControlForDataBinding(this.cameraOnCheckButton, PredefinedChannels.BeamProfileMonitor.CameraOn);
			this.AddControlForDataBinding(this.laserOnCheckButton, PredefinedChannels.BeamProfileMonitor.LaserOn);
			this.AddControlForDataBinding(this.viewScreenInCheckButton, PredefinedChannels.BeamProfileMonitor.ViewScreenIn);
			
			this.AddDevExpressControlForBugFixes(this.steeringMagnetXCurrentOnCheckButton, this.steeringMagnetXCurrentOffCheckButton);
			this.AddDevExpressControlForBugFixes(this.steeringMagnetYCurrentOnCheckButton, this.steeringMagnetYCurrentOffCheckButton);
			this.AddDevExpressControlForBugFixes(this.solenoidCurrentOnCheckButton, this.solenoidCurrentOffCheckButton);
			this.AddDevExpressControlForBugFixes(this.apertureInCheckButton, this.apertureOutCheckButton);
			this.AddDevExpressControlForBugFixes(this.cameraOnCheckButton, this.cameraOffCheckButton);
			this.AddDevExpressControlForBugFixes(this.laserOnCheckButton, this.laserOffCheckButton);
			this.AddDevExpressControlForBugFixes(this.viewScreenInCheckButton, this.viewScreenOutCheckButton);
		}

		public override void InitializeCustomDataBindings()
		{
			Provider.Client.CreateChannel<string>(PredefinedChannels.BeamProfileMonitor.Image.SetId(this.SectorId)).Changed += newValue =>
				{
					if (!string.IsNullOrEmpty(newValue) && newValue != "0")
					{
						using (var stream = new MemoryStream(Convert.FromBase64String(newValue)))
						{
							this.beamProfileMonitorPicture.Image = new Bitmap(stream);
						}
					}
				};
		}

		public override void InitializeDataBindingConverters()
		{
			// Custom filters to prevent visualization control from consuming excessive resources (especially the processing power)
			Provider.Client.CreateChannel<double>(PredefinedChannels.BeamPositionMonitor.Current.SetId(this.SectorId)).RegisterPercentageFilter(75);
			Provider.Client.CreateChannel<double>(PredefinedChannels.BeamPositionMonitor.XPosition.SetId(this.SectorId)).RegisterPercentageFilter(75);
			Provider.Client.CreateChannel<double>(PredefinedChannels.BeamPositionMonitor.YPosition.SetId(this.SectorId)).RegisterPercentageFilter(75);

			// The format event fires when pulling data from a source into a control and the Parse event fires when pulling data from a control back into the datasource
			// Convert A to uA)
			this.bpmCurrentValueLabel.DataBindings[0].Format += (s, e) => e.Value = Math.Round((double)e.Value * 1E6, 3).ToString();

			// Convert m to um (for label) then mm (for chart)
			this.bpmXPositionValueLabel.DataBindings[0].Format += (s, e) =>
			{
				this.bpmChart.Series[0].Points[0].Argument = ((int)((double)e.Value * 1E3)).ToString();
				e.Value = Math.Round((double)e.Value * 1E6, 3).ToString();
				this.bpmChart.RefreshData();
			};
			this.bpmYPositionValueLabel.DataBindings[0].Format += (s, e) =>
			{
				this.bpmChart.Series[0].Points[0].Values[0] = (int)((double)e.Value * 1E3);
				e.Value = Math.Round((double)e.Value * 1E6, 3).ToString();
				this.bpmChart.RefreshData();
			};

			// Convert A to mA and vice versa for steering magnets
			this.steeringMagnetXCurrentValueSpinEdit.DataBindings[0].Format += (s, e) => e.Value = Math.Round((double)e.Value * 1E3, 3);
			this.steeringMagnetXCurrentValueSpinEdit.DataBindings[0].Parse += (s, e) => e.Value = (decimal)e.Value * 0.001m;
			this.steeringMagnetXCurrentValueTrackBar.DataBindings[0].Format += (s, e) => e.Value = (double)e.Value * 1E3;
			this.steeringMagnetXCurrentValueTrackBar.DataBindings[0].Parse += (s, e) => e.Value = (int)e.Value * 1E-3;
			this.steeringMagnetYCurrentValueSpinEdit.DataBindings[0].Format += (s, e) => e.Value = Math.Round((double)e.Value * 1E3, 3);
			this.steeringMagnetYCurrentValueSpinEdit.DataBindings[0].Parse += (s, e) => e.Value = (decimal)e.Value * 0.001m;
			this.steeringMagnetYCurrentValueTrackBar.DataBindings[0].Format += (s, e) => e.Value = (double)e.Value * 1E3;
			this.steeringMagnetYCurrentValueTrackBar.DataBindings[0].Parse += (s, e) => e.Value = (int)e.Value * 1E-3;

			// Convert m to mm and vice versa for aperture
			this.apertureRadiusValueSpinEdit.DataBindings[0].Format += (s, e) => e.Value = Math.Round((double)e.Value * 1E3, 3);
			this.apertureRadiusValueSpinEdit.DataBindings[0].Parse += (s, e) => e.Value = (decimal)e.Value * 0.001m;
			this.apertureRadiusValueTrackBar.DataBindings[0].Format += (s, e) => e.Value = (double)e.Value * 1E3;
			this.apertureRadiusValueTrackBar.DataBindings[0].Parse += (s, e) => e.Value = (int)e.Value * 1E-3;

			// Convert A to mA and vice versa for solenoid
			this.solenoidCurrentValueSpinEdit.DataBindings[0].Format += (s, e) => e.Value = Math.Round((double)e.Value * 1E3, 3);
			this.solenoidCurrentValueSpinEdit.DataBindings[0].Parse += (s, e) => e.Value = (decimal)e.Value * 0.001m;
			this.solenoidCurrentValueTrackBar.DataBindings[0].Format += (s, e) => e.Value = (double)e.Value * 1E3;
			this.solenoidCurrentValueTrackBar.DataBindings[0].Parse += (s, e) => e.Value = (int)e.Value * 1E-3;
		}
	}
}
