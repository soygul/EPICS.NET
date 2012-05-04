namespace Epics.UserControls.Panels.ControlPanels
{
	partial class BeamOnOff
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.beamOnPanelControl = new DevExpress.XtraEditors.PanelControl();
			this.beamOnOffCheckButton = new DevExpress.XtraEditors.CheckButton();
			((System.ComponentModel.ISupportInitialize)(this.beamOnPanelControl)).BeginInit();
			this.beamOnPanelControl.SuspendLayout();
			this.SuspendLayout();
			// 
			// beamOnPanelControl
			// 
			this.beamOnPanelControl.Controls.Add(this.beamOnOffCheckButton);
			this.beamOnPanelControl.Location = new System.Drawing.Point(0, 0);
			this.beamOnPanelControl.Name = "beamOnPanelControl";
			this.beamOnPanelControl.Size = new System.Drawing.Size(79, 27);
			this.beamOnPanelControl.TabIndex = 4;
			// 
			// beamOnOffCheckButton
			// 
			this.beamOnOffCheckButton.AllowFocus = false;
			this.beamOnOffCheckButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.beamOnOffCheckButton.Location = new System.Drawing.Point(2, 2);
			this.beamOnOffCheckButton.Name = "beamOnOffCheckButton";
			this.beamOnOffCheckButton.Size = new System.Drawing.Size(75, 23);
			this.beamOnOffCheckButton.TabIndex = 3;
			this.beamOnOffCheckButton.Text = "Beam Off";
			this.beamOnOffCheckButton.CheckedChanged += new System.EventHandler(this.BeamOnOffCheckButton_CheckedChanged);
			// 
			// BeamOnOff
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.beamOnPanelControl);
			this.Name = "BeamOnOff";
			this.Size = new System.Drawing.Size(79, 27);
			((System.ComponentModel.ISupportInitialize)(this.beamOnPanelControl)).EndInit();
			this.beamOnPanelControl.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.PanelControl beamOnPanelControl;
		private DevExpress.XtraEditors.CheckButton beamOnOffCheckButton;

	}
}
