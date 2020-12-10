namespace LostInTheGarden.Utils.Filter
{
	public class ExponentialDecayPushPull : IFilterPushPull<float>
	{
		ExponentialDecay filter = new ExponentialDecay();
		float lastValue = 0;

		public float Hit(float currentValue, float tauPushAwayFromZero, float tauPullTowardsZero, float deltaT)
		{
			// symmetrical push-pull
			// push tau when moving away from 0
			// pull tau when moving towards 0

			// towards 0: lastValue - currentValue - 0 || 0 - currentValue - lastValue
			// away from 0: 0 lastValue - currentValue || currentValue - lastValue - 0

			float tau;
			if (((lastValue <= currentValue) && (currentValue <= 0) || (0 <= currentValue) && (currentValue <= lastValue)))
			{
				tau = tauPullTowardsZero;
			}
			else
			{
				tau = tauPushAwayFromZero;
			}
			
			//var tau = (currentValue > lastValue) ? tauPullRisingEdge : tauPushFallingEdge;
			lastValue = filter.Hit(currentValue, tau, deltaT);
			return lastValue;
		}

		public void Reset(float value)
		{
			filter.Reset(value);
		}
	}
}
