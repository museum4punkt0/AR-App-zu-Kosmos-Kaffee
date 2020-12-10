using System.Collections.Generic;

namespace LostInTheGarden.Utils
{
	public class BoolStack
	{

		private HashSet<string> stack = new HashSet<string>();

		public void Add(string id)
		{
			if (!stack.Contains(id))
			{
				stack.Add(id);
			}
		}

		public void Remove(string id)
		{
			if (stack.Contains(id))
			{
				stack.Remove(id);
			}
		}

		public void Clear()
		{
			stack.Clear();
		}

		public bool IsSet
		{
			get { return stack.Count > 0; }
		}

		public override string ToString()
		{
			var output = "";
			foreach (var value in stack)
			{
				output += value + "; ";
			}
			if(output.Length > 2)
			{
				output = output.Substring(0, output.Length - 2);
			}
			return output;
		}

	}
}