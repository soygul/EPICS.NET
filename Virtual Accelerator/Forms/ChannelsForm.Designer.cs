namespace Epics.VirtualAccelerator.Forms
{
	partial class ChannelsForm
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
			this.closeButton = new DevExpress.XtraEditors.SimpleButton();
			this.channelsTitleLabel = new DevExpress.XtraEditors.LabelControl();
			this.channelsLabel = new DevExpress.XtraEditors.LabelControl();
			this.channelsMemoEdit = new DevExpress.XtraEditors.MemoEdit();
			((System.ComponentModel.ISupportInitialize)(this.channelsMemoEdit.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// closeButton
			// 
			this.closeButton.Location = new System.Drawing.Point(169, 375);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(75, 23);
			this.closeButton.TabIndex = 0;
			this.closeButton.Text = "&Close";
			this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
			// 
			// channelsTitleLabel
			// 
			this.channelsTitleLabel.Location = new System.Drawing.Point(13, 13);
			this.channelsTitleLabel.Name = "channelsTitleLabel";
			this.channelsTitleLabel.Size = new System.Drawing.Size(142, 13);
			this.channelsTitleLabel.TabIndex = 1;
			this.channelsTitleLabel.Text = "Total channels being listened:";
			// 
			// channelsLabel
			// 
			this.channelsLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.channelsLabel.Location = new System.Drawing.Point(161, 13);
			this.channelsLabel.Name = "channelsLabel";
			this.channelsLabel.Size = new System.Drawing.Size(7, 13);
			this.channelsLabel.TabIndex = 2;
			this.channelsLabel.Text = "0";
			// 
			// channelsMemoEdit
			// 
			this.channelsMemoEdit.Location = new System.Drawing.Point(12, 32);
			this.channelsMemoEdit.Name = "channelsMemoEdit";
			this.channelsMemoEdit.Size = new System.Drawing.Size(232, 336);
			this.channelsMemoEdit.TabIndex = 3;
			// 
			// ChannelsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(256, 405);
			this.Controls.Add(this.channelsMemoEdit);
			this.Controls.Add(this.channelsLabel);
			this.Controls.Add(this.channelsTitleLabel);
			this.Controls.Add(this.closeButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "ChannelsForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Client Channels";
			((System.ComponentModel.ISupportInitialize)(this.channelsMemoEdit.Properties)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private DevExpress.XtraEditors.SimpleButton closeButton;
		private DevExpress.XtraEditors.LabelControl channelsTitleLabel;
		private DevExpress.XtraEditors.LabelControl channelsLabel;
		private DevExpress.XtraEditors.MemoEdit channelsMemoEdit;

	}
}