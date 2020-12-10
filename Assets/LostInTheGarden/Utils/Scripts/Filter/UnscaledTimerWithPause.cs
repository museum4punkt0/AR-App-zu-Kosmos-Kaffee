namespace LostInTheGarden.Utils.Filter
{
	public class UnscaledTimerWithPause
	{
		private float startTime;
		public float duration;
		public bool isArmed;

		public UnscaledTimerWithPause()
		{
			startTime = CustomTimer.UnscaledTimeWithPause;
		}

		public UnscaledTimerWithPause(float startTime)
		{
			this.startTime = startTime;
		}

		public void Reset()
		{
			startTime = CustomTimer.UnscaledTimeWithPause;
		}

		public void ResetAndDisarm()
		{
			startTime = CustomTimer.UnscaledTimeWithPause;
			isArmed = false;
		}

		public void ResetAndArm()
		{
			startTime = CustomTimer.UnscaledTimeWithPause;
			isArmed = true;
		}

		public float Elapsed
		{
			get { return CustomTimer.UnscaledTimeWithPause - startTime; }
			set { startTime = CustomTimer.UnscaledTimeWithPause - value; }
		}

		public float ExpiredValue()
		{
			return (CustomTimer.UnscaledTimeWithPause - startTime) / duration;
		}

		public float ExpiredValueArmed()
		{
			return isArmed ? ((CustomTimer.UnscaledTimeWithPause - startTime) / duration) : 0;
		}

		public bool HasExpired()
		{
			return CustomTimer.UnscaledTimeWithPause > (startTime + duration);
		}

		public bool HasExpiredArmed()
		{
			return isArmed && CustomTimer.UnscaledTimeWithPause > (startTime + duration);
		}
	}
}
