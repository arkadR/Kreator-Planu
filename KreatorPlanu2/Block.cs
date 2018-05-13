using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace KreatorPlanu
{
	public class Block : Button, IComparable
	{
		State state = State.deactivated;

		public DataBase data;

		public string code;
		public string name;
		public Class_type type = Class_type.I;
		public string teacher;
		public string day;
		public Week_type week;
		public Time startTime;
		public Time endTime;
		public string clump_type;
		public int clump_number;
		public int group_capacity = 9999;
		public int group_taken;

		public bool activated = false;
		public Color Color_Activated;
		public Color Color_Deactivated;
		public Color Color_Grayed;

		Font font = new Font(DefaultFont, FontStyle.Regular);



		public string TextMain { get; set; }
		public string TextType { get; set; }
		public string TextWeek { get; set; }
		public string TextBottLeft { get; set; }


		public int SpotsLeft { get { return group_capacity - group_taken; } }
		public string BlockID { get	{ return type.ToString() + name; } }


		public Block()
		{
			UseVisualStyleBackColor = false;
			TextImageRelation = TextImageRelation.ImageAboveText;
		}
		public Block(Block other)
		{
			TextMain = other.TextMain;
			TextType = other.TextType;
			TextWeek = other.TextWeek;
			TextBottLeft = other.TextBottLeft;
			Location = other.Location;
			BackColor = other.Color_Deactivated;
			FlatStyle = other.FlatStyle;
			FlatAppearance.BorderColor = other.FlatAppearance.BorderColor;
			Size = other.Size;
		}
		public Block(string[] values, DataBase d)
		{
			FlatStyle = FlatStyle.Flat;
			code = values[0];
			name = values[2];
			if ("WCLPS".Contains(values[1].ToUpper()))
				type = (Class_type) Enum.Parse(typeof(Class_type), values[1].ToUpper());
			teacher = values[3];
			day = values[4].ToLower();
			week = (values[7].ToUpper().Equals("TP") ? Week_type.TP : (values[7].ToUpper().Equals("TN") ? Week_type.TN : Week_type.O));
			startTime = new Time(values[5]);
			endTime = new Time(values[6]);
			clump_type = values[8];
			if (values.Length > 9 && values[9] != null)
			{
				if (values[9].Length == 0)
					clump_number = 0;
				else
					clump_number = int.Parse(values[9]);
			}
			if (values.Length > 10 && values[10] != null)
			{
				if (values[10].Length == 0)
					group_capacity = 9999;
				else
					group_capacity = int.Parse(values[10]);
			}
			if (values.Length > 11 && values[11] != null)
			{
				if (values[11].Length == 0)
					group_taken = 0;
				else
					group_taken = int.Parse(values[11]);
			}


			SetColors();
			BackColor = Color_Deactivated;
			TextMain = code;
			TextMain += "\n" + name;
			TextMain += "\n" + teacher;
			TextType = type.ToString();
			TextWeek = (week == Week_type.O ? "" : week.ToString());
			TextBottLeft = (group_capacity == 9999 ? "" : group_taken.ToString() + "/" + group_capacity.ToString() + Environment.NewLine) + 
						(clump_type == "" ? "" : clump_type + "_" + clump_number.ToString());
			Left = 53;
			Top = ToDayNumber(day) * 900 + (startTime.Hour - 6) * 60 + startTime.Minute;
			Height = (endTime.Hour * 60 + endTime.Minute) - (startTime.Hour * 60 + startTime.Minute);
			Width = Form1.brickWidth - 6;
			Name = code;

			data = d;
			Click += new EventHandler(OnClick);
			FlatAppearance.BorderColor = Color_Activated.Add(-30, -30, -30);
			Cursor = Cursors.Hand;

			if (group_capacity-group_taken <= 0)
			{
				BackColor = Color.LightGray;
				FlatAppearance.BorderSize = 0;
				ForeColor = Color.Gray;
				font = new Font(font, FontStyle.Strikeout);
				Enabled = false;
			}
		}
		public override string Text
		{
			get { return ""; }
			set { base.Text = value; }
		}
	
		protected override void OnPaint(PaintEventArgs pevent)
		{
			base.OnPaint(pevent);
			Rectangle rect = ClientRectangle;
			rect.Inflate(-5, -5);
			using (StringFormat sf = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near })
			{
				using (Brush brush = new SolidBrush(ForeColor))
				{
					pevent.Graphics.DrawString(TextMain, font, brush, rect, sf);

					sf.Alignment = StringAlignment.Far;
					pevent.Graphics.DrawString(TextType, font, brush, rect, sf);

					sf.LineAlignment = StringAlignment.Far;
					pevent.Graphics.DrawString(TextWeek, font, brush, rect, sf);

					sf.Alignment = StringAlignment.Near;
					pevent.Graphics.DrawString(TextBottLeft, font, brush, rect, sf);
				}
			}
		}

		public void OnClick(object sender, EventArgs args)
		{
			if (activated)
			{
				OnDeactivate();
				return;
			}
			OnActivate();
		}

		public void OnActivate()
		{
			state = State.activated;
			activated = true;
			BackColor = Color_Activated;
			ForeColor = Color.Black;
			FlatAppearance.BorderSize = 3;
			FixAllBlocks();
			data.subjects[BlockID].Activate();
		}

		public void OnDeactivate()
		{
			state = State.deactivated;
			activated = false;
			BackColor = Color_Deactivated;
			FlatAppearance.BorderSize = 1;
			FixAllBlocks();
			data.subjects[BlockID].Deactivate();
		}

		public void Fade()
		{
			if (activated)
				OnDeactivate();
			BackColor = Color.White;
			Enabled = false;
			ForeColor = Color.White;
		}

		public void UnFade()
		{
			Enabled = true;
			if (CanBeDeactivated())
			{
				state = State.deactivated;
				BackColor = Color_Deactivated;
				ForeColor = Color.Black;
			}
			else
			{
				state = State.grayed;
				BackColor = Color_Grayed;
				ForeColor = Color.Gray;
			}
		}
		
		public bool DifferentWeeks(Block other)
		{
			return ((other.week == Week_type.TN && week == Week_type.TP) || (other.week == Week_type.TP && week == Week_type.TN));
		}

		public void SetColors()
		{
			switch (type)
			{
				case (Class_type.W):
					{
						Color_Activated = Color.FromArgb(0, 145, 0);
						Color_Deactivated = Color.FromArgb(75, 198, 75);
						Color_Grayed = Color.FromArgb(180, 250, 150);
					}
					break;
				case (Class_type.C):
					{
						Color_Activated = Color.FromArgb(244, 122, 0);
						Color_Deactivated = Color.FromArgb(245, 171, 54);
						Color_Grayed = Color.FromArgb(255, 200, 140);
						
					}
					break;
				case (Class_type.L):
					{
						Color_Activated = Color.FromArgb(11, 131, 196);
						Color_Deactivated = Color.FromArgb(90, 159, 198);
						Color_Grayed = Color.FromArgb(184, 207, 224);
					}
					break;
				case (Class_type.P):
					{
						Color_Activated = Color.FromArgb(42, 161, 247);
						Color_Deactivated = Color.FromArgb(39, 202, 241);
						Color_Grayed = Color.FromArgb(142, 234, 255);
					}
					break;
				case (Class_type.S):
					{
						Color_Activated = Color.FromArgb(0, 173, 161);
						Color_Deactivated = Color.FromArgb(98, 193, 188);
						Color_Grayed = Color.FromArgb(159, 204, 201);
					}
					break;
				case (Class_type.I):
					{
						Color_Activated = Color.FromArgb(137, 137, 137);
						Color_Deactivated = Color.FromArgb(200, 200, 200);
						Color_Grayed = Color.FromArgb(240, 240, 240);
					}
					break;
			}
		}

		public bool Intersects(Block b)
		{
			return Bounds.IntersectsWith(b.Bounds);
		}

		public bool SameTime(Block b)
		{
			bool t =  (day.Equals(b.day) && !(startTime >= b.endTime || b.startTime >= endTime));
			if (t && this.code.Equals("M04-35d"))
			{
				Debug.WriteLine(b.code + " ");
			}
			return t;
		}

		public int CompareTo(object obj)
		{
			if (obj is Block)
			{
				return startTime.CompareTo(((Block)obj).startTime);
			}
			return code.CompareTo(obj);
		}

		public bool CanBeDeactivated()
		{
			
			foreach (Block b in data.subjects[BlockID].blocks)
			{
				if (b != this && b.activated)
				{
					return false;
				}
			}
			foreach (Block b in data.blocks)
			{
				if (b != this && SameTime(b))
				{
					if (b.activated && !DifferentWeeks(b))
					{
						
						return false;
					}
				}
			}
			return true;
		}

		public void FixAllBlocks()
		{
			bool again;
			do
			{
				again = false;
				foreach (Block b in data.blocks)
				{
					if (b.Equals(this))
					{
						continue;
					}
					if (b.activated && b.CanBeDeactivated())
					{

					}
					if (b.activated && !b.CanBeDeactivated())
					{
						b.state = State.deactivated;
						b.activated = false;
						again = true;
						b.BackColor = b.Color_Deactivated;
						b.FlatAppearance.BorderSize = 1;
					}
					if (!b.activated && b.CanBeDeactivated())
					{
						b.state = State.deactivated;
						b.BackColor = b.Color_Deactivated;
						b.ForeColor = Color.Black;
					}
					if (!b.activated && !b.CanBeDeactivated())
					{
						b.state = State.grayed;
						b.BackColor = b.Color_Grayed;
						b.ForeColor = Color.Gray;
					}
				}
			}
			while (again);
		}
		











		public static int ToDayNumber(string day)
		{
			day = day.ToLower();
			switch (day)
			{
				case "poniedziałek": return 0;
				case "wtorek": return 1;
				case "środa": return 2;
				case "czwartek": return 3;
				case "piątek": return 4;

			}
			return 0;
		}

	}

	enum State
	{
		deactivated, activated, grayed
	}
}
