// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Injector.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.UserControls.Panels.ProcessVisualization
{
	using Epics.ChannelAccess.Provider;

	public partial class Injector : ControlPanel
	{
		public Injector()
		{
			this.InitializeComponent();

			this.AddControlForDataBinding(this.gateValve01PullButton, PredefinedChannels.GateValve.Closed.SetId(1));
			this.AddControlForDataBinding(this.gateValve02PullButton, PredefinedChannels.GateValve.Closed.SetId(2));
			this.AddControlForDataBinding(this.gateValve03PullButton, PredefinedChannels.GateValve.Closed.SetId(3));
			this.AddControlForDataBinding(this.gateValve04PullButton, PredefinedChannels.GateValve.Closed.SetId(4));
			this.AddControlForDataBinding(this.aperture01PullButton, PredefinedChannels.Aperture.In.SetId(1));
			this.AddControlForDataBinding(this.aperture02PullButton, PredefinedChannels.Aperture.In.SetId(2));
			this.AddControlForDataBinding(this.aperture03PullButton, PredefinedChannels.Aperture.In.SetId(3));
			this.AddControlForDataBinding(this.aperture04PullButton, PredefinedChannels.Aperture.In.SetId(4));
		}
	}
}
