namespace b35k.Utils.Filter.Trigger
{
	public class GenericTriggerChange<T> where T : class
	{
		private T previousState;
		public GenericTriggerChange(T currentValue)
		{
			previousState = currentValue;
		}

		public bool Trigger(T currentValue)
		{
			var result = (currentValue != previousState);
			previousState = currentValue;
			return result;
		}
	}
}
