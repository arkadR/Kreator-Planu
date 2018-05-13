using System;

namespace KreatorPlanu
{
	public class Time : IComparable
	{
		public Time(string s)
		{
			string[] values = s.Split(':');
			Hour = int.Parse(values[0]);
			if (values[1].Length == 0)
				Minute = 0;
			else
				Minute = int.Parse(values[1]);
		}
		public static bool operator <(Time t1, Time t2)
		{
			if (t1.Hour != t2.Hour)
				return t1.Hour < t2.Hour;
			return t1.Minute < t1.Minute;
		}
		public static bool operator >(Time t1, Time t2)
		{
			if (t1.Hour != t2.Hour)
				return t1.Hour > t2.Hour;
			return t1.Minute > t1.Minute;
		}
		public static bool operator <=(Time t1, Time t2)
		{
			if (t1.Hour != t2.Hour)
				return t1.Hour < t2.Hour;
			return t1.Minute < t1.Minute;
		}
		public static bool operator >=(Time t1, Time t2)
		{
			if (t1.Hour != t2.Hour)
				return t1.Hour >= t2.Hour;
			return t1.Minute >= t1.Minute;
		}
		public Time(int hour, int minute) { Hour = hour; Minute = minute; }
		public int Hour { get; set; }
		public int Minute { get; set; }

		public int CompareTo(object obj)
		{
			if (obj is Time)
			{
				if (Hour.Equals(((Time)obj).Hour))
				{
					return Minute.CompareTo(((Time)obj).Minute);
				}
			}
			return Hour.CompareTo(obj);
		}
	}
}
