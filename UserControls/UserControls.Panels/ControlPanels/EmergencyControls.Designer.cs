namespace Epics.UserControls.Panels.ControlPanels
{
	partial class EmergencyControls
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
			this.emergencyBeamLossControlLabel = new DevExpress.XtraEditors.LabelControl();
			this.beamLossLabel = new DevExpress.XtraEditors.LabelControl();
			this.emergencySwitchesLabel = new DevExpress.XtraEditors.LabelControl();
			this.emergencyBeamControlLabel = new DevExpress.XtraEditors.LabelControl();
			this.emergencyLatchCheckEdit = new DevExpress.XtraEditors.CheckEdit();
			this.emergencyHVControlLabel = new DevExpress.XtraEditors.LabelControl();
			this.emergencyPulseGeneratorControlLabel = new DevExpress.XtraEditors.LabelControl();
			this.emergencyMainGateControlLabel = new DevExpress.XtraEditors.LabelControl();
			this.beamCheckButton = new DevExpress.XtraEditors.CheckButton();
			this.hVCheckButton = new DevExpress.XtraEditors.CheckButton();
			this.gateCheckButton = new DevExpress.XtraEditors.CheckButton();
			this.pulseGeneratorCheckButton = new DevExpress.XtraEditors.CheckButton();
			((System.ComponentModel.ISupportInitialize)(this.emergencyLatchCheckEdit.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// emergencyBeamLossControlLabel
			// 
			this.emergencyBeamLossControlLabel.Location = new System.Drawing.Point(17, 127);
			this.emergencyBeamLossControlLabel.Name = "emergencyBeamLossControlLabel";
			this.emergencyBeamLossControlLabel.Size = new System.Drawing.Size(54, 13);
			this.emergencyBeamLossControlLabel.TabIndex = 90;
			this.emergencyBeamLossControlLabel.Text = "Beam Loss:";
			// 
			// beamLossLabel
			// 
			this.beamLossLabel.Appearance.BackColor = System.Drawing.Color.White;
			this.beamLossLabel.Appearance.BorderColor = System.Drawing.Color.LightGray;
			this.beamLossLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.beamLossLabel.Appearance.ForeColor = System.Drawing.Color.LightGray;
			this.beamLossLabel.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.beamLossLabel.Appearance.Options.UseBackColor = true;
			this.beamLossLabel.Appearance.Options.UseBorderColor = true;
			this.beamLossLabel.Appearance.Options.UseFont = true;
			this.beamLossLabel.Appearance.Options.UseForeColor = true;
			this.beamLossLabel.Appearance.Options.UseTextOptions = true;
			this.beamLossLabel.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.beamLossLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.beamLossLabel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.beamLossLabel.Location = new System.Drawing.Point(118, 124);
			this.beamLossLabel.Name = "beamLossLabel";
			this.beamLossLabel.Padding = new System.Windows.Forms.Padding(2, 0, 3, 0);
			this.beamLossLabel.Size = new System.Drawing.Size(60, 20);
			this.beamLossLabel.TabIndex = 89;
			this.beamLossLabel.Text = "Alert";
			this.beamLossLabel.UseMnemonic = false;
			// 
			// emergencySwitchesLabel
			// 
			this.emergencySwitchesLabel.Location = new System.Drawing.Point(8, 10);
			this.emergencySwitchesLabel.Name = "emergencySwitchesLabel";
			this.emergencySwitchesLabel.Size = new System.Drawing.Size(102, 13);
			this.emergencySwitchesLabel.TabIndex = 0;
			this.emergencySwitchesLabel.Text = "Emergency Switches:";
			// 
			// emergencyBeamControlLabel
			// 
			this.emergencyBeamControlLabel.Location = new System.Drawing.Point(17, 35);
			this.emergencyBeamControlLabel.Name = "emergencyBeamControlLabel";
			this.emergencyBeamControlLabel.Size = new System.Drawing.Size(30, 13);
			this.emergencyBeamControlLabel.TabIndex = 77;
			this.emergencyBeamControlLabel.Text = "Beam:";
			// 
			// emergencyLatchCheckEdit
			// 
			this.emergencyLatchCheckEdit.EditValue = true;
			this.emergencyLatchCheckEdit.Location = new System.Drawing.Point(125, 8);
			this.emergencyLatchCheckEdit.Name = "emergencyLatchCheckEdit";
			this.emergencyLatchCheckEdit.Properties.Caption = "Latch";
			this.emergencyLatchCheckEdit.Size = new System.Drawing.Size(53, 18);
			this.emergencyLatchCheckEdit.TabIndex = 78;
			// 
			// emergencyHVControlLabel
			// 
			this.emergencyHVControlLabel.Location = new System.Drawing.Point(17, 58);
			this.emergencyHVControlLabel.Name = "emergencyHVControlLabel";
			this.emergencyHVControlLabel.Size = new System.Drawing.Size(68, 13);
			this.emergencyHVControlLabel.TabIndex = 80;
			this.emergencyHVControlLabel.Text = "Hight Voltage:";
			// 
			// emergencyPulseGeneratorControlLabel
			// 
			this.emergencyPulseGeneratorControlLabel.Location = new System.Drawing.Point(17, 104);
			this.emergencyPulseGeneratorControlLabel.Name = "emergencyPulseGeneratorControlLabel";
			this.emergencyPulseGeneratorControlLabel.Size = new System.Drawing.Size(81, 13);
			this.emergencyPulseGeneratorControlLabel.TabIndex = 84;
			this.emergencyPulseGeneratorControlLabel.Text = "Pulse Generator:";
			// 
			// emergencyMainGateControlLabel
			// 
			this.emergencyMainGateControlLabel.Location = new System.Drawing.Point(17, 81);
			this.emergencyMainGateControlLabel.Name = "emergencyMainGateControlLabel";
			this.emergencyMainGateControlLabel.Size = new System.Drawing.Size(52, 13);
			this.emergencyMainGateControlLabel.TabIndex = 82;
			this.emergencyMainGateControlLabel.Text = "Main Gate:";
			// 
			// beamCheckButton
			// 
			this.beamCheckButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.beamCheckButton.Location = new System.Drawing.Point(118, 32);
			this.beamCheckButton.Name = "beamCheckButton";
			this.beamCheckButton.Size = new System.Drawing.Size(60, 20);
			this.beamCheckButton.TabIndex = 91;
			this.beamCheckButton.Text = "Beam Off";
			this.beamCheckButton.CheckedChanged += new System.EventHandler(this.BeamCheckButton_CheckedChanged);
			// 
			// hVCheckButton
			// 
			this.hVCheckButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.hVCheckButton.Location = new System.Drawing.Point(118, 55);
			this.hVCheckButton.Name = "hVCheckButton";
			this.hVCheckButton.Size = new System.Drawing.Size(60, 20);
			this.hVCheckButton.TabIndex = 92;
			this.hVCheckButton.Text = "HV Off";
			this.hVCheckButton.CheckedChanged += new System.EventHandler(this.HVCheckButton_CheckedChanged);
			// 
			// gateCheckButton
			// 
			this.gateCheckButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.gateCheckButton.Location = new System.Drawing.Point(118, 78);
			this.gateCheckButton.Name = "gateCheckButton";
			this.gateCheckButton.Size = new System.Drawing.Size(60, 20);
			this.gateCheckButton.TabIndex = 93;
			this.gateCheckButton.Text = "Closed";
			this.gateCheckButton.CheckedChanged += new System.EventHandler(this.GateCheckButton_CheckedChanged);
			// 
			// pulseGeneratorCheckButton
			// 
			this.pulseGeneratorCheckButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pulseGeneratorCheckButton.Location = new System.Drawing.Point(118, 101);
			this.pulseGeneratorCheckButton.Name = "pulseGeneratorCheckButton";
			this.pulseGeneratorCheckButton.Size = new System.Drawing.Size(60, 20);
			this.pulseGeneratorCheckButton.TabIndex = 94;
			this.pulseGeneratorCheckButton.Text = "PG Off";
			this.pulseGeneratorCheckButton.CheckedChanged += new System.EventHandler(this.PulseGeneratorCheckButton_CheckedChanged);
			// 
			// EmergencyControls
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.pulseGeneratorCheckButton);
			this.Controls.Add(this.gateCheckButton);
			this.Controls.Add(this.hVCheckButton);
			this.Controls.Add(this.beamCheckButton);
			this.Controls.Add(this.emergencyBeamLossControlLabel);
			this.Controls.Add(this.beamLossLabel);
			this.Controls.Add(this.emergencyLatchCheckEdit);
			this.Controls.Add(this.emergencyMainGateControlLabel);
			this.Controls.Add(this.emergencySwitchesLabel);
			this.Controls.Add(this.emergencyPulseGeneratorControlLabel);
			this.Controls.Add(this.emergencyHVControlLabel);
			this.Controls.Add(this.emergencyBeamControlLabel);
			this.Name = "EmergencyControls";
			this.Size = new System.Drawing.Size(209, 160);
			((System.ComponentModel.ISupportInitialize)(this.emergencyLatchCheckEdit.Properties)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private DevExpress.XtraEditors.LabelControl emergencyBeamLossControlLabel;
		private DevExpress.XtraEditors.LabelControl beamLossLabel;
		private DevExpress.XtraEditors.LabelControl emergencySwitchesLabel;
		private DevExpress.XtraEditors.LabelControl emergencyBeamControlLabel;
		private DevExpress.XtraEditors.CheckEdit emergencyLatchCheckEdit;
		private DevExpress.XtraEditors.LabelControl emergencyHVControlLabel;
		private DevExpress.XtraEditors.LabelControl emergencyPulseGeneratorControlLabel;
		private DevExpress.XtraEditors.LabelControl emergencyMainGateControlLabel;
		private DevExpress.XtraEditors.CheckButton beamCheckButton;
		private DevExpress.XtraEditors.CheckButton hVCheckButton;
		private DevExpress.XtraEditors.CheckButton gateCheckButton;
		private DevExpress.XtraEditors.CheckButton pulseGeneratorCheckButton;
	}
}
