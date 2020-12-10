using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Text;


#pragma warning disable CS0219
namespace LostintheGarden.DebugUtils
{
	public static class AssertDebug
	{
		[CompilerFlag]
		public const string AssertDefine = "LITG_DEBUGUTILS_ASSERT";

		private struct Tuple
		{
			public DateTime startTime;
			public bool alreadyTriggered;
		}

		private static Dictionary<int, Tuple> timeoutTable = new Dictionary<int, Tuple>();
		private static int mainThreadId;
		private static bool isInitialized = false;


		public enum Error
		{
			Error_AssertMainThread,
			Error_AssertWorkerThread,
			Error_AssertTimeCritical,
			Error_AssertNotTimeCritical
		}

		[ThreadStatic]
		private static Stack<List<Error>> _suppressedErrorStack;

		private static Stack<List<Error>> getThreadLocalSuppressedErrorStack()
		{
			if (_suppressedErrorStack == null)
			{
				_suppressedErrorStack = new Stack<List<Error>>();
			}

			return _suppressedErrorStack;
		}


		private static bool isErrorSuppressed(Error error)
		{
			var stack = getThreadLocalSuppressedErrorStack();
			if (stack.Count > 0)
			{
				var currentSuppressedErrors = stack.Peek();
				return currentSuppressedErrors.Contains(error);
			}

			return false;
		}

		[Conditional(AssertDefine)]
		public static void suppressErrorPush(params Error[] errors)
		{
			var stack = getThreadLocalSuppressedErrorStack();

			var newErrors = new List<Error>();
			newErrors.AddRange(errors);

			if (stack.Count > 0)
			{
				var currentSuppressedErrors = stack.Peek();
				newErrors.AddRange(currentSuppressedErrors);
			}

			stack.Push(newErrors);
		}

		[Conditional(AssertDefine)]
		public static void suppressErrorPop()
		{
			var stack = getThreadLocalSuppressedErrorStack();

			if (stack.Count > 0)
			{
				stack.Pop();
			}
		}

		[Conditional(AssertDefine)]
		public static void AssertNoSuppresedErrors()
		{
			var stack = getThreadLocalSuppressedErrorStack();

			if (stack.Count > 0)
			{
				var strb = new StringBuilder();
				strb.Append("There are suppressed errors: ");
				var errors = stack.Peek();
				foreach (var err in errors)
				{
					strb.Append(err.ToString() + ", ");
				}

				Log.Error(strb.ToString(), Log.ASSERT_TAG);
			}
		}


		[Conditional(AssertDefine)]
		public static void Initialize()
		{
			if (isInitialized)
			{
				Log.Error("The Assert Class was already initialized", Log.ASSERT_TAG);
				return;
			}

			mainThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
			Log.Debug("Main thread id: " + mainThreadId, Log.ASSERT_TAG);

			isInitialized = true;
		}

		[Conditional(AssertDefine)]
		public static void AssertTrue(bool condition, string message)
		{
			if (condition == false)
			{
				var breakHere = 42;
				Log.Error(message, Log.ASSERT_TAG);
			}
		}

		[Conditional(AssertDefine)]
		public static void AssertTrue(bool condition, string message, UnityEngine.Object context)
		{
			if (condition == false)
			{
				var breakHere = 42;
				Log.Error(message, Log.ASSERT_TAG, context);
			}
		}

		[Conditional(AssertDefine)]
		public static void AssertFalse(bool condition, string message)
		{
			if (condition == true)
			{
				var breakHere = 42;
				Log.Error(message, Log.ASSERT_TAG);
			}
		}

		[Conditional(AssertDefine)]
		public static void AssertFalse(bool condition, string message, UnityEngine.Object context)
		{
			if (condition == true)
			{
				var breakHere = 42;
				Log.Error(message, Log.ASSERT_TAG, context);
			}
		}

		[Conditional(AssertDefine)]
		public static void AssertNotNull<T>(T obj, string message) where T : class
		{
			if (obj == null)
			{
				var breakHere = 42;
				Log.Error(message, Log.ASSERT_TAG);
			}
		}

		[Conditional(AssertDefine)]
		public static void AssertNull<T>(T obj, string message) where T : class
		{
			if (obj != null)
			{
				var breakHere = 42;
				Log.Error(message, Log.ASSERT_TAG);
			}
		}

