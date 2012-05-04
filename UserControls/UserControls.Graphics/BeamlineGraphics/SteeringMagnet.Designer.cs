namespace Epics.UserControls.Graphics.BeamlineGraphics
{
	partial class SteeringMagnet
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SteeringMagnet));
			this.topIconPictureBox = new System.Windows.Forms.PictureBox();
			this.bottomIconPictureBox = new System.Windows.Forms.PictureBox();
			this.emptyPanel = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.topIconPictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bottomIconPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// topIconPictureBox
			// 
			this.topIconPictureBox.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.topIconPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("topIconPictureBox.Image")));
			this.topIconPictureBox.Location = new System.Drawing.Point(0, 0);
			this.topIconPictureBox.Name = "topIconPictureBox";
			this.topIconPictureBox.Size = new System.Drawing.Size(30, 7);
			this.topIconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.topIconPictureBox.TabIndex = 0;
			this.topIconPictureBox.TabStop = false;
			// 
			// bottomIconPictureBox
			// 
			this.bottomIconPictureBox.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomIconPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("bottomIconPictureBox.Image")));
			this.bottomIconPictureBox.Location = new System.Drawing.Point(0, 20);
			this.bottomIconPictureBox.Name = "bottomIconPictureBox";
			this.bottomIconPictureBox.Size = new System.Drawing.Size(30, 7);
			this.bottomIconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.bottomIconPictureBox.TabIndex = 1;
			this.bottomIconPictureBox.TabStop = false;
			// 
			// emptyPanel
			// 
			this.emptyPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.emptyPanel.Location = new System.Drawing.Point(0, 7);
			this.emptyPanel.Name = "emptyPanel";
			this.emptyPanel.Size = new System.Drawing.Size(30, 13);
			this.emptyPanel.TabIndex = 3;
			// 
			// SteeringMagnet
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.topIconPictureBox);
			this.Controls.Add(this.emptyPanel);
			this.Controls.Add(this.bottomIconPictureBox);
			this.Name = "SteeringMagnet";
			this.Size = new System.Drawing.Size(30, 27);
			((System.ComponentModel.ISupportInitialize)(this.topIconPictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bottomIconPictureBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox topIconPictureBox;
		private System.Windows.Forms.PictureBox bottomIconPictureBox;
		private System.Windows.Forms.Panel emptyPanel;
	}
}
