using LostintheGarden.DebugUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LostInTheGarden.Utils
{

	public static class CSharpUtils
	{
		public static void ClearArray<T>(this T[] array, T newValue)
		{
			for (var i = 0; i < array.Length; i++)
			{
				array[i] = newValue;
			}
		}

		public static string ArrayToString<T>(this T[] arr)
		{
			string buff = "[";
			for (var i = 0; i < arr.Length; i++)
			{
				buff += arr[i];
				if (i < arr.Length - 1)
				{
					buff += ", ";
				}
			}
			buff += "]";
			return buff;
		}


		public static void Clear(this MemoryStream stream)
		{
			stream.SetLength(0);
			stream.Position = 0;
		}

		public static void SetData(this MemoryStream stream, byte[] data)
		{
			AssertDebug.AssertNotNull(stream, "The thream on which the data is set must not be null");
			AssertDebug.AssertNotNull(data, "The Data written to the stream nust not be null");
			stream.Clear();
			stream.Write(data, offset: 0, count: data.Length);
			stream.SetLength(data.Length);
			stream.Position = 0;
		}

		public static T LastElement<T>(this List<T> list)
		{
			return list[list.Count - 1];
		}

		public static bool IsInBounds<T>(this List<T> list, int index)
		{
			return index < list.Count;
		}

		// bubble sort
		public static int[] SortArrayIndicesInPlace(float[] data, int[] indices)
		{
			for (var i = 0; i < indices.Length; i++)
			{
				indices[i] = i;
			}
			var currentIndex = 0;

			while (currentIndex < indices.Length - 1)
			{
				for (var i = indices.Length - 1; i > currentIndex; i--)
				{
					var value = data[indices[i]];
					var leftValue = data[indices[i - 1]];

					if (value < leftValue)
					{
						// swap indices[i], indices[i -1]
						var tmp = indices[i];
						indices[i] = indices[i - 1];
						indices[i - 1] = tmp;
					}
				}
				currentIndex++;
			}
			return indices;
		}

		public static void HardcodedSort4(float[] distances, int[] indices)
		{
			for (var i = 0; i < 4; i++)
			{
				indices[i] = i;
			}

			// swap 1, 2
			// swap 0, 3
			// swap 0, 1
			// swap 2, 3
			// swap 1, 2

			// swap 1, 2
			if (distances[1] > distances[2])
			{
				indices[1] = 2;
				indices[2] = 1;
			}

			// swap 0, 3
			if (distances[indices[0]] > distances[indices[3]])
			{
				var tmp = indices[0];
				indices[0] = indices[3];
				indices[3] = tmp;
			}

			// swap 0, 1
			if (distances[indices[0]] > distances[indices[1]])
			{
				var tmp = indices[0];
				indices[0] = indices[1];
				indices[1] = tmp;
			}

			// swap 2, 3
			if (distances[indices[2]] > distances[indices[3]])
			{
				var tmp = indices[2];
				indices[2] = indices[3];
				indices[3] = tmp;
			}

			// swap 1, 2
			if (distances[indices[1]] > distances[indices[2]])
			{
				var tmp = indices[1];
				indices[1] = indices[2];
				indices[2] = tmp;
			}
		}

		public static List<T> GetRandomListFromList<T>(List<T> list, int num)
		{
			List<T> returnList = new List<T>();
			if (list.Count == 0)
			{
				return returnList;
			}
			List<T> pot = new List<T>(list);
			for (int i = 0; i < num; i++)
			{
				int id = UnityEngine.Random.Range(0, pot.Count);
				returnList.Add(pot[id]);
				pot.RemoveAt(id);
				if (pot.Count == 0)
				{
					pot.AddRange(list);
				}
			}
			return returnList;
		}

		public static void SetNull<T>(this T[] arr, T element) where T : class
		{
			for (var i = 0; i < arr.Length; i++)
			{
				if (arr[i] == element)
				{
					arr[i] = null;
				}
			}
		}

		// move all elements to the left, and all null to the right
		// [1, null, 2, 3, null, null, 6] ==> [1, 2, 3, null, null, null]
		public static void BubbleNullsToRight<T>(this T[] arr) where T : class
		{
			// bubble up nulls by swapping with their neighbours
			var dirty = false;
			for (var i = 0; i < arr.Length - 1; i++)
			{
				if (arr[i] == null && arr[i + 1] != null)
				{
					arr.Swap(i, i + 1);
					dirty = true;
				}
			}

			if (dirty)
			{
				arr.BubbleNullsToRight();
			}
		}

		public static void Swap<T>(this T[] arr, int i, int j)
		{
			var tmp = arr[i];
			arr[i] = arr[j];
			arr[j] = tmp;
		}

		public static StringBuilder Clear(this StringBuilder strb)
		{
			// no Clear() function in .Net 2.0
			// only resetting the length not the capacity, to retain the allocated memory.
			strb.Length = 0;
			return strb;
		}

		public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue)
		{
			if (dict.ContainsKey(key))
			{
				return dict[key];
			}
			else
			{
				dict.Add(key, defaultValue);
				return defaultValue;
			}
		}

		public class Rollback : IDisposable
		{
			private Action dispose;

			public Rollback(Action disposeAction)
			{
				dispose = disposeAction;
			}

			public Rollback(Action setupAction, Action disposeAction)
			{
				setupAction();
				dispose = disposeAction;
			}

			public void Dispose()
			{
				dispose();
			}
		}
		

		public static T ReverseAccess<T>(T[] arr, int i)
		{
			return arr[(arr.Length - 1) - i];
		}

		public static T ReverseAccess<T>(List<T> arr, int i)
		{
			return arr[(arr.Count - 1) - i];
		}
	}
}
