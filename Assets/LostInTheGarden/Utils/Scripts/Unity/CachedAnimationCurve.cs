using UnityEngine;

namespace LostInTheGarden.Utils.Unity
{
	public class CachedAnimationCurve
	{
		public AnimationCurve curve;
		private Keyframe[] cachedKeys = null;

		public CachedAnimationCurve(AnimationCurve curve)
		{
			this.curve = curve;
		}

		public Keyframe[] CachedKeys()
		{
			if (cachedKeys == null)
			{
				cachedKeys = curve.keys;
			}

			return cachedKeys;
		}

		public void Invalidate()
		{
			cachedKeys = null;
		}
	}
}
