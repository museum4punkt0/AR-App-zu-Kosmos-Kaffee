using UnityEngine.Profiling;
using System.Diagnostics;

namespace LostintheGarden.DebugUtils
{
	public class ConditionalProfiler
	{
		public const string ProfilerDefine = "LITG_DEBUGUTILS_PROFILER";

		[Conditional(ProfilerDefine)]
		public static void BeginSample(string str)
		{
			Profiler.BeginSample(str);
		}

		[Conditional(ProfilerDefine)]
		public static void EndSample()
		{
			Profiler.EndSample();
		}
	}
}
