using UnityEngine;

namespace LostInTheGarden.Utils.Filter
{
	public class SecondOrderExponetialVector3 : IFilter<Vector3>
	{
		private Vector3 v0 = Vector3.zero;
		private Vector3 v1 = Vector3.zero;

		public Vector3 Hit(Vector3 currentValue, float tau, float deltaT)
		{
			v0 = MathUtils.ExponentialDecayFilter(v0, currentValue, tau, deltaT);
			v1 = MathUtils.ExponentialDecayFilter(v1, v0, tau, deltaT);
			return v1;
		}

		public void Reset(Vector3 value)
		{
			v0 = value;
		}

		internal void ResetHard(Vector3 value)
		{
			v0 = value;
			v1 = value;
		}

		public Vector3 CurrentValue
		{
			get { return v1; }
		}
	}
}
