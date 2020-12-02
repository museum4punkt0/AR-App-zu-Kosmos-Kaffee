using System;

namespace LostInTheGarden.Utils.Filter
{

	[Serializable]
	public class UnscaledTimerLimit
	{
		public float target;
		private UnscaledTimer timer;



		public UnscaledTimerLimit()
		{
		}

		void Init()
		{
			if (timer == null)
			{
				timer = new UnscaledTimer();
			}
		}

		public void Reset()
		{
			Init();
			timer.Reset();
		}

		public float ElapsedTime
		{
			get
			{
				Init();
				return timer.Elapsed;
			}
		}

		public float ElapsedValue
		{
			get
			{
				Init();
				return timer.Elapsed / target;
			}
		}

		public bool IsElapsed
		{
			get
			{
				Init();
				return timer.Elapsed >= target;
			}
		}
	}
}
