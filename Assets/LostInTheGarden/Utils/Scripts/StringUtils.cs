using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LostInTheGarden.Utils
{
	public static class StringUtils
	{
		private static StringBuilder memStrb = new StringBuilder();

		public static StringBuilder ToPrettyTime(this long ms, StringBuilder strb)
		{
			float seconds = (float)ms / 1000f;
			seconds.ToPrettyTime(strb);
			return strb;
		}

		public static string ToPrettyTimeString(this long ms)
		{
			float seconds = (float)ms / 1000f;
			memStrb.Clear();
			seconds.ToPrettyTime(memStrb);
			return memStrb.ToString();
		}
		

		public static StringBuilder ToPrettyTime(this float s, StringBuilder strb)
		{
			int seconds = (int)Mathf.Floor(s);
			float millis = s - seconds;
			int minutes = (int)Mathf.Floor(seconds / 60.0f);
			seconds = seconds - (minutes * 60);
			int mil = (int)Mathf.Floor(millis * 1000);

			strb.AppendPositivePadded(minutes, 2).Append(":").AppendPositivePadded(seconds, 2).Append(":").AppendPositivePadded(mil, 3);

			return strb;
		}

		public static string ToPrettyTimeString(this float s)
		{
			memStrb.Clear();
			s.ToPrettyTime(memStrb);
			return memStrb.ToString();
		}

		public static StringBuilder AppendPositivePadded(this StringBuilder strb, int value, int length)
		{
			var valueLength = (value > 0) ? Mathf.FloorToInt(Mathf.Log10(value)) : 0;

			for (var i = valueLength; i < (length - 1); i++)
			{
				strb.Append("0");
			}
			strb.AppendInt(value);
			return strb;
		}

		public static StringBuilder AppendPrettyTime(this StringBuilder strb, float value)
		{
			return value.ToPrettyTime(strb);
		}

		public static string ToPrettyBytes(this int bytes)
		{
			return ((long)bytes * 8).ToPrettyBits();
		}

		public static string ToPrettyBits(this int bits)
		{
			return ((long)bits).ToPrettyBits();
		}

		public static string ToPrettyBits(this long bits)
		{
			string retVal = "";

			if (bits < 8)
			{
				retVal = bits.ToString() + "b";
			}
			else
			{
				float bytes = bits / 8f;
				if (bytes < 1024)
				{
					float floatVal = (float)bytes;
					retVal = floatVal.ToString("0.00") + "B";
				}
				else if (bytes < 1024 * 1024)
				{
					float floatVal = (float)bytes;
					floatVal /= (float)1024;
					retVal = floatVal.ToString("0.00") + "KB";
				}
				else if (bytes < 1024 * 1024 * 1024)
				{
					float floatVal = (float)bytes;
					floatVal /= (float)1024 * 1024;
					retVal = floatVal.ToString("0.00") + "MB";
				}
				else if (bytes < 1024L * 1024 * 1024 * 1024)
				{
					float floatVal = (float)bytes;
					floatVal /= (float)(1024 * 1024 * 1024);
					retVal = floatVal.ToString("0.00") + "GB";
				}
			}

			return retVal;
		}


		public static string ToPrettyDelta(this long ms)
		{
			float seconds = (float)ms / 1000f;
			return seconds.ToPrettyDelta();
		}


		public static string ToPrettyPercent(this float val)
		{
			val *= 100f;
			return val.ToString("0") + "%";
		}

		public static string ToPrettyDelta(this float s)
		{
			bool isNeg = (s < 0);
			s = Mathf.Abs(s);
			int seconds = (int)Mathf.Floor(s);
			float millis = s - seconds;
			int minutes = (int)Mathf.Floor(seconds / 60.0f);
			seconds = seconds - (minutes * 60);
			int mil = (int)Mathf.Floor(millis * 1000);
			string retVal;
			if (minutes > 0)
			{
				retVal = string.Format("{0}:{1}.{2}", minutes, seconds.ToString("00"), mil.ToString("000"));
			}
			else
			{
				retVal = string.Format("{0}.{1}", seconds.ToString("00"), mil.ToString("000"));
			}
			if (isNeg)
			{
				retVal = "-" + retVal;
			}

			return retVal;
		}

		public static StringBuilder ToFixed(this float value, StringBuilder strb, int decimalPlaces)
		{
			//var leftOfDot = Mathf.FloorToInt(value);
			//var rightOfDot = Mathf.FloorToInt(value % 1) * Mathf.Pow(10, decimalPlaces));

			//strb.Append(leftOfDot).Append(".").Append(rightOfDot);

			var vInt = (int)(value * Mathf.Pow(10, decimalPlaces));
			strb.AppendInt(vInt);
			strb.Insert(strb.Length - decimalPlaces, ".");
			return strb;
		}

		public static string AddUnitPrefixString(this int value)
		{
			memStrb.Clear();
			return value.AddUnitPrefix(memStrb).ToString();
		}

		public static StringBuilder AddUnitPrefix(this int value, StringBuilder strb)
		{
			if (value < 1E3)
			{
				strb.AppendInt(value);
			}
			else if (value < 1E6)
			{
				//float floatVal = (float)value;
				//floatVal /= (float)1E3;
				//floatVal.ToFixed(strb, 2).Append("K");
				strb.AppendInt(value);
			}
			else if (value < 1E9)
			{
				float floatVal = (float)value;
				floatVal /= (float)1E6;
				floatVal.ToFixed(strb, 2).Append("M");
			}
			else if (value < 1E12)
			{
				float floatVal = (float)value;
				floatVal /= (float)1E9;
				floatVal.ToFixed(strb, 2).Append("G");
			}
			else if (value < 1E15)
			{
				float floatVal = (float)value;
				floatVal /= (float)1E12;
				floatVal.ToFixed(strb, 2).Append("T");
			}

			return strb;
		}


		public static StringBuilder AppendOrdinal(this StringBuilder strb, int value)
		{
			return value.GetOrdinal(strb);
		}


		public static StringBuilder GetOrdinal(this int num, StringBuilder strb)
		{
			if (num <= 0) return strb;

			switch (num % 100)
			{
				case 11:
				case 12:
				case 13:
					strb.Append("th");
					return strb;
			}

			switch (num % 10)
			{
				case 1:
					strb.Append("st");
					break;
				case 2:
					strb.Append("nd");
					break;
				case 3:
					strb.Append("rd");
					break;
				default:
					strb.Append("th");
					break;
			}
			return strb;
		}
		public static string GetOrdinalString(this int num)
		{
			memStrb.Clear();
			return num.GetOrdinal(memStrb).ToString();
		}
		

		public static string GetOrdinalString(this uint num)
		{
			return ((int)num).GetOrdinalString();
		}

		public static StringBuilder AppendOrdinal(this StringBuilder strb, uint value)
		{
			return ((int)value).GetOrdinal(strb);
		}

		
		public static StringBuilder AppendInt(this StringBuilder strb, int value)
		{
			if (value < 0)
			{
				strb.Append('-');
			}
			long longValue = (long)value;
			var absValue = Math.Abs(longValue);
			var digits = Math.Floor(Math.Log10(absValue) + 1);
			if (absValue == 0)
			{
				digits = 1;
			}
			for (int i = 0; i < digits; i++)
			{
				var digitValue = absValue / Math.Pow(10, digits - 1 - i);
				char digit = (char)(digitValue % 10 + '0');
				strb.Append(digit);
			}
			return strb;
		}


		public static string ToPrettyString<T>(this List<T> list)
		{
			var res = "";
			foreach (var element in list)
			{
				res += element.ToString() + ", ";
			}

			return res;
		}
	}
}