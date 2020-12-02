using UnityEngine;

namespace LostInTheGarden.Utils.Filter
{
	public class ExponentialDecayVector3 : IFilter<Vector3>
	{
		private Vector3 v = Vector3.zero;

		public Vector3 Hit(Vector3 currentValue, float tau, float deltaT)
		{
			v =  MathUtils.ExponentialDecayFilter(v, currentValue, tau, deltaT);
			return v;
		}

		public void Reset(Vector3 value)
		{
			v = value;
		}

		public Vector3 CurrentValue
		{
			get { return v; }
		}
	}
}
