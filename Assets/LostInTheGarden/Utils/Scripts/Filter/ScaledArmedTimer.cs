namespace LostInTheGarden.Utils.Filter
{
	public class ScaledArmedTimer
	{
		private float startTime;

		public ScaledArmedTimer()
		{
			startTime = CustomTimer.ScaledTime;
		}

		public ScaledArmedTimer(float startTime)
		{
			this.startTime = startTime;
		}

		public void Reset()
		{
			startTime = CustomTimer.ScaledTime;
		}

		public float Elapsed
		{
			get { return CustomTimer.ScaledTime - startTime; }
			set { startTime = CustomTimer.ScaledTime - value; }
		}
	}
}
