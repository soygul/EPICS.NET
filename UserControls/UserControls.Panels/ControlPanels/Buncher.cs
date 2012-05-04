// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Buncher.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.UserControls.Panels.ControlPanels
{
	using System.Windows.Forms;

	public partial class Buncher : UserControl
	{
		private readonly string[] buncherTypeFrequencies = { "260 MHz", "1.3 GHz" };

		private readonly string[] buncherTypeNames = { "Subharmonic", "Fundamental" };

		private int buncherNo = 1;

		private BuncherTypeName buncherType = BuncherTypeName.SubharmonicBuncher;

		public Buncher()
		{
			this.InitializeComponent();
		}

		public enum BuncherTypeName
		{
			/// <summary>
			/// Subharmonic Buncher type.
			/// </summary>
			SubharmonicBuncher = 0, 

			/// <summary>
			/// Fundamental Buncher type.
			/// </summary>
			FundamentalBuncher = 1
		}

		public string BuncherGradientWP
		{
			get
			{
				return this.buncherGradientWPProgressBar.Text;
			}

			set
			{
				this.buncherGradientWPProgressBar.Text = value;
			}
		}

		public int BuncherNo
		{
			get
			{
				return this.buncherNo;
			}

			set
			{
				this.buncherGroup.Text = this.buncherGroup.Text.Replace(this.buncherNo.ToString(), value.ToString());
				this.buncherNo = value;
			}
		}

		public string BuncherPhaseWP
		{
			get
			{
				return this.buncherPhaseWPProgressBar.Text;
			}

			set
			{
				this.buncherPhaseWPProgressBar.Text = value;
			}
		}

		public BuncherTypeName BuncherType
		{
			get
			{
				return this.buncherType;
			}

			set
			{
				this.buncherGroup.Text = this.buncherGroup.Text.Replace(this.buncherTypeNames[(int)this.buncherType], this.buncherTypeNames[(int)value]);
				this.buncherFrequencyPhaseLabel.Text = this.buncherFrequencyPhaseLabel.Text.Replace(this.buncherTypeFrequencies[(int)this.buncherType], this.buncherTypeFrequencies[(int)value]);
				this.buncherType = value;
			}
		}
	}
}