		[Conditional(AssertDefine)]
		public static void AssertNotNullOrEmpty(string str, string message)
		{
			if (string.IsNullOrEmpty(str))
			{
				var breakHere = 42;
				Log.Error(message, Log.ASSERT_TAG);
			}
		}

		[Conditional(AssertDefine)]
		public static void AssertNotNullOrEmpty<T>(ICollection<T> collection, string message)
		{
			if (collection == null || collection.Count == 0)
			{
				var breakHere = 42;
				Log.Error(message, Log.ASSERT_TAG);
			}
		}

		[Conditional(AssertDefine)]
		public static void AssertTimeout(object thisPointer, string uniqueID, float timeout, string message, bool failOnEachCall = false)
		{
			var key = thisPointer.GetHashCode() + uniqueID.GetHashCode();
			if (timeoutTable.ContainsKey(key))
			{
				var entry = timeoutTable[key];

				if (DateTime.Now > (entry.startTime.AddSeconds(timeout)))
				{
					if (!entry.alreadyTriggered || failOnEachCall)
					{
						Log.Error("Assert Timeout expired: " + message, Log.ASSERT_TAG);
						entry.alreadyTriggered = true;

						timeoutTable[key] = entry;
					}
				}
			}
			else
			{
				var entry = new Tuple();
				entry.startTime = DateTime.Now;
				entry.alreadyTriggered = false;

				timeoutTable.Add(key, entry);
			}
		}

		[Conditional(AssertDefine)]
		public static void AssertMainThread(string message = null)
		{
			if (!isErrorSuppressed(Error.Error_AssertMainThread))
			{
				if (isInitialized == false)
				{
					Log.Error("Assert: The assert class was not initialized. You have to call Initialize() from the main thread to init", Log.ASSERT_TAG);
					return;
				}

				if (System.Threading.Thread.CurrentThread.ManagedThreadId != mainThreadId)
				{
					if (message != null)
					{
						Log.Error("Asset MAIN Thread: " + message, Log.ASSERT_TAG);
					}
					else
					{
						Log.Error("This function must only be called from the MAIN thread", Log.ASSERT_TAG);
					}
				}
			}
		}

		[Conditional(AssertDefine)]
		public static void AssertWorkerThread(string message = null)
		{
			if (!isErrorSuppressed(Error.Error_AssertWorkerThread))
			{
				if (isInitialized == false)
				{
					Log.Error("Assert: The assert class was not initialized. You have to call Initialize() from the main thread to init", Log.ASSERT_TAG);
					return;
				}

				if (System.Threading.Thread.CurrentThread.ManagedThreadId == mainThreadId)
				{
					if (message != null)
					{
						Log.Error("Asset WORKER Thread: " + message, Log.ASSERT_TAG);
					}
					else
					{
						Log.Error("This function must only be called from a WORKER thread", Log.ASSERT_TAG);
					}
				}
			}
		}

		[Conditional(AssertDefine)]
		public static void AssertLambda(Action action)
		{
			action();
		}

		[Conditional(AssertDefine)]
		public static void AssertShaderIsSupported(Shader shader, String message)
		{
			if (!shader.isSupported)
			{
				Log.Error("Assert Shader is Supported: " + message, Log.ASSERT_TAG);
			}
		}

		[Conditional(AssertDefine)]
		public static void AssertPCOnly(string message)
		{
#if !(UNITY_STANDALONE || UNITY_EDITOR)
			Log.Error("Assert PC only: " + message, Log.ASSERT_TAG);
#endif
		}

		[Conditional(AssertDefine)]
		public static void AssertNormalized(Vector3 a, string message)
		{
			var EPS = 10e-6;
			if (a.magnitude < 1 - EPS || a.magnitude > 1 + EPS)
			{
				Log.Error("Vector not normalised (magnitude: " + a.magnitude + ") " + message);
			}
		}

		// makes not that much sense since it would be file bound and not object bound
		// multiple objects, e.g. ships could not use it...
		// try getting the caller object next time...
		//[Conditional("B35K_DEBUG")]
		//public static void AssertAtMostOncePerFrame()
		//{
		//	var strackTrace = new StackTrace(fNeedFileInfo: true);
		//	var frame = strackTrace.GetFrame(1);
		//	Log.Debug("frame hash: " + frame.GetHashCode().ToString());
		//}

	}

