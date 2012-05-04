// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlPanel.cs" company="Turkish Accelerator Center">
//   Copyright (c) 2010 - 2011 Teoman Soygul. Licensed under LGPLv3 (http://www.gnu.org/licenses/lgpl.html).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Epics.UserControls.Panels
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows.Forms;

	using DevExpress.XtraEditors;

	using Epics.ChannelAccess.Provider;

	// [System.ComponentModel.ToolboxItem(false)] // Can't use this because all the inheritors also inherit this and disapper from toolbox
	public partial class ControlPanel : UserControl
	{
		/// <summary>
		/// The list of controls and matching data access channel settings used to bind the controls to the channel data.
		/// First parameter is a <see cref="Control"/> to set the data binding for, second parameter is a
		/// <see cref="Epics.ChannelAccess.Provider.PredefinedChannel"/> instance to create a channel with
		/// predefined settings.
		/// </summary>
		private readonly Dictionary<Control, PredefinedChannel> listOfControlsWithDataBinding = new Dictionary<Control, PredefinedChannel>();
		
		/// <summary>
		/// The list of controls with sectors ID texts that needs to match the current sector ID parameters of the 
		/// container control, specified at the 'Parameters' window.
		/// </summary>
		private readonly List<Control> listOfControlsWithSectorIdText = new List<Control>();

		/// <summary>
		/// The list of DevExpress controls that needs bug fixes. First filed is the On button, second field is the Off button.
		/// </summary>
		private readonly Dictionary<CheckButton, CheckButton> listOfDevExpressBugFixControls = new Dictionary<CheckButton, CheckButton>();

		private int oldSectorId;
		private string oldSectorIdText;

		/// <summary>
		/// Variables are initialized to default values since class constructor is called before the public properties are set 
		/// by the UI for user controls.
		/// </summary>
		private int sectorId = 1;
		private string sectorIdText = "01";

		/// <summary>
		/// Initializes a new instance of the ControlPanel class and provides a constructor for all derived <see cref="UserControl"/> types.
		/// </summary>
		public ControlPanel()
		{
			this.InitializeComponent();

			// this.Load += (sender, e) => {  /* ToDo: Initializers here! */ };
		}

		/// <summary>
		/// Gets or sets the sector number on beam line elements. This property is important because it initializes
		/// the Channel Access protocol since sector no is a prerequisite of this initialization.
		/// </summary>
		public int SectorId
		{
			get
			{
				return this.sectorId;
			}

			set
			{
				this.oldSectorId = this.sectorId;
				this.oldSectorIdText = this.sectorIdText;
				this.sectorId = value;

				if (value < 10)
				{
					this.sectorIdText = "0" + value;
				}
				else
				{
					this.sectorIdText = value.ToString();
				}

				// ToDo: Having below commands here in a property requires this property to be set to a proper value at design time
				// or else initializers won't run. It is better to have these commands in some sort of a constructor.
				this.SetSectorIdTexts();

				// Do not do data bindings at design time. Also just checking this.DesignMode is not enough for composite controls
				if (this.DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
				{
				}
				else
				{
					this.InitializeDataBindings();
					this.InitializeCustomDataBindings();
					this.InitializeDataBindingConverters();
				}

				this.DevExpressBugFixes();
			}
		}

		public virtual void InitializeCustomDataBindings()
		{
		}

		public virtual void InitializeDataBindingConverters()
		{
		}

		/*public ChannelAccess ChannelAccess
		{
			get
			{
				return this.channelAccess;
			}

			set
			{
				this.channelAccess = value;
			}
		}*/

		public void AddControlForDataBinding(Control control, PredefinedChannel predefinedChannel)
		{
			this.listOfControlsWithDataBinding.Add(control, predefinedChannel);
		}

		public void AddControlForSectorIdTextSettings(Control control)
		{
			this.listOfControlsWithSectorIdText.Add(control);
		}

		public void AddDevExpressControlForBugFixes(CheckButton checkButton1, CheckButton checkButton2)
		{
			this.listOfDevExpressBugFixControls.Add(checkButton1, checkButton2);
		}

		/// <summary>
		/// Initializes DevExpress UI bug fixes.
		/// </summary>
		/// <exception cref="Exception"><see cref="listOfDevExpressBugFixControls"/> cannot be left empty.</exception>
		private void DevExpressBugFixes()
		{
			foreach (KeyValuePair<CheckButton, CheckButton> keyValuePair in this.listOfDevExpressBugFixControls)
			{
				CheckButton control1 = keyValuePair.Key;
				CheckButton control2 = keyValuePair.Value;
				control1.CheckedChanged += (sender, e) =>
				{
					if (control1.Tag != null)
					{
						control1.Tag = null;
					}
					else
					{
						control2.Tag = "GroupToggle";
						control2.Toggle();
					}
				};

				control2.CheckedChanged += (sender, e) =>
				{
					if (control2.Tag != null)
					{
						control2.Tag = null;
					}
					else
					{
						control1.Tag = "GroupToggle";
						control1.Toggle();
					}
				};
			}
		}

		/// <summary>
		/// Initializes all data binding on the <see cref="UserControl"/> and creates corresponding client channels.
		/// </summary>
		/// <exception cref="ArgumentException">Throws <c>ArgumentException</c> parameter <see cref="Type"/> is not know yet.</exception>
		private void InitializeDataBindings()
		{
			foreach (var keyValuePair in this.listOfControlsWithDataBinding)
			{
				// Set component ID only if it wasn't explicitly appointed already
				if (keyValuePair.Value.SectorId == 0)
				{
					keyValuePair.Value.SetId(this.SectorId);
				}

				string controlPropertyName;

				if (keyValuePair.Key is LabelControl)
				{
					controlPropertyName = "Text";
				}
				else if (keyValuePair.Key is CheckButton)
				{
					controlPropertyName = "Checked";
				}
				else if (keyValuePair.Key is SpinEdit)
				{
					controlPropertyName = "Value";
				}
				else if (keyValuePair.Key is TrackBarControl)
				{
					controlPropertyName = "Value";
				}
				else if (keyValuePair.Key is ProgressBarControl)
				{
					controlPropertyName = "Text";
				}
				else if (keyValuePair.Key is PictureEdit)
				{
					controlPropertyName = "Image";
				}
				else
				{
					throw new ArgumentException("Supplied parameter: " + keyValuePair.Key.GetType() + " is not know yet. Add it to known properties list.");
				}

				var dataSource = typeof(IClient)
					.GetMethod("CreateChannel", new[] { typeof(PredefinedChannel) })
					.MakeGenericMethod(keyValuePair.Value.DataType)
					.Invoke(Provider.Client, new[] { keyValuePair.Value });

				keyValuePair.Key.DataBindings.Add(controlPropertyName, dataSource, "Value", false, DataSourceUpdateMode.OnPropertyChanged);
			}
		}

		/// <summary>
		/// Replaces the sector ID texts of the specified list of controls with the new matching ID numbers.
		/// </summary>
		/// <exception cref="Exception"><see cref="listOfControlsWithSectorIdText"/> list cannot be left empty.</exception>
		/// <exception cref="ApplicationException">Specified control does not have a sector identifier.</exception>
		private void SetSectorIdTexts()
		{
			foreach (var control in this.listOfControlsWithSectorIdText.Where(control => !string.IsNullOrEmpty(control.Text)))
			{
				if (control.Text.IndexOf(this.oldSectorIdText) != -1)
				{
					control.Text = control.Text.Replace(this.oldSectorIdText, this.sectorIdText);
				}
				else if (control.Text.IndexOf(this.oldSectorId.ToString()) != -1)
				{
					control.Text = control.Text.Replace(this.oldSectorId.ToString(), this.sectorId.ToString());
				}
				else
				{
					throw new ApplicationException("Specified control does not have a sector identifier.");
				}
			}
		}
	}
}
