// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Aperture.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.UserControls.Graphics.BeamlineGraphics
{
	using System.Windows.Forms;

	public partial class Aperture : UserControl
	{
		/*[Browsable(true)] // Browsable in the "Properties" window
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)] // Make sure that the property value is saved in the setup data for the form
		[EditorBrowsable(EditorBrowsableState.Always)] // Make sure that property appears in the drop-down Intellisense list in Visual Studio
		[Bindable(true)] // Make the property data bindable (for future use)
		public override string Text
		{
			get
			{
				return this.textLabel.Text;
			}

			set
			{
				this.textLabel.Text = value;
			}
		}*/
		public Aperture()
		{
			this.InitializeComponent();
		}
	}
}
