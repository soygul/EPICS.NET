namespace Epics.ChannelAccess.Provider
{
	partial class ProviderSelector
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
			this.selectButton = new System.Windows.Forms.Button();
			this.providersComboBox = new System.Windows.Forms.ComboBox();
			this.selectLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// selectButton
			// 
			this.selectButton.Location = new System.Drawing.Point(224, 34);
			this.selectButton.Name = "selectButton";
			this.selectButton.Size = new System.Drawing.Size(93, 23);
			this.selectButton.TabIndex = 0;
			this.selectButton.Text = "&Select && Close";
			this.selectButton.UseVisualStyleBackColor = true;
			this.selectButton.Click += new System.EventHandler(this.SelectButton_Click);
			// 
			// providersComboBox
			// 
			this.providersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.providersComboBox.FormattingEnabled = true;
			this.providersComboBox.Location = new System.Drawing.Point(12, 34);
			this.providersComboBox.Name = "providersComboBox";
			this.providersComboBox.Size = new System.Drawing.Size(186, 21);
			this.providersComboBox.TabIndex = 1;
			// 
			// selectLabel
			// 
			this.selectLabel.AutoSize = true;
			this.selectLabel.Location = new System.Drawing.Point(12, 9);
			this.selectLabel.Name = "selectLabel";
			this.selectLabel.Size = new System.Drawing.Size(246, 13);
			this.selectLabel.TabIndex = 2;
			this.selectLabel.Text = "Please select one of the available providers below:";
			// 
			// ProviderSelector
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(329, 75);
			this.Controls.Add(this.selectLabel);
			this.Controls.Add(this.providersComboBox);
			this.Controls.Add(this.selectButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "ProviderSelector";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Channel access provider selector:";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button selectButton;
		private System.Windows.Forms.ComboBox providersComboBox;
		private System.Windows.Forms.Label selectLabel;
	}
}