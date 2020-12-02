using UnityEngine;

namespace LostInTheGarden.Utils.Filter
{
	public class SecondOrderExponetialQuaternion : IFilter<Quaternion>
	{
		private Quaternion v0 = Quaternion.identity;
		private Quaternion v1 = Quaternion.identity;

		public Quaternion Hit(Quaternion currentValue, float tau, float deltaT)
		{
			v0 = MathUtils.ExponentialDecayFilter(v0, currentValue, tau, deltaT);
			v1 = MathUtils.ExponentialDecayFilter(v1, v0, tau, deltaT);
			return v1;
		}

		public void Reset(Quaternion value)
		{
			v0 = value;
		}

		internal void ResetHard(Quaternion value)
		{
			v0 = value;
			v1 = value;
		}

		public Quaternion CurrentValue
		{
			get { return v1; }
		}
	}
}
