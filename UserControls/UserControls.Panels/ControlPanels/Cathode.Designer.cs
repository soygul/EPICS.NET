namespace Epics.UserControls.Panels.ControlPanels
{
	partial class Cathode
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
			this.cathodeGroup = new DevExpress.XtraEditors.GroupControl();
			this.cathodeCurrentTrackBarLabel = new DevExpress.XtraEditors.LabelControl();
			this.cathodeCurrentUnitLabel = new DevExpress.XtraEditors.LabelControl();
			this.cathodeTempUnitLabel = new DevExpress.XtraEditors.LabelControl();
			this.cathodeCurrentLabel = new DevExpress.XtraEditors.LabelControl();
			this.cathodeTempLabel = new DevExpress.XtraEditors.LabelControl();
			this.cathodeCurrentTrackBar = new DevExpress.XtraEditors.TrackBarControl();
			this.cathodeCurrentValueSpinEdit = new DevExpress.XtraEditors.SpinEdit();
			this.cathodeTempProgressBar = new DevExpress.XtraEditors.ProgressBarControl();
			this.cathodeTempValueLabel = new DevExpress.XtraEditors.LabelControl();
			((System.ComponentModel.ISupportInitialize)(this.cathodeGroup)).BeginInit();
			this.cathodeGroup.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cathodeCurrentTrackBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cathodeCurrentTrackBar.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cathodeCurrentValueSpinEdit.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cathodeTempProgressBar.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// cathodeGroup
			// 
			this.cathodeGroup.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cathodeGroup.AppearanceCaption.Options.UseFont = true;
			this.cathodeGroup.AppearanceCaption.Options.UseTextOptions = true;
			this.cathodeGroup.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.cathodeGroup.Controls.Add(this.cathodeCurrentTrackBarLabel);
			this.cathodeGroup.Controls.Add(this.cathodeCurrentUnitLabel);
			this.cathodeGroup.Controls.Add(this.cathodeTempUnitLabel);
			this.cathodeGroup.Controls.Add(this.cathodeCurrentLabel);
			this.cathodeGroup.Controls.Add(this.cathodeTempLabel);
			this.cathodeGroup.Controls.Add(this.cathodeCurrentTrackBar);
			this.cathodeGroup.Controls.Add(this.cathodeCurrentValueSpinEdit);
			this.cathodeGroup.Controls.Add(this.cathodeTempProgressBar);
			this.cathodeGroup.Controls.Add(this.cathodeTempValueLabel);
			this.cathodeGroup.Location = new System.Drawing.Point(0, 0);
			this.cathodeGroup.Name = "cathodeGroup";
			this.cathodeGroup.Size = new System.Drawing.Size(228, 173);
			this.cathodeGroup.TabIndex = 45;
			this.cathodeGroup.Text = "Cathode";
			// 
			// cathodeCurrentTrackBarLabel
			// 
			this.cathodeCurrentTrackBarLabel.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cathodeCurrentTrackBarLabel.Appearance.Options.UseFont = true;
			this.cathodeCurrentTrackBarLabel.Location = new System.Drawing.Point(7, 143);
			this.cathodeCurrentTrackBarLabel.Name = "cathodeCurrentTrackBarLabel";
			this.cathodeCurrentTrackBarLabel.Size = new System.Drawing.Size(217, 12);
			this.cathodeCurrentTrackBarLabel.TabIndex = 57;
			this.cathodeCurrentTrackBarLabel.Text = "  0   100 200  300  400 500  600 700  800 900 1000 Max";
			// 
			// cathodeCurrentUnitLabel
			// 
			this.cathodeCurrentUnitLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cathodeCurrentUnitLabel.Appearance.Options.UseFont = true;
			this.cathodeCurrentUnitLabel.Location = new System.Drawing.Point(203, 90);
			this.cathodeCurrentUnitLabel.Name = "cathodeCurrentUnitLabel";
			this.cathodeCurrentUnitLabel.Size = new System.Drawing.Size(13, 13);
			this.cathodeCurrentUnitLabel.TabIndex = 56;
			this.cathodeCurrentUnitLabel.Text = "µA";
			// 
			// cathodeTempUnitLabel
			// 
			this.cathodeTempUnitLabel.Location = new System.Drawing.Point(206, 33);
			this.cathodeTempUnitLabel.Name = "cathodeTempUnitLabel";
			this.cathodeTempUnitLabel.Size = new System.Drawing.Size(12, 13);
			this.cathodeTempUnitLabel.TabIndex = 56;
			this.cathodeTempUnitLabel.Text = "°C";
			// 
			// cathodeCurrentLabel
			// 
			this.cathodeCurrentLabel.Location = new System.Drawing.Point(5, 90);
			this.cathodeCurrentLabel.Name = "cathodeCurrentLabel";
			this.cathodeCurrentLabel.Size = new System.Drawing.Size(85, 13);
			this.cathodeCurrentLabel.TabIndex = 53;
			this.cathodeCurrentLabel.Text = "Cathode Current:";
			// 
			// cathodeTempLabel
			// 
			this.cathodeTempLabel.Location = new System.Drawing.Point(5, 33);
			this.cathodeTempLabel.Name = "cathodeTempLabel";
			this.cathodeTempLabel.Size = new System.Drawing.Size(110, 13);
			this.cathodeTempLabel.TabIndex = 52;
			this.cathodeTempLabel.Text = "Cathode Temperature:";
			// 
			// cathodeCurrentTrackBar
			// 
			this.cathodeCurrentTrackBar.EditValue = null;
			this.cathodeCurrentTrackBar.Location = new System.Drawing.Point(5, 113);
			this.cathodeCurrentTrackBar.Name = "cathodeCurrentTrackBar";
			this.cathodeCurrentTrackBar.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Buffered;
			this.cathodeCurrentTrackBar.Properties.LargeChange = 10;
			this.cathodeCurrentTrackBar.Properties.Maximum = 1100;
			this.cathodeCurrentTrackBar.Properties.ShowValueToolTip = true;
			this.cathodeCurrentTrackBar.Properties.Tag = "";
			this.cathodeCurrentTrackBar.Properties.TickFrequency = 100;
			this.cathodeCurrentTrackBar.Size = new System.Drawing.Size(218, 42);
			this.cathodeCurrentTrackBar.TabIndex = 51;
			// 
			// cathodeCurrentValueSpinEdit
			// 
			this.cathodeCurrentValueSpinEdit.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.cathodeCurrentValueSpinEdit.Location = new System.Drawing.Point(104, 87);
			this.cathodeCurrentValueSpinEdit.Name = "cathodeCurrentValueSpinEdit";
			this.cathodeCurrentValueSpinEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.cathodeCurrentValueSpinEdit.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
			this.cathodeCurrentValueSpinEdit.Size = new System.Drawing.Size(93, 20);
			this.cathodeCurrentValueSpinEdit.TabIndex = 50;
			// 
			// cathodeTempProgressBar
			// 
			this.cathodeTempProgressBar.Location = new System.Drawing.Point(5, 56);
			this.cathodeTempProgressBar.Name = "cathodeTempProgressBar";
			this.cathodeTempProgressBar.Properties.EndColor = System.Drawing.Color.Red;
			this.cathodeTempProgressBar.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
			this.cathodeTempProgressBar.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
			this.cathodeTempProgressBar.Properties.Maximum = 200;
			this.cathodeTempProgressBar.Properties.ProgressViewStyle = DevExpress.XtraEditors.Controls.ProgressViewStyle.Solid;
			this.cathodeTempProgressBar.Properties.ShowTitle = true;
			this.cathodeTempProgressBar.Properties.StartColor = System.Drawing.Color.Lime;
			this.cathodeTempProgressBar.Size = new System.Drawing.Size(218, 18);
			this.cathodeTempProgressBar.TabIndex = 49;
			// 
			// cathodeTempValueLabel
			// 
			this.cathodeTempValueLabel.Appearance.BackColor = System.Drawing.Color.White;
			this.cathodeTempValueLabel.Appearance.BorderColor = System.Drawing.Color.LightGray;
			this.cathodeTempValueLabel.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.cathodeTempValueLabel.Appearance.Options.UseBackColor = true;
			this.cathodeTempValueLabel.Appearance.Options.UseBorderColor = true;
			this.cathodeTempValueLabel.Appearance.Options.UseTextOptions = true;
			this.cathodeTempValueLabel.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
			this.cathodeTempValueLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.cathodeTempValueLabel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.cathodeTempValueLabel.Location = new System.Drawing.Point(123, 30);
			this.cathodeTempValueLabel.Name = "cathodeTempValueLabel";
			this.cathodeTempValueLabel.Padding = new System.Windows.Forms.Padding(2, 0, 3, 0);
			this.cathodeTempValueLabel.Size = new System.Drawing.Size(77, 20);
			this.cathodeTempValueLabel.TabIndex = 3;
			this.cathodeTempValueLabel.UseMnemonic = false;
			// 
			// Cathode
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.cathodeGroup);
			this.Name = "Cathode";
			this.Size = new System.Drawing.Size(228, 173);
			((System.ComponentModel.ISupportInitialize)(this.cathodeGroup)).EndInit();
			this.cathodeGroup.ResumeLayout(false);
			this.cathodeGroup.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.cathodeCurrentTrackBar.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cathodeCurrentTrackBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cathodeCurrentValueSpinEdit.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cathodeTempProgressBar.Properties)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.GroupControl cathodeGroup;
		private DevExpress.XtraEditors.LabelControl cathodeCurrentTrackBarLabel;
		private DevExpress.XtraEditors.LabelControl cathodeCurrentUnitLabel;
		private DevExpress.XtraEditors.LabelControl cathodeTempUnitLabel;
		private DevExpress.XtraEditors.LabelControl cathodeCurrentLabel;
		private DevExpress.XtraEditors.LabelControl cathodeTempLabel;
		private DevExpress.XtraEditors.TrackBarControl cathodeCurrentTrackBar;
		private DevExpress.XtraEditors.SpinEdit cathodeCurrentValueSpinEdit;
		private DevExpress.XtraEditors.ProgressBarControl cathodeTempProgressBar;
		private DevExpress.XtraEditors.LabelControl cathodeTempValueLabel;
	}
}
