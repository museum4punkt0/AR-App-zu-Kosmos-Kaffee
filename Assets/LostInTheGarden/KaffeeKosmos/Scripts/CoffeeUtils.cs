using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace LostInTheGarden.KaffeeKosmos
{
	public static class CoffeeUtils
	{
		public static Vector3[] ParsePath(string pathString)
		{
			var culture = new CultureInfo("en-US");
			List<Vector3> path = new List<Vector3>();
			if (!pathString.StartsWith("["))
			{
				var pathLines = pathString.Split('\n');
				foreach (var line in pathLines)
				{
					var editLine = line;
					string[] values;
					if (editLine.Contains("\t"))
					{
						values = editLine.Split('\t');
					}
					else
					{
						for (int i = 0; i < 5; i++)
						{
							editLine = editLine.Replace("  ", " ");
						}
						values = editLine.Split(' ');
					}
					
					var pos = new Vector3(-float.Parse(values[0], culture), float.Parse(values[1], culture), float.Parse(values[2], culture));
					path.Add(pos);
				}
			}else
			{

				var pathLines = pathString.Split('\n');
				foreach (var line in pathLines)
				{
					var editLine = line;
					string[] values;
					editLine = editLine.Replace("\r", "");
					editLine = editLine.Trim(new char[] { '[', ']' });
					values = editLine.Split(',');
					for(int i = 0; i < values.Length; i++)
					{
						values[i] = values[i].Trim();
					}

					var pos = new Vector3(-float.Parse(values[0], culture), float.Parse(values[1], culture), float.Parse(values[2], culture)) * 0.01f;
					path.Add(pos);
				}
			}

			return path.ToArray();
		}

	}
}