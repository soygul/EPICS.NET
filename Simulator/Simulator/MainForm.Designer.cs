namespace Epics.Simulator
{
	partial class MainForm
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.simulatorTimeTitleLabel = new System.Windows.Forms.Label();
			this.titleBale = new System.Windows.Forms.Label();
			this.simulatorTimeLabel = new System.Windows.Forms.Label();
			this.simulatedChannelsTitleLabel = new System.Windows.Forms.Label();
			this.viewChannelsButton = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.hideButton = new System.Windows.Forms.Button();
			this.exitButton = new System.Windows.Forms.Button();
			this.startStopButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// notifyIcon
			// 
			this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
			this.notifyIcon.Text = "EPICS .NET Simulator";
			this.notifyIcon.Visible = true;
			this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseDoubleClick);
			// 
			// simulatorTimeTitleLabel
			// 
			this.simulatorTimeTitleLabel.AutoSize = true;
			this.simulatorTimeTitleLabel.Location = new System.Drawing.Point(70, 45);
			this.simulatorTimeTitleLabel.Name = "simulatorTimeTitleLabel";
			this.simulatorTimeTitleLabel.Size = new System.Drawing.Size(75, 13);
			this.simulatorTimeTitleLabel.TabIndex = 4;
			this.simulatorTimeTitleLabel.Text = "Simulator time:";
			// 
			// titleBale
			// 
			this.titleBale.AutoSize = true;
			this.titleBale.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.titleBale.Location = new System.Drawing.Point(66, 13);
			this.titleBale.Name = "titleBale";
			this.titleBale.Size = new System.Drawing.Size(277, 24);
			this.titleBale.TabIndex = 5;
			this.titleBale.Text = "T.A.C. Control System Simulator";
			// 
			// simulatorTimeLabel
			// 
			this.simulatorTimeLabel.AutoSize = true;
			this.simulatorTimeLabel.Location = new System.Drawing.Point(167, 45);
			this.simulatorTimeLabel.Name = "simulatorTimeLabel";
			this.simulatorTimeLabel.Size = new System.Drawing.Size(67, 13);
			this.simulatorTimeLabel.TabIndex = 6;
			this.simulatorTimeLabel.Text = "0.0 Seconds";
			// 
			// simulatedChannelsTitleLabel
			// 
			this.simulatedChannelsTitleLabel.AutoSize = true;
			this.simulatedChannelsTitleLabel.Location = new System.Drawing.Point(70, 68);
			this.simulatedChannelsTitleLabel.Name = "simulatedChannelsTitleLabel";
			this.simulatedChannelsTitleLabel.Size = new System.Drawing.Size(91, 13);
			this.simulatedChannelsTitleLabel.TabIndex = 8;
			this.simulatedChannelsTitleLabel.Text = "Simulated signals:";
			// 
			// viewChannelsButton
			// 
			this.viewChannelsButton.Image = global::Epics.Simulator.Properties.Resources.zoom_16;
			this.viewChannelsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.viewChannelsButton.Location = new System.Drawing.Point(170, 64);
			this.viewChannelsButton.Name = "viewChannelsButton";
			this.viewChannelsButton.Size = new System.Drawing.Size(57, 21);
			this.viewChannelsButton.TabIndex = 7;
			this.viewChannelsButton.Text = "&View";
			this.viewChannelsButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.viewChannelsButton.UseVisualStyleBackColor = true;
			this.viewChannelsButton.Click += new System.EventHandler(this.ViewChannelsButton_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::Epics.Simulator.Properties.Resources.Simulator_Icon_48;
			this.pictureBox1.Location = new System.Drawing.Point(12, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(48, 48);
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			// 
			// hideButton
			// 
			this.hideButton.Image = global::Epics.Simulator.Properties.Resources.hide_16;
			this.hideButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.hideButton.Location = new System.Drawing.Point(272, 127);
			this.hideButton.Name = "hideButton";
			this.hideButton.Size = new System.Drawing.Size(75, 23);
			this.hideButton.TabIndex = 2;
			this.hideButton.Text = "&Hide";
			this.hideButton.UseVisualStyleBackColor = true;
			this.hideButton.Click += new System.EventHandler(this.HideButton_Click);
			// 
			// exitButton
			// 
			this.exitButton.Image = global::Epics.Simulator.Properties.Resources.exit_16;
			this.exitButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.exitButton.Location = new System.Drawing.Point(188, 127);
			this.exitButton.Name = "exitButton";
			this.exitButton.Size = new System.Drawing.Size(78, 23);
			this.exitButton.TabIndex = 1;
			this.exitButton.Text = "&Exit";
			this.exitButton.UseVisualStyleBackColor = true;
			this.exitButton.Click += new System.EventHandler(this.ExitButton_Click);
			// 
			// startStopButton
			// 
			this.startStopButton.Image = global::Epics.Simulator.Properties.Resources.play_16;
			this.startStopButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.startStopButton.Location = new System.Drawing.Point(12, 127);
			this.startStopButton.Name = "startStopButton";
			this.startStopButton.Size = new System.Drawing.Size(86, 23);
			this.startStopButton.TabIndex = 0;
			this.startStopButton.Text = "&Start";
			this.startStopButton.UseVisualStyleBackColor = true;
			this.startStopButton.Click += new System.EventHandler(this.StartStopButton_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(359, 162);
			this.Controls.Add(this.simulatedChannelsTitleLabel);
			this.Controls.Add(this.viewChannelsButton);
			this.Controls.Add(this.simulatorTimeLabel);
			this.Controls.Add(this.titleBale);
			this.Controls.Add(this.simulatorTimeTitleLabel);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.hideButton);
			this.Controls.Add(this.exitButton);
			this.Controls.Add(this.startStopButton);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Turkish Accelerator Center - CS Simulator";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.Button startStopButton;
		private System.Windows.Forms.Button exitButton;
		private System.Windows.Forms.Button hideButton;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label simulatorTimeTitleLabel;
		private System.Windows.Forms.Label titleBale;
		private System.Windows.Forms.Label simulatorTimeLabel;
		private System.Windows.Forms.Button viewChannelsButton;
		private System.Windows.Forms.Label simulatedChannelsTitleLabel;
	}
}