namespace LostInTheGarden.Utils.Filter
{
	public class SecondOrderExponetialDegrees : IFilter<float>
	{
		private float v0 = 0f;
		private float v1 = 0f;

		public float Hit(float currentValue, float tau, float deltaT)
		{
			v0 = MathUtils.exponentialDecayFilterDegrees(v0, currentValue, tau, deltaT);
			v1 = MathUtils.exponentialDecayFilterDegrees(v1, v0, tau, deltaT);
			return v1;
		}

		public void Reset(float value)
		{
			v0 = value;
			v1 = value;
		}

		public float CurrentValue
		{
			get { return v0; }
		}
	}
}
