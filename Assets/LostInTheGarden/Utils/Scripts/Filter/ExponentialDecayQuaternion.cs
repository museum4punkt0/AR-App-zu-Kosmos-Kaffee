using UnityEngine;

namespace LostInTheGarden.Utils.Filter
{
	public class ExponentialDecayQuaternion : IFilter<Quaternion>
	{
		private Quaternion v = Quaternion.identity;

		public Quaternion Hit(Quaternion currentValue, float tau, float deltaT)
		{
			v =  MathUtils.ExponentialDecayFilter(v, currentValue, tau, deltaT);
			return v;
		}

		public void Reset(Quaternion value)
		{
			v = value;
		}

		public Quaternion CurrentValue
		{
			get { return v; }
		}
	}
}
