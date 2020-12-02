using UnityEngine;

namespace b35k.Utils.Filter.Trigger
{

	public class FloatTrigger
	{
		private float value;

		public FloatTrigger(float initialValue)
		{
			value = initialValue;
		}

		public bool hasChanged(float newValue, float eps = 0.000001f)
		{
			var changed = Mathf.Abs(value - newValue) > eps;
			value = newValue;

			return changed;
		}
	}
}
