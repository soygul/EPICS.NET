namespace Epics.Simulator
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
			this.closeButton = new System.Windows.Forms.Button();
			this.channelsTextBox = new System.Windows.Forms.TextBox();
			this.channelsTitleLabel = new System.Windows.Forms.Label();
			this.channelsLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// closeButton
			// 
			this.closeButton.Location = new System.Drawing.Point(200, 294);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(75, 23);
			this.closeButton.TabIndex = 0;
			this.closeButton.Text = "&Close";
			this.closeButton.UseVisualStyleBackColor = true;
			this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
			// 
			// channelsTextBox
			// 
			this.channelsTextBox.Location = new System.Drawing.Point(12, 30);
			this.channelsTextBox.Multiline = true;
			this.channelsTextBox.Name = "channelsTextBox";
			this.channelsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.channelsTextBox.Size = new System.Drawing.Size(263, 258);
			this.channelsTextBox.TabIndex = 1;
			// 
			// channelsTitleLabel
			// 
			this.channelsTitleLabel.AutoSize = true;
			this.channelsTitleLabel.Location = new System.Drawing.Point(12, 9);
			this.channelsTitleLabel.Name = "channelsTitleLabel";
			this.channelsTitleLabel.Size = new System.Drawing.Size(127, 13);
			this.channelsTitleLabel.TabIndex = 2;
			this.channelsTitleLabel.Text = "Total simulated channels:";
			// 
			// channelsLabel
			// 
			this.channelsLabel.AutoSize = true;
			this.channelsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.channelsLabel.Location = new System.Drawing.Point(146, 9);
			this.channelsLabel.Name = "channelsLabel";
			this.channelsLabel.Size = new System.Drawing.Size(14, 13);
			this.channelsLabel.TabIndex = 3;
			this.channelsLabel.Text = "0";
			// 
			// ChannelsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(287, 322);
			this.Controls.Add(this.channelsLabel);
			this.Controls.Add(this.channelsTitleLabel);
			this.Controls.Add(this.channelsTextBox);
			this.Controls.Add(this.closeButton);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "ChannelsForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Simulated Channels";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.TextBox channelsTextBox;
		private System.Windows.Forms.Label channelsTitleLabel;
		private System.Windows.Forms.Label channelsLabel;
	}
}