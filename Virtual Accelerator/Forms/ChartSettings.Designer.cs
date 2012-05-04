namespace Epics.VirtualAccelerator.Forms
{
	partial class ChartSettings
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
			this.okButton = new DevExpress.XtraEditors.SimpleButton();
			this.updateIntervalLabel = new DevExpress.XtraEditors.LabelControl();
			this.updateIntervalSpinEdit = new DevExpress.XtraEditors.SpinEdit();
			((System.ComponentModel.ISupportInitialize)(this.updateIntervalSpinEdit.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(151, 49);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 0;
			this.okButton.Text = "&OK";
			this.okButton.Click += new System.EventHandler(this.OKButton_Click);
			// 
			// updateIntervalLabel
			// 
			this.updateIntervalLabel.Location = new System.Drawing.Point(13, 13);
			this.updateIntervalLabel.Name = "updateIntervalLabel";
			this.updateIntervalLabel.Size = new System.Drawing.Size(107, 13);
			this.updateIntervalLabel.TabIndex = 1;
			this.updateIntervalLabel.Text = "Update Interval (ms): ";
			// 
			// updateIntervalSpinEdit
			// 
			this.updateIntervalSpinEdit.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.updateIntervalSpinEdit.Location = new System.Drawing.Point(126, 10);
			this.updateIntervalSpinEdit.Name = "updateIntervalSpinEdit";
			this.updateIntervalSpinEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.updateIntervalSpinEdit.Size = new System.Drawing.Size(100, 20);
			this.updateIntervalSpinEdit.TabIndex = 2;
			// 
			// ChartSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(238, 83);
			this.Controls.Add(this.updateIntervalSpinEdit);
			this.Controls.Add(this.updateIntervalLabel);
			this.Controls.Add(this.okButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "ChartSettings";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Chart Settings";
			((System.ComponentModel.ISupportInitialize)(this.updateIntervalSpinEdit.Properties)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private DevExpress.XtraEditors.SimpleButton okButton;
		private DevExpress.XtraEditors.LabelControl updateIntervalLabel;
		private DevExpress.XtraEditors.SpinEdit updateIntervalSpinEdit;
	}
}