using UnityEngine;

namespace LostInTheGarden.Utils.Filter
{
	/**
	 * Filter with different parameters for push and pull
	 */
	class PushPull
	{
		private float lastValue = 0;

		/**
		 * push: away from 0
		 * pull: towards 0
		 */
		public float filter(float currentValue, float tauPush, float tauPull, float deltaT)
		{
			float newValue;
			if (MathUtils.ZeroCrossing(currentValue, lastValue))
			{
				newValue = 0;
			}
			else
			{
				if (Mathf.Abs(currentValue) < Mathf.Abs(lastValue)) // pull
				{
					newValue = MathUtils.ExponentialDecayFilter(lastValue, currentValue, tauPull, deltaT);
				}
				else // push
				{
					newValue = MathUtils.ExponentialDecayFilter(lastValue, currentValue, tauPush, deltaT);
				}

			}

			lastValue = newValue;
			return newValue;
		}
	}
}
