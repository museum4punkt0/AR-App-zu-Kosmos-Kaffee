namespace LostInTheGarden.Utils.Filter
{
	public class ExponentialDecay : IFilter<float>
	{
		private float v = 0;

		public float Hit(float currentValue, float tau, float deltaT)
		{
			v =  MathUtils.ExponentialDecayFilter(v, currentValue, tau, deltaT);
			return v;
		}

		public void Reset(float value)
		{
			v = value;
		}

		public float CurrentValue
		{
			get { return v; }
		}
	}
}
