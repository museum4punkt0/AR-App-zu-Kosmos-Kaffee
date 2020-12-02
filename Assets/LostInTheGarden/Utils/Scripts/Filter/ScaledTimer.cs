namespace LostInTheGarden.Utils.Filter
{
	public class ScaledTimer
	{
		private float startTime;
		public float duration;
		public bool isArmed;

		public ScaledTimer()
		{
			startTime = CustomTimer.ScaledTime;
		}

		public ScaledTimer(float startTime)
		{
			this.startTime = startTime;
		}

		public void Reset()
		{
			startTime = CustomTimer.ScaledTime;
		}

		public void ResetAndDisarm()
		{
			startTime = CustomTimer.ScaledTime;
			isArmed = false;
		}

		public void ResetAndArm()
		{
			startTime = CustomTimer.ScaledTime;
			isArmed = true;
		}

		public float Elapsed
		{
			get { return CustomTimer.ScaledTime - startTime; }
			set { startTime = CustomTimer.ScaledTime - value; }
		}

		public float ExpiredValue()
		{
			return (CustomTimer.ScaledTime - startTime) / duration;
		}

		public float ExpiredValueArmed()
		{
			return isArmed ? ((CustomTimer.ScaledTime - startTime) / duration) : 0;
		}

		public bool HasExpired()
		{
			return CustomTimer.ScaledTime > (startTime + duration);
		}

		public bool HasExpiredArmed()
		{
			return isArmed && CustomTimer.ScaledTime > (startTime + duration);
		}
	}
}
