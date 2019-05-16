using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using KreatorPlanu.Properties;
using System.Diagnostics;

namespace KreatorPlanu
{
	public partial class Form1 : Form
	{
		public static int brickWidth = 150;
        readonly DataBase data = new DataBase();
        readonly List<Label> textLabels = new List<Label>();
		bool loaded = false;
		string sheetsUrl;

        public Form1()
		{
			InitializeComponent();

			//Add previous link to textBox
			if (Settings.Default.urlSheets != null)
			{
				textBox_GoogleLink.Text = Settings.Default.urlSheets;
			}
		}

		private void ArrangeData()
		{
			//Activate Side panel
			panelLeft.Visible = true;

			//Create panels for the list of subjects
			foreach (Subject s in data.subjects.Values)
			{
				panelLeft.Controls.Add(s);
				panelLeft.Controls.Add(s.checkBox);
			}

			//Fix blocks
			for (int i = 0; i < data.blocks.Count; i++)
			{
				bool intersects;
				do
				{
					intersects = false;
					for (int j = 0; j < i; j++)
					{
						if (data.blocks[j].Intersects(data.blocks[i]))
						{
							data.blocks[i].Left += brickWidth;
							intersects = true;
							break;
						}
					}
				}
				while (intersects);
			}

            //Add buttons to the panel
            data.blocks.ForEach(b => panel3.Controls.Add(b));

			for (int i = 0; i < 5; i++)
			{
				Label l = new Label
				{
					Text = ((Day_name)i).ToString(),
					Location = new Point(500, i * 900),
					Font = new Font("Arial", 24, FontStyle.Bold),
					AutoSize = true
				};
				panel3.Controls.Add(l);
				for (int j = 7; j <= 20; j++)
				{
					l = new Label
					{
						Text = (j < 10 ? "  " : "") + j.ToString() + ":00",
						Location = new Point(0, i * 900 + (j - 6) * 60 - 5),
						AutoSize = true
					};
					textLabels.Add(l);
					panel3.Controls.Add(l);
				}
			}
			int width = panelLeft.Bounds.Width;
			panelLeft.Width = width;
			panelLeft.AutoSize = false;
			panelLeft.Width = width;

		}

		private void Panel3_Paint(object sender, PaintEventArgs e)
		{
			if (loaded)
			{
				//DRAW HORIZONTAL LINES
				e.Graphics.TranslateTransform(panel3.AutoScrollPosition.X, panel3.AutoScrollPosition.Y);
				Graphics g = e.Graphics;
				Pen p = new Pen(Color.Gray, 1);
				for (int i = 0; i < 5; i++)
				{
					for (int j = 7; j <= 20; j++)
					{
						int y = i * 900 + (j - 6) * 60;
						g.DrawLine(p, 0, y, 5000, y);
					}
				}
				g.Dispose();
			}
			base.OnPaint(e);
		}

		private void Button_generate_Click(object sender, EventArgs e) //GENERATE THE CODES AND INSERT THEM TO TEXT BOX
		{
			List<Block> enabledBlocks = new List<Block>();
			textBox_codes.Text = "";
			foreach(Block b in data.blocks)
			{
				if (b.activated)
				{
					enabledBlocks.Add(b);
				}
			}
			if (checkBox1.Checked)
			{
				enabledBlocks.Sort((block1, block2) => (block1.group_capacity - block1.group_taken).CompareTo(block2.group_capacity - block2.group_taken));
			}
			foreach(Block b in enabledBlocks)
			{
				textBox_codes.Text += b.code + Environment.NewLine;
			}
		}

		private void Button_export_Click(object sender, EventArgs e)
		{
			Stream stream;
			using (SaveFileDialog sfd = new SaveFileDialog { Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*", RestoreDirectory = true, FileName = "kody"})
			{
				if (sfd.ShowDialog() == DialogResult.OK)
				{
					if ((stream = sfd.OpenFile()) != null)
					{
						StreamWriter writer = new StreamWriter(stream);
						List<Block> enabledBlocks = new List<Block>();
						foreach (Block b in data.blocks)
						{
							if (b.activated)
							{
								enabledBlocks.Add(b);
							}
						}
						if (checkBox1.Checked)
						{
							enabledBlocks.Sort((block1, block2) => (block1.group_capacity - block1.group_taken).CompareTo(block2.group_capacity - block2.group_taken));
						}
						foreach (Block b in enabledBlocks)
						{
							writer.WriteLine(b.code);
						}
						label_debug.Text = string.Format("Pomyślnie zapisano do {0}", sfd.FileName);
						writer.Close();
						stream.Close();
					}
				}
			}
		}

		private void Button_import_Click(object sender, EventArgs e)
		{
			string dir = "";

			using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*", Multiselect = false, RestoreDirectory = true })
			{
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					dir = ofd.FileName;
				}
				else
				{
					return;
				}
			}
            data.blocks.ForEach(b => b.OnDeactivate());
			int ile = 0;
			foreach (string line in File.ReadLines(dir))
			{
				foreach (Block b in data.blocks)
				{
					if (b.code.Equals(line))
					{
						ile++;
						b.OnActivate();
						break;
					}
				}
			}
			label_debug.Text = string.Format("Pomyślnie zaimportowano {0} grup zajęciowych", ile);
		}

		private void Button_showTable_Click(object sender, EventArgs e)
		{
			List<Block> activeBlocks = new List<Block>();
			foreach (Block b in data.blocks)
			{
				if (b.activated)
				{
					activeBlocks.Add(b);
				}
			}
			Form week = new WeekView(activeBlocks);
			week.Show();
		}

		private void Button_loadCSV_Click(object sender, EventArgs e)
		{
			string dir;
			
			using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "CSV files (*.csv)|*.csv", Multiselect = false, RestoreDirectory = true })
			{
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					dir = ofd.FileName;
				}
				else
				{
					return;
				}
			}
			
