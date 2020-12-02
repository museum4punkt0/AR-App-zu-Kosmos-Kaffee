namespace LostInTheGarden.Utils.Filter
{
	public class SecondOrderExponetialTuple : IFilter<Tuple4>
	{
		private Tuple4 v0;
		private Tuple4 v1;

		public SecondOrderExponetialTuple(Tuple4 initialValue)
		{
			Reset(initialValue);
		}

		public Tuple4 Hit(Tuple4 currentValue, float tau, float deltaT)
		{
			v0 = MathUtils.ExponentialDecayFilterTuble4(v0, currentValue, tau, deltaT);
			v1 = MathUtils.ExponentialDecayFilterTuble4(v1, v0, tau, deltaT);
			return v1;
		}

		public void Reset(Tuple4 value)
		{
			v0 = value;
			v1 = value;
		}

		public Tuple4 CurrentValue
		{
			get { return v0; }
		}
	}
}