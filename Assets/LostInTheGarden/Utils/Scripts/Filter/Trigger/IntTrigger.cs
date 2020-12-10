namespace b35k.Utils.Filter.Trigger
{

	public class IntTrigger
	{
		private int value;

		public IntTrigger(int initialValue)
		{
			value = initialValue;
		}

		public bool hasChanged(int newValue)
		{
			var changed = newValue != value;
			value = newValue;
			return changed;
		}
	}
}