			data.LoadFromCSV(dir);
			loaded = true;
			tableLayoutPanel3.Dispose();
			ArrangeData();
		}

		private void Button_loadFromGoogle_Click(object sender, EventArgs e)
		{
			string url = textBox_GoogleLink.Text;
			if (!url.Contains("https://docs.google.com/spreadsheets/d/"))
			{
				MessageBox.Show("Niepoprawny adres url. Przykładowy adres to: https://docs.google.com/spreadsheets/d/1Pw62XZBYuGd1sN8KsisPP2ReAcs503V1ijONnnA4uWM");
				return;
			}
			string parsedUrl = url.Replace("https://docs.google.com/spreadsheets/d/", "");
			if (parsedUrl.IndexOf("/") > 0)
				parsedUrl = parsedUrl.Remove(parsedUrl.IndexOf("/"));
			parsedUrl = "https://spreadsheets.google.com/tq?key=" + parsedUrl;
			sheetsUrl = parsedUrl;
			Debug.Write(parsedUrl);
			if (!data.LoadFromSheets(parsedUrl))
			{
				MessageBox.Show("Niepoprawny adres url. Przykładowy adres to: https://docs.google.com/spreadsheets/d/1Pw62XZBYuGd1sN8KsisPP2ReAcs503V1ijONnnA4uWM");
				return;
			}
			Settings.Default.urlSheets = url;
			Button button_refresh = new Button
			{
				Text = "Odśwież",
				AutoSize = true,
				Location = new Point(5, 5),
				FlatStyle = FlatStyle.Flat
			};
			button_refresh.Click += Button_refresh_Click;
			panel3.Controls.Add(button_refresh);
			loaded = true;
			tableLayoutPanel3.Dispose();
			ArrangeData();
		}

		private void Button_refresh_Click(object sender, EventArgs e) 
		{
			List<string> codes = new List<string>();
			List<string> hiddenCodes = new List<string>();
			foreach (Block b in data.blocks)
			{
				if (b.activated)
				{
					codes.Add(b.code);
				}
			}
			foreach (Subject sub in data.subjects.Values)
			{
				panelLeft.Controls.RemoveByKey(sub.Text);
				panelLeft.Controls.RemoveByKey(sub.Text + "checkBox");
				if (!sub.checkBox.Checked)
				{
					hiddenCodes.Add(sub.Type.ToString() + sub.name);
				}
				sub.checkBox.Dispose();
				sub.Dispose();
			}

			foreach (Block b in data.blocks)
			{
				panel3.Controls.RemoveByKey(b.Name);
				b.Dispose();
			}
			data.Clear();
			data.LoadFromSheets(sheetsUrl);
			ArrangeData();
			foreach (string s in hiddenCodes)
			{
				data.subjects[s].checkBox.Checked = false;
                data.subjects[s].blocks.ForEach(b => b.Fade());
			}
			foreach (string s in codes)
			{
				foreach (Block b in data.blocks)
				{
					if (b.code.Equals(s) && b.Enabled)
					{
						b.OnActivate();
						break;
					}
				}
			}

		}

		private void Form1_Load(object sender, EventArgs e)
		{
			// Set window location
			if (Settings.Default.WindowLocation != null)
			{
				this.Location = Settings.Default.WindowLocation;
			}

			// Set window size
			if (Settings.Default.WindowSize != null)
			{
				this.Size = Settings.Default.WindowSize;
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			// Copy window location to app settings
			Settings.Default.WindowLocation = this.Location;

			// Copy window size to app settings
			if (this.WindowState == FormWindowState.Normal)
			{
				Settings.Default.WindowSize = this.Size;
			}
			else
			{
				Settings.Default.WindowSize = this.RestoreBounds.Size;
			}

			// Save settings
			Settings.Default.Save();
		}
	}

	public enum Class_type
	{
		W, C, L, P, S, I
	}

	public enum Day_name
	{
		Poniedziałek, Wtorek, Środa, Czwartek, Piątek
	}

	public enum Week_type
	{
		TP, TN, O
	}
	
}
