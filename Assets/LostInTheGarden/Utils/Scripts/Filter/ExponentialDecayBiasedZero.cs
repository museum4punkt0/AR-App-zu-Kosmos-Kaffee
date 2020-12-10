namespace LostInTheGarden.Utils.Filter
{
	public class ExponentialDecayBiasedZero : IFilter<float>
	{ 
		private float v;
		private float bias;

		public float Hit(float currentValue, float tau, float deltaT)
		{
			bias =  MathUtils.ExponentialDecayFilter(bias, currentValue, tau, deltaT);
			v = currentValue;
			return v - bias;
		}

		public void Reset(float value)
		{
			v = value;
		}

		public float CurrentValue
		{
			get { return v - bias; }
		}
	}
}
