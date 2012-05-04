namespace Epics.VirtualAccelerator.Forms
{
	partial class LegendForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.legendAbbreviationsLabel = new DevExpress.XtraEditors.LabelControl();
			this.legendLabel = new DevExpress.XtraEditors.LabelControl();
			this.SuspendLayout();
			// 
			// legendAbbreviationsLabel
			// 
			this.legendAbbreviationsLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.legendAbbreviationsLabel.Appearance.ForeColor = System.Drawing.Color.SaddleBrown;
			this.legendAbbreviationsLabel.Location = new System.Drawing.Point(12, 12);
			this.legendAbbreviationsLabel.Name = "legendAbbreviationsLabel";
			this.legendAbbreviationsLabel.Size = new System.Drawing.Size(27, 143);
			this.legendAbbreviationsLabel.TabIndex = 0;
			this.legendAbbreviationsLabel.Text = "BPM:\r\nCT:\r\nSM:\r\nBM:\r\nVA:\r\nAP:\r\nVS:\r\nLS:\r\nGV:\r\nSL:\r\nBN:";
			// 
			// legendLabel
			// 
			this.legendLabel.Location = new System.Drawing.Point(48, 12);
			this.legendLabel.Name = "legendLabel";
			this.legendLabel.Size = new System.Drawing.Size(105, 143);
			this.legendLabel.TabIndex = 1;
			this.legendLabel.Text = "Beam Position Monitor\r\nCurrent Transformer\r\nSteering Magnet\r\nBeam Profile Monitor" +
    "\r\nVacuum\r\nAperture\r\nView Screen\r\nLaser\r\nGate Valve\r\nSolenoid\r\nBuncher";
			// 
			// LegendForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(163, 273);
			this.Controls.Add(this.legendLabel);
			this.Controls.Add(this.legendAbbreviationsLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "LegendForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Machine Labels Legend";
			this.TopMost = true;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private DevExpress.XtraEditors.LabelControl legendAbbreviationsLabel;
		private DevExpress.XtraEditors.LabelControl legendLabel;
	}
}