	public static class AssertPrecondition
	{
		[Conditional(AssertDebug.AssertDefine)]
		public static void AssertTrue(bool condition, string message)
		{
			AssertDebug.AssertTrue(condition, "Precondition Failed (assert true): " + message);
		}

		[Conditional(AssertDebug.AssertDefine)]
		public static void AssertTrue(bool condition, string message, UnityEngine.Object context)
		{
			AssertDebug.AssertTrue(condition, "Precondition Failed (assert true): " + message, context);
		}

		[Conditional(AssertDebug.AssertDefine)]
		public static void AssertFalse(bool condition, string message)
		{
			AssertDebug.AssertFalse(condition, "Precondition Failed (assert false): " + message);
		}

		[Conditional(AssertDebug.AssertDefine)]
		public static void AssertFalse(bool condition, string message, UnityEngine.Object context)
		{
			AssertDebug.AssertFalse(condition, "Precondition Failed (assert fase): " + message, context);
		}

		[Conditional(AssertDebug.AssertDefine)]
		public static void AssertNotNull<T>(T obj, string message = "Object must not be null") where T : class
		{
			AssertDebug.AssertNotNull(obj, "Precondition Failed (assert value is not null): " + message);
		}

		[Conditional(AssertDebug.AssertDefine)]
		public static void AssertNull<T>(T obj, string message) where T : class
		{
			AssertDebug.AssertNull(obj, "Precondition Failed (assert value is null): " + message);
		}

		[Conditional(AssertDebug.AssertDefine)]
		public static void AssertNotNullOrEmpty(string str, string message)
		{
			AssertDebug.AssertNotNullOrEmpty(str, "Precondition Failed (string is null or empty): " + message);
		}

		[Conditional(AssertDebug.AssertDefine)]
		public static void AssertNotNullOrEmpty<T>(ICollection<T> collection, string message)
		{
			AssertDebug.AssertNotNullOrEmpty(collection, "Precondition Failed (collection is null or empty): " + message);
		}

		[Conditional(AssertDebug.AssertDefine)]
		public static void AssertSizeOfIs(Type type, int expectedSize, string message)
		{
			var actualSize = System.Runtime.InteropServices.Marshal.SizeOf(type);
            if (actualSize != expectedSize)
			{
				Log.Error("Assert Size Of. Size expected: " + expectedSize + " but was: " + actualSize, Log.ASSERT_TAG);
			}
		}

		[Conditional(AssertDebug.AssertDefine)]
		public static void AssertNormalised(Vector3 a, string message)
		{
			AssertDebug.AssertNormalized(a, message);
		}
	}

	public static class AssertPostcondition
	{
		[Conditional(AssertDebug.AssertDefine)]
		public static void AssertTrue(bool condition, string message)
		{
			AssertDebug.AssertTrue(condition, "Postcondition Failed (assert true): " + message);
		}

		[Conditional(AssertDebug.AssertDefine)]
		public static void AssertTrue(bool condition, object message, UnityEngine.Object context)
		{
			AssertDebug.AssertTrue(condition, "Postcondition Failed (assert true): " + message, context);
		}

		[Conditional(AssertDebug.AssertDefine)]
		public static void AssertFalse(bool condition, string message)
		{
			AssertDebug.AssertFalse(condition, "Postcondition Failed (assert false): " + message);
		}

		[Conditional(AssertDebug.AssertDefine)]
		public static void AssertFalse(bool condition, object message, UnityEngine.Object context)
		{
			AssertDebug.AssertFalse(condition, "Postcondition Failed (assert false): " + message, context);
		}

		[Conditional(AssertDebug.AssertDefine)]
		public static void AssertNotNull<T>(T obj, string message = "Object must not be null.") where T : class
		{
			AssertDebug.AssertNotNull(obj, "Postcondition Failed (assert not null): " + message);
		}

		[Conditional(AssertDebug.AssertDefine)]
		public static void AssertNull<T>(T obj, string message) where T : class
		{
			AssertDebug.AssertNull(obj, "Postcondition Failed (assert value is null): " + message);
		}

		[Conditional(AssertDebug.AssertDefine)]
		public static void AssertNotNullOrEmpty(string str, string message)
		{
			AssertDebug.AssertNotNullOrEmpty(str, "Postcondition Failed (string is null or empty)" + message);
		}
	}
}

#pragma warning restore
