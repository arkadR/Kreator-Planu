namespace KreatorPlanu
{
	partial class WeekView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WeekView));
			this.panel1 = new System.Windows.Forms.Panel();
			this.button_saveImage = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.button_saveImage);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1484, 876);
			this.panel1.TabIndex = 0;
			this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel1_Paint);
			// 
			// button_saveImage
			// 
			this.button_saveImage.AutoSize = true;
			this.button_saveImage.Cursor = System.Windows.Forms.Cursors.Hand;
			this.button_saveImage.Location = new System.Drawing.Point(1397, 3);
			this.button_saveImage.Name = "button_saveImage";
			this.button_saveImage.Size = new System.Drawing.Size(75, 23);
			this.button_saveImage.TabIndex = 0;
			this.button_saveImage.Text = "Zapisz";
			this.button_saveImage.UseVisualStyleBackColor = true;
			this.button_saveImage.Click += new System.EventHandler(this.Button_saveImage_Click);
			// 
			// WeekView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1484, 876);
			this.Controls.Add(this.panel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximumSize = new System.Drawing.Size(1500, 915);
			this.MinimumSize = new System.Drawing.Size(800, 450);
			this.Name = "WeekView";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Widok tygodnia";
			this.Load += new System.EventHandler(this.WeekView_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button button_saveImage;
	}
}