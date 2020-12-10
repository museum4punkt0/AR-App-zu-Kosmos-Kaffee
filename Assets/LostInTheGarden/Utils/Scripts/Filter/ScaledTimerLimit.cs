using System;

namespace LostInTheGarden.Utils.Filter
{

	[Serializable]
	public class ScaledTimerLimit
	{
		public float target;
		private ScaledTimer timer;


		[System.Obsolete("Use ScaledTimer instead")]
		public ScaledTimerLimit()
		{
			timer = new ScaledTimer();
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
