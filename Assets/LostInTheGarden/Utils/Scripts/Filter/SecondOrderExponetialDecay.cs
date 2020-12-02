namespace LostInTheGarden.Utils.Filter
{
	public class SecondOrderExponetialDecay : IFilter<float>
	{
		private float v0 = 0f;
		private float v1 = 0f;

		public float Hit(float currentValue, float tau, float deltaT)
		{
			v0 = MathUtils.ExponentialDecayFilter(v0, currentValue, tau, deltaT);
			v1 = MathUtils.ExponentialDecayFilter(v1, v0, tau, deltaT);
			return v1;
		}

		public void Reset(float value)
		{
			v0 = value;
		}

		internal void ResetHard(float value)
		{
			v0 = value;
			v1 = value;
		}

		public float CurrentValue
		{
			get { return v1; }
		}
	}
}
