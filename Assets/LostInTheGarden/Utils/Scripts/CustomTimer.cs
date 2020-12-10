using UnityEngine;

namespace LostInTheGarden.Utils
{
	public class CustomTimer : MonoBehaviour
	{

		private static float timeScale = 1;
		private static float unityDefaultFixedTimestamp = 0.02f; // FIXME: query from unity, don't trust their docs! D:

		private static float scaledTime;
		private static float unscaledTimeWithPause;
		private static BoolStack pauseStack = new BoolStack();

		// For now craete one file handling Constants somewhere
		// even though it creates close cuppling I moved the tokes here, 
		// since there is a uniqueness constraint on them that can only be 
		// handled by managing the tokes here.
		// (a better way would be to offer a function that only resolves the uniqueness constraint
		// while not managing all the tokes explicitly...

		public static float TimeScale
		{
			get { return timeScale; }
			set
			{
				timeScale = value;
				SetTimeScale();
			}
		}

		public static void SetPause(string pauseToken, bool value)
		{
			if (value)
			{
				pauseStack.Add(pauseToken);
			}
			else
			{
				pauseStack.Remove(pauseToken);
			}
			SetTimeScale();
		}

		public static void ClearPause()
		{
			pauseStack.Clear();
		}

		public static bool Pause
		{
			get
			{
				return pauseStack.IsSet;
			}
		}

		private static void SetTimeScale()
		{
			Time.timeScale = (Pause ? 0 : timeScale);
			Time.fixedDeltaTime = (Pause ? 0 : unityDefaultFixedTimestamp * timeScale);
		}

		public static float deltaTime
		{
			get
			{
				return Time.deltaTime;
			}
		}

		public static float unscaledDeltaTimeWithPause
		{
			get
			{
				return Pause ? 0 : Time.unscaledDeltaTime;
			}
		}

		public static float UnscaledDeltaTime
		{
			get
			{
				return Time.unscaledDeltaTime;
			}
		}

		public static float ScaledTime
		{
			get
			{
				return scaledTime;
			}
		}

		public static float UnscaledTimeWithPause
		{
			get
			{
				return unscaledTimeWithPause;
			}
		}
		public static float UnscaledTime
		{
			get
			{
				return Time.unscaledTime;
			}
		}

		public static float deltaTimeWithoutPause
		{
			get
			{
				return Time.unscaledDeltaTime * timeScale;
			}
		}

		public void Update()
		{
			scaledTime += Pause ? 0 : Time.deltaTime;
			unscaledTimeWithPause += Pause ? 0 : Time.unscaledDeltaTime;
		}

		public static bool TimeScaleIsLocked
		{
			get;set;
		}

		public void Start()
		{
			// reset value manually
			// the static values survive the scene loads and shit
			scaledTime = 0;
			unscaledTimeWithPause = 0;
		}
	}
}