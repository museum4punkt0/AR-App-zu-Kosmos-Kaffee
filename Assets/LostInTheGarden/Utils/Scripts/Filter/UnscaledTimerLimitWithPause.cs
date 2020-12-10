using System;

namespace LostInTheGarden.Utils.Filter
{

	[Serializable]
	public class UnscaledTimerLimitWithPause
	{
		public float target;
		private UnscaledTimerWithPause timer;

		public UnscaledTimerLimitWithPause()
		{
			timer = new UnscaledTimerWithPause();
		}

		public void Reset()
		{
			timer.Reset();
		}

		public float ElapsedTime
		{
			get { return timer.Elapsed; }
		}

		public float ElapsedValue
		{
			get { return timer.Elapsed / target; }
		}

		public bool IsElapsed
		{
			get { return timer.Elapsed >= target; }
		}
	}
}
