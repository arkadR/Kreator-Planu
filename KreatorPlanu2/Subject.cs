using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace KreatorPlanu
{
	public class Subject : Label, IComparable
	{
		public Subject(Block b)
		{
			AutoSize = true;
			Type = b.type;
			name = b.name;
			Deactivate();
			ForeColor = b.Color_Activated;
			checkBox = new CheckBox
			{
				Name = name + "checkBox",
				Location = new Point(3, 0),
				Checked = true
			};
			checkBox.CheckedChanged += CheckBox_Changed;
		}
		public string name;
		public List<Block> blocks = new List<Block>();
		public bool used = false;
		public Class_type Type { get; set; }
		public CheckBox checkBox;

        public void Activate() => Font = new Font(Font, FontStyle.Strikeout);
        public void Deactivate() => Font = new Font(Font, FontStyle.Bold);
        public void CheckBox_Changed(object o, EventArgs e)
		{
			if (!checkBox.Checked)
			{
                blocks.ForEach(b => b.Fade());
			}
			else
			{
                blocks.ForEach(b => b.UnFade());
			}
		}

		public int CompareTo(object obj)
		{
			if (obj is Subject)
			{
				if (Text.Equals(((Subject)obj).Text))
				{
					return Type.CompareTo(((Subject)obj).Type);
				}
			}
			return Text.CompareTo(obj);
		}

		public void UpdateCount()
		{
            var spots = blocks.Count(b => b.SpotsLeft > 0);
			Text = $"{Type} {name} ({spots})";
		}
	}
}
