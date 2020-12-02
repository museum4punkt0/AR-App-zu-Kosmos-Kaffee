namespace LostInTheGarden.Utils.Filter
{
	public class UnscaledTimer
	{
		private float startTime;
		public float duration;
		public bool isArmed;

		public UnscaledTimer()
		{
			startTime = CustomTimer.UnscaledTime;
		}

		public UnscaledTimer(float startTime)
		{
			this.startTime = startTime;
		}
		
		public void Reset()
		{
			startTime = CustomTimer.UnscaledTime;
		}

		public void ResetAndDisarm()
		{
			startTime = CustomTimer.UnscaledTime;
			isArmed = false;
		}

		public void ResetAndArm()
		{
			startTime = CustomTimer.UnscaledTime;
			isArmed = true;
		}
		
		public float Elapsed
		{
			get { return CustomTimer.UnscaledTime - startTime; }
			set { startTime = CustomTimer.UnscaledTime - value; }
		}

		public float ExpiredValue()
		{
			return (CustomTimer.UnscaledTime - startTime) / duration;
		}

		public float ExpiredValueArmed()
		{
			return isArmed ? ((CustomTimer.UnscaledTime - startTime) / duration) : 0;
		}

		public bool HasExpired()
		{
			return CustomTimer.UnscaledTime > (startTime + duration);
		}

		public bool HasExpiredArmed()
		{
			return isArmed && CustomTimer.UnscaledTime > (startTime + duration);
		}

	}
}
