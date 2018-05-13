using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace KreatorPlanu
{
	static class Extensions
	{
		public static Color Add(this Color c, int R, int G, int B)
		{
			return Color.FromArgb(Math.Min(255, Math.Max(0, c.R + R)), Math.Min(255, Math.Max(0, c.G + G)), Math.Min(255, Math.Max(0, c.B + B)));
			//return Color.White;
		}
	}
}
