using System;
using System.Drawing;

namespace KreatorPlanu
{
	static class Extensions
	{
        public static Color Add(this Color c, int R, int G, int B) => Color.FromArgb(
            Math.Min(255, Math.Max(0, c.R + R)), 
            Math.Min(255, Math.Max(0, c.G + G)), 
            Math.Min(255, Math.Max(0, c.B + B)));
    }
}
