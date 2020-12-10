using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LostintheGarden.DebugUtils
{
	public static class Log
	{
		#region public interface

		[CompilerFlag]
		public const string DebugDefine = "LITG_DEBUG";
		[CompilerFlag]
		public const string LogDefine = "LITG_DEBUGUTILS_LOG";

		public delegate void LoggedLineCallback(string logLine);

		public static LoggedLineCallback LoggedLine;
		public static DateTime startTime = DateTime.Now;

		private static readonly string MARKER_START = "[LostLog]";
		private static readonly string MARKER_END = "[/LostLog]";
		private static readonly string TAG_SECTION = "section";
		private static readonly string[] NO_TAG = new string[] { };

		private static StringBuilder strb = new StringBuilder(512);
		private static Object lockObject = new Object();

		private static Dictionary<string, Timer> timers = new Dictionary<string, Timer>();
		private static readonly string TIMER_DEFAULT_NAME = "default";

		// public constants
		public static readonly string[] ASSERT_TAG = new string[] { "assert" };

		#region Logging
		[Conditional(LogDefine)]
		public static void Trace(string message, string[] tags = null, UnityEngine.Object context = null)
		{
			LogInUnityEditor(message, context);
			Write(message, LogLevel.Trace, tags);
		}

		[Conditional(LogDefine)]
		public static void Debug(string message, string[] tags = null, UnityEngine.Object context = null)
		{
			LogInUnityEditor(message, context);
			Write(message, LogLevel.Debug, tags);
		}

		[Conditional(LogDefine)]
		public static void Error(string message, string[] tags = null, UnityEngine.Object context = null)
		{
			LogErrorInUnityEditor(message, context);
			Write(message, LogLevel.Error, tags);
		}

		[Conditional(LogDefine)]
		public static void Warn(string message, string[] tags = null, UnityEngine.Object context = null)
		{
			LogWarningInUnityEditor(message, context);
			Write(message, LogLevel.Warn, tags);
		}

		[Conditional(LogDefine)]
		public static void Exception(System.Exception e, string[] tags = null, UnityEngine.Object context = null)
		{
			LogErrorInUnityEditor(e.Message, context);
			Write(e.Message, LogLevel.Exception, tags);
		}

		[Conditional(LogDefine)]
		public static void SectionHeader(string name, UnityEngine.Object context = null)
		{
			LogInUnityEditor("----------------------- " + name + " ---------------------", context);
			Write(name, LogLevel.Debug, new string[] { TAG_SECTION });
		}

		[Conditional(LogDefine)]
		public static void DebugLambda(System.Func<string> func, string[] tags = null, UnityEngine.Object context = null)
		{
			var message = func();
			if (message != null)
			{
				LogInUnityEditor(message, context);
				Write(message, LogLevel.Debug, tags);
			}
		}
		#endregion


		#region Timer
		[Conditional(LogDefine)]
		public static void HitTimer(string eventName)
		{
			HitTimer(TIMER_DEFAULT_NAME, eventName);
		}

		[Conditional(LogDefine)]
		public static void HitTimer(string timerName, string eventName)
		{
			Timer timer;
			if (timers.ContainsKey(timerName))
			{
				timer = timers[timerName];
			} else
			{
				timer = new Timer();
				timers.Add(timerName, timer);
			}

			Write("[" + timer.ElapsedMillis() + "ms] " + timerName + ": " + eventName, LogLevel.Debug, new String[] { "timer", "timer-" + timerName });
		}

		[Conditional(LogDefine)]
		public static void ResetTimer()
		{
			ResetTimer(TIMER_DEFAULT_NAME);
		}

		[Conditional(LogDefine)]
		public static void ResetTimer(string timerName)
		{
			Timer timer;
			if (timers.ContainsKey(timerName))
			{
				timer = timers[timerName];
			}
			else
			{
				timer = new Timer();
				timers.Add(timerName, timer);
			}

			timer.Reset();
		}
		#endregion


		#endregion

		#region privates
		private static void Write(string message, LogLevel level, string[] tags)
		{
			var stacktrace = new StackTrace(2, true);
			var delta = DateTime.Now - startTime;

			if (tags == null)
			{
				// using the methond name as default 
				tags = new string[] { stacktrace.GetFrame(0).GetMethod().DeclaringType.FullName.Replace(".", "-") };
			}

			lock (lockObject)
			{
				strb.Length = 0; // clear stringbuilder

				strb.AppendLine(MARKER_START);
				strb.AppendLine("{");
				strb.AppendLine("\t\"logLevel\": \"", level.ToString(), "\",");
				strb.AppendLine("\t\"message\": \"", EscapeString(message), "\",");
				strb.AppendLine("\t\"time\": ", "\"", delta.Hours.ToString("00"), ":", delta.Minutes.ToString("00"), ":", delta.Seconds.ToString("00"), ".", delta.Milliseconds.ToString("000"), "\",");

				strb.Append("\t\"tags\": ", "[");
				for (var i = 0; i < tags.Length; i++)
				{
					strb.Append("\"", tags[i], "\"", (i < tags.Length - 1) ? "," : "");
				}
				strb.AppendLine("],");


				strb.AppendLine("\t\"stackTrace\": [");

				var stFrames = stacktrace.GetFrames();

				for (var i = 0; i < stFrames.Length; i++)
				{
					var frame = stFrames[i];
					//strb.AppendLine("\t\t\"", frame.GetFileName().Replace("\\", "/"), ":", frame.GetFileLineNumber().ToString(), " (", frame.GetMethod().Name, ")", (i < (stFrames.Length - 1) ? "," : ""));
					strb.AppendLine("\t\t\"", frame.ToString().Replace("\\", "/"), "\"", (i < (stFrames.Length - 1) ? "," : ""));
				}
				strb.AppendLine("\t],");
				strb.AppendLine("\t\"thread\": \"", System.Threading.Thread.CurrentThread.ManagedThreadId.ToString(), "\"");
				strb.AppendLine("}");
				strb.AppendLine(MARKER_END);

				System.Console.WriteLine(strb.ToString());

				if (LoggedLine != null)
				{
					LoggedLine(message);
				}
			}
		}

		#region conditional unity logging forwards
		[Conditional("UNITY_EDITOR")]
		private static void LogInUnityEditor(string message, UnityEngine.Object context)
		{
			UnityEngine.Debug.Log(message, context);
		}

		[Conditional("UNITY_EDITOR")]
		private static void LogWarningInUnityEditor(string message, UnityEngine.Object context)
		{
			UnityEngine.Debug.LogWarning(message, context);
		}

		[Conditional("UNITY_EDITOR")]
		private static void LogErrorInUnityEditor(string message, UnityEngine.Object context)
		{
			UnityEngine.Debug.LogError(message, context);
		}
		#endregion
		#endregion

		#region helpers

		private static StringBuilder Append(this StringBuilder strb, params string[] args)
		{
			for (var i = 0; i < args.Length; i++)
			{
				strb.Append(args[i]);
			}

			return strb;
		}
		private static StringBuilder AppendLine(this StringBuilder strb, params string[] args)
		{
			Append(strb, args);
			strb.Append("\n");

			return strb;
		}

		private class Timer
		{
			private DateTime start;

			public Timer()
			{
				Reset();
			}

			public double ElapsedMillis()
			{
				DateTime currentTime = DateTime.Now;
				var elapsed = currentTime - start;
				return elapsed.TotalMilliseconds;
			}

			public void Reset()
			{
				start = DateTime.Now;
			}
		}

		private static string EscapeString(string input)
		{
			var result = input
				.Replace("\r\n", "<br>")
				.Replace("\r", "<br>")
				.Replace("\n", "<br>")
				.Replace("\t", " ")
				.Replace("\"", "\\\"");
			return result;
		}
		#endregion
	}
}