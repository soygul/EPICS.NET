namespace Epics.UserControls.Panels.ControlPanels.Parts
{
	partial class Vacuum
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Vacuum));
			this.vAPressureValueLabel = new DevExpress.XtraEditors.LabelControl();
			this.smallIconsImageCollection = new DevExpress.Utils.ImageCollection(this.components);
			this.vAPressureUnitLabel = new DevExpress.XtraEditors.LabelControl();
			this.vAPressureLabel = new DevExpress.XtraEditors.LabelControl();
			((System.ComponentModel.ISupportInitialize)(this.smallIconsImageCollection)).BeginInit();
			this.SuspendLayout();
			// 
			// vAPressureValueLabel
			// 
			this.vAPressureValueLabel.Appearance.BackColor = System.Drawing.Color.White;
			this.vAPressureValueLabel.Appearance.BorderColor = System.Drawing.Color.LightGray;
			this.vAPressureValueLabel.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.vAPressureValueLabel.Appearance.ImageList = this.smallIconsImageCollection;
			this.vAPressureValueLabel.Appearance.Options.UseBackColor = true;
			this.vAPressureValueLabel.Appearance.Options.UseBorderColor = true;
			this.vAPressureValueLabel.Appearance.Options.UseTextOptions = true;
			this.vAPressureValueLabel.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.vAPressureValueLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.vAPressureValueLabel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.vAPressureValueLabel.Location = new System.Drawing.Point(109, 0);
			this.vAPressureValueLabel.Name = "vAPressureValueLabel";
			this.vAPressureValueLabel.Padding = new System.Windows.Forms.Padding(2, 0, 3, 0);
			this.vAPressureValueLabel.Size = new System.Drawing.Size(77, 20);
			this.vAPressureValueLabel.TabIndex = 5;
			this.vAPressureValueLabel.UseMnemonic = false;
			// 
			// smallIconsImageCollection
			// 
			this.smallIconsImageCollection.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("smallIconsImageCollection.ImageStream")));
			this.smallIconsImageCollection.Images.SetKeyName(0, "led_hollow_small.png");
			this.smallIconsImageCollection.Images.SetKeyName(1, "led_blue_small.png");
			this.smallIconsImageCollection.Images.SetKeyName(2, "led_red_small.png");
			// 
			// vAPressureUnitLabel
			// 
			this.vAPressureUnitLabel.Location = new System.Drawing.Point(192, 3);
			this.vAPressureUnitLabel.Name = "vAPressureUnitLabel";
			this.vAPressureUnitLabel.Size = new System.Drawing.Size(22, 13);
			this.vAPressureUnitLabel.TabIndex = 4;
			this.vAPressureUnitLabel.Text = "nbar";
			// 
			// vAPressureLabel
			// 
			this.vAPressureLabel.Location = new System.Drawing.Point(3, 3);
			this.vAPressureLabel.Name = "vAPressureLabel";
			this.vAPressureLabel.Size = new System.Drawing.Size(78, 13);
			this.vAPressureLabel.TabIndex = 3;
			this.vAPressureLabel.Text = "VA.01 Pressure:";
			// 
			// Vacuum
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.vAPressureValueLabel);
			this.Controls.Add(this.vAPressureUnitLabel);
			this.Controls.Add(this.vAPressureLabel);
			this.Name = "Vacuum";
			this.Size = new System.Drawing.Size(220, 20);
			((System.ComponentModel.ISupportInitialize)(this.smallIconsImageCollection)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private DevExpress.XtraEditors.LabelControl vAPressureValueLabel;
		private DevExpress.XtraEditors.LabelControl vAPressureUnitLabel;
		private DevExpress.XtraEditors.LabelControl vAPressureLabel;
		private DevExpress.Utils.ImageCollection smallIconsImageCollection;
	}
}
