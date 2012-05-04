// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BeamLineChart.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.UserControls.Panels.ControlPanels
{
	using System;
	using System.Windows.Forms;

	using Epics.ChannelAccess.Provider;

	public partial class BeamLineChart : ControlPanel
	{
		private readonly Timer timer = new Timer();
		private readonly double[] xSeries;
		private readonly double[] ySeries;
		private readonly double[] currentSeries;

		public BeamLineChart()
		{
			this.InitializeComponent();
			this.xSeries = new double[this.beamLineChartControl.Series[0].Points.Count];
			this.ySeries = new double[this.beamLineChartControl.Series[1].Points.Count];
			this.currentSeries = new double[this.beamLineChartControl.Series[2].Points.Count];

			// Set timer interval default
			this.timer.Interval = 250;
			this.timer.Tick += (sender, e) => this.UpdateChart();
		}

		public int UpdateInterval
		{
			get
			{
				return this.timer.Interval;
			}

			set
			{
				this.timer.Interval = value;
			}
		}

		public override void InitializeCustomDataBindings()
		{
			// Cache all the values before updating chart (which takes the lion's share in CPU usage)
			for (var i = 0; i < this.xSeries.Length; i++)
			{
				var index = i;
				Provider.Client.CreateChannel<double>(PredefinedChannels.BeamPositionMonitor.XPosition.SetId(index)).Changed +=
					newValue => this.xSeries[index] = Math.Round(newValue * 1E3); // m to mm
			}

			for (var i = 0; i < this.ySeries.Length; i++)
			{
				var index = i;
				Provider.Client.CreateChannel<double>(PredefinedChannels.BeamPositionMonitor.YPosition.SetId(index)).Changed +=
					newValue => this.ySeries[index] = Math.Round(newValue * 1E3); // m to mm
			}

			for (var i = 0; i < this.currentSeries.Length; i++)
			{
				var index = i;
				Provider.Client.CreateChannel<double>(PredefinedChannels.BeamPositionMonitor.Current.SetId(index)).Changed +=
					newValue => this.currentSeries[index] = Math.Round(newValue * 25E3); // A to 10uA
			}

			// Update the chart periodically
			this.timer.Start();

			/* Legacy data binding:
			Provider.Client.CreateChannel<double>(PredefinedChannels.BeamPositionMonitor.XPosition.SetId(1)).Changed +=
				newValue => { this.beamLineChartControl.Series[0].Points[1].Values[0] = (int)(newValue * 1E3); };
			Provider.Client.CreateChannel<double>(PredefinedChannels.BeamPositionMonitor.YPosition.SetId(1)).Changed +=
				newValue => { this.beamLineChartControl.Series[1].Points[1].Values[0] = (int)(newValue * 1E3); };
			Provider.Client.CreateChannel<double>(PredefinedChannels.BeamPositionMonitor.Current.SetId(1)).Changed +=
				newValue => { this.beamLineChartControl.Series[2].Points[1].Values[0] = (int)(newValue * 0.5E5); this.beamLineChartControl.RefreshData(); };
			
			Provider.Client.CreateChannel<double>(PredefinedChannels.BeamPositionMonitor.XPosition.SetId(2)).Changed +=
				newValue => { this.beamLineChartControl.Series[0].Points[2].Values[0] = (int)(newValue * 1E3); };
			Provider.Client.CreateChannel<double>(PredefinedChannels.BeamPositionMonitor.YPosition.SetId(2)).Changed +=
				newValue => { this.beamLineChartControl.Series[1].Points[2].Values[0] = (int)(newValue * 1E3); };
			Provider.Client.CreateChannel<double>(PredefinedChannels.BeamPositionMonitor.Current.SetId(2)).Changed +=
				newValue => { this.beamLineChartControl.Series[2].Points[2].Values[0] = (int)(newValue * 0.5E5); };*/
		}

		private void UpdateChart()
		{
			if (this.beamLineChartControl.Series[0] == null)
			{
				return;
			}

			var updatedAtLeasOnce = false;

			for (var i = 0; i < this.xSeries.Length; i++)
			{
				// Pre-appointment checks are necessary because actually updating the values utilizes the CPU massively
				if (this.beamLineChartControl.Series[0].Points[i].Values[0] != this.xSeries[i])
				{
					this.beamLineChartControl.Series[0].Points[i].Values[0] = this.xSeries[i];
					updatedAtLeasOnce = true;
				}
			}

			for (var i = 0; i < this.ySeries.Length; i++)
			{
				if (this.beamLineChartControl.Series[1].Points[i].Values[0] != this.ySeries[i])
				{
					this.beamLineChartControl.Series[1].Points[i].Values[0] = this.ySeries[i];
					updatedAtLeasOnce = true;
				}
			}

			for (var i = 0; i < this.currentSeries.Length; i++)
			{
				if (this.beamLineChartControl.Series[2].Points[i].Values[0] != this.currentSeries[i])
				{
					this.beamLineChartControl.Series[2].Points[i].Values[0] = this.currentSeries[i];
					updatedAtLeasOnce = true;
				}
			}

			if (updatedAtLeasOnce)
			{
				this.beamLineChartControl.RefreshData();
			}
		}
	}
}