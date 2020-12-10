namespace b35k.Utils.Filter.Trigger
{
	public class BoolTriggerChange
	{
		private bool previousState;

		public BoolTriggerChange(bool initialState)
		{
			previousState = initialState;
		}

		public bool Trigger(bool currentValue)
		{
			var result = (previousState != currentValue);
			previousState = currentValue;
			return result;
		}
	}

	public class BoolTriggerTrue
	{
		private bool previousState;

		public BoolTriggerTrue(bool initialState)
		{
			previousState = initialState;
		}

		public bool Trigger(bool currentValue)
		{
			var result = ((previousState == false) && (currentValue == true));
			previousState = currentValue;
			return result;
		}
	}

	public class BoolTriggerFalse
	{
		private bool previousState;

		public BoolTriggerFalse(bool initialState)
		{
			previousState = initialState;
		}

		public bool Trigger(bool currentValue)
		{
			var result = ((previousState == true) && (currentValue == false));
			previousState = currentValue;
			return result;
		}
	}
}
