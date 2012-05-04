namespace Epics.UserControls.Panels.ControlPanels
{
	partial class Vacuums
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
			this.VacuumGroup = new DevExpress.XtraEditors.GroupControl();
			this.vacuum3 = new Epics.UserControls.Panels.ControlPanels.Parts.Vacuum();
			this.vacuum2 = new Epics.UserControls.Panels.ControlPanels.Parts.Vacuum();
			this.vacuum1 = new Epics.UserControls.Panels.ControlPanels.Parts.Vacuum();
			((System.ComponentModel.ISupportInitialize)(this.VacuumGroup)).BeginInit();
			this.VacuumGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// VacuumGroup
			// 
			this.VacuumGroup.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
			this.VacuumGroup.AppearanceCaption.Options.UseFont = true;
			this.VacuumGroup.AppearanceCaption.Options.UseTextOptions = true;
			this.VacuumGroup.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.VacuumGroup.Controls.Add(this.vacuum3);
			this.VacuumGroup.Controls.Add(this.vacuum2);
			this.VacuumGroup.Controls.Add(this.vacuum1);
			this.VacuumGroup.Location = new System.Drawing.Point(0, 0);
			this.VacuumGroup.Name = "VacuumGroup";
			this.VacuumGroup.Size = new System.Drawing.Size(228, 122);
			this.VacuumGroup.TabIndex = 57;
			this.VacuumGroup.Text = "Vacuum";
			// 
			// vacuum3
			// 
			this.vacuum3.Location = new System.Drawing.Point(5, 86);
			this.vacuum3.Name = "vacuum3";
			this.vacuum3.SectorId = 3;
			this.vacuum3.Size = new System.Drawing.Size(220, 20);
			this.vacuum3.TabIndex = 2;
			// 
			// vacuum2
			// 
			this.vacuum2.Location = new System.Drawing.Point(5, 59);
			this.vacuum2.Name = "vacuum2";
			this.vacuum2.SectorId = 2;
			this.vacuum2.Size = new System.Drawing.Size(220, 20);
			this.vacuum2.TabIndex = 1;
			// 
			// vacuum1
			// 
			this.vacuum1.Location = new System.Drawing.Point(5, 32);
			this.vacuum1.Name = "vacuum1";
			this.vacuum1.SectorId = 1;
			this.vacuum1.Size = new System.Drawing.Size(220, 20);
			this.vacuum1.TabIndex = 0;
			// 
			// Vacuums
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.VacuumGroup);
			this.Name = "Vacuums";
			this.Size = new System.Drawing.Size(228, 122);
			((System.ComponentModel.ISupportInitialize)(this.VacuumGroup)).EndInit();
			this.VacuumGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.GroupControl VacuumGroup;
		private Parts.Vacuum vacuum1;
		private Parts.Vacuum vacuum3;
		private Parts.Vacuum vacuum2;
	}
}
