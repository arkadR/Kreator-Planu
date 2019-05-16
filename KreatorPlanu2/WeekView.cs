using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace KreatorPlanu
{
	public partial class WeekView : Form
	{
        readonly List<Label> textLabels = new List<Label>();
        readonly List<Block> blocks = new List<Block>();

		public WeekView(List<Block> blockList)
		{
			InitializeComponent();

			for (int i = 0; i < 5; i++)
			{
                Label l = new Label
                {
                    Text = ((Day_name)i).ToString(),
                    Location = new Point(50 + i * 300, 0),
                    Font = new Font("Arial", 24, FontStyle.Bold),
                    AutoSize = true
                };
                panel1.Controls.Add(l);
				for (int j = 7; j <= 20; j++)
				{
					l = new Label
					{
						Text = (j < 10 ? "  " : "") + j.ToString() + ":00",
						Location = new Point(i * 300, (j - 6) * 60 - 5),
						AutoSize = true
					};
					textLabels.Add(l);
					panel1.Controls.Add(l);
				}
			}

			foreach (Block b in blockList)
			{
				Block block = new Block(b)
				{
					Left = Block.ToDayNumber(b.day) * 300 + 53,
					Width = 194,
					Top = (b.startTime.Hour - 6) * 60 + b.startTime.Minute
			    };
				block.FlatAppearance.BorderColor = b.Color_Deactivated;
				block.FlatAppearance.CheckedBackColor = b.Color_Deactivated;
				block.FlatAppearance.MouseDownBackColor = b.Color_Deactivated;
				block.FlatAppearance.MouseOverBackColor = b.Color_Deactivated;
				blocks.Add(block);
				panel1.Controls.Add(block);
			}
		}

		private void WeekView_Load(object sender, EventArgs e)
		{

		}

		private void Panel1_Paint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			Pen p = new Pen(Color.Gray, 1);
			for (int i = 0; i < 5; i++)
			{
				for (int j = 7; j <= 20; j++)
				{
					int y = (j - 6) * 60;
					int x = 30 + i * 300;
					g.DrawLine(p, x, y, x + 230, y);
				}
			}
			g.Dispose();
			base.OnPaint(e);
		}

		private void Button_saveImage_Click(object sender, EventArgs e)
		{
			Bitmap bmp = new Bitmap(panel1.Width, panel1.Height);
			using (SaveFileDialog sfd = new SaveFileDialog { Filter = "JPEG files (*.jpg)|*.jpg|All files (*.*)|*.*", RestoreDirectory = true })
			{
				if (sfd.ShowDialog() == DialogResult.OK)
				{
					button_saveImage.Hide();
					panel1.DrawToBitmap(bmp, new Rectangle(0, 0, panel1.Width, panel1.Height));
					button_saveImage.Show();
					bmp.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
				}
			}
		}
	}
}
