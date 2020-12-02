using LostintheGarden.DebugUtils;
using LostInTheGarden.Utils.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LostInTheGarden.Utils
{
	public static class MathUtils
	{
		/**
	  * Inverted box that is 0 inside [-domain, +domain] and 1 else
	  */
		public static float InvertedBox(float domain, float value)
		{
			return System.Convert.ToSingle((value < -domain) || (value > domain));
		}

		/**
	 * Returns 0 inside [-deadZone, +deadZone], value else
	 */
		public static float DeadZone(float deadZone, float value)
		{
			return value * InvertedBox(deadZone, value);
		}

		/**
	 * map a value from the interval [a, b] to [r, s]. Values outside 
	 * of [a, b] are mapped accordingly.
	 */
		public static float Map(float a, float b, float r, float s, float value)
		{
			if (a == b)
			{
				if (value <= a)
				{
					return r;
				}
				else
				{
					return s;
				}
			}

			var ratio = (value - a) / (b - a);

			return r + (s - r) * ratio;
		}

		public static float MapAndClamp(float a, float b, float r, float s, float value)
		{
			return Mathf.Clamp(Map(a, b, r, s, value), Mathf.Min(r, s), Mathf.Max(r, s));
		}

		internal static Matrix4x4 Lerp(Matrix4x4 a, Matrix4x4 b, float t)
		{
			var result = new Matrix4x4();
			for (var i = 0; i < 4; i++)
			{
				for (var j = 0; j < 4; j++)
				{
					result[i, j] = a[i, j] * (1 - t) + b[i, j] * t;
				}
			}
			return result;
		}

		public static Vector3 MapAndClamp(float a, float b, Vector3 r, Vector3 s, float value)
		{
			var mix = MapAndClamp(a, b, 0, 1, value);
			return Vector3.Lerp(r, s, mix);
		}

		public static Vector2 Clamp(this Vector2 vec, Vector2 min, Vector2 max)
		{
			vec.x = Mathf.Clamp(vec.x, min.x, max.x);
			vec.y = Mathf.Clamp(vec.y, min.y, max.y);

			return vec;
		}
		public static Vector3 Clamp(this Vector3 vec, Vector3 min, Vector3 max)
		{
			vec.x = Mathf.Clamp(vec.x, min.x, max.x);
			vec.y = Mathf.Clamp(vec.y, min.y, max.y);
			vec.z = Mathf.Clamp(vec.z, min.z, max.z);

			return vec;
		}

		public static float FindQuaternionTwist(Quaternion q, Vector3 axis)
		{
			axis.Normalize();


			//get the plane the axis is a normal of
			Vector3 orthonormal1, orthonormal2;
			MathUtils.FindOrthonormals(axis, out orthonormal1, out orthonormal2);


			Vector3 transformed = q * orthonormal1;


			//project transformed vector onto plane
			Vector3 flattened = transformed - (Vector3.Dot(transformed, axis) * axis);
			flattened.Normalize();


			//get angle between original vector and projected transform to get angle around normal
			float a = Mathf.Acos(Mathf.Abs(Vector3.Dot(orthonormal1, flattened)));
			Vector3 cross = Vector3.Cross(orthonormal1, flattened);
			cross.Normalize();

			float sign = Vector3.Dot(cross, axis);
			a *= sign;
			return a;
		}

		public static float FindQuaternionRotation(Quaternion q, Vector3 axis)
		{
			axis.Normalize();


			//get the plane the axis is a normal of
			Vector3 orthonormal1, orthonormal2;
			MathUtils.FindOrthonormals(axis, out orthonormal1, out orthonormal2);


			Vector3 transformed = q * orthonormal1;


			//project transformed vector onto plane
			Vector3 flattened = transformed - (Vector3.Dot(transformed, axis) * axis);
			flattened.Normalize();


			//get angle between original vector and projected transform to get angle around normal
			float a = Mathf.Acos(Vector3.Dot(orthonormal1, flattened));
			Vector3 cross = Vector3.Cross(orthonormal1, flattened);
			cross.Normalize();

			float sign = Vector3.Dot(cross, axis);
			a *= sign;
			return a;
		}

		public static void FindOrthonormals(Vector3 normal, out Vector3 orthonormal1, out Vector3 orthonormal2)
		{

			Quaternion orthoX = Quaternion.AngleAxis(Mathf.PI / 2f, new Vector3(1f, 0, 0));
			Quaternion orthoY = Quaternion.AngleAxis(Mathf.PI / 2f, new Vector3(0, 1f, 0));
			Vector3 w = orthoX * normal;
			float dot = Vector3.Dot(normal, w);
			if (Mathf.Abs(dot) > 0.6)
			{
				w = orthoY * normal;
			}
			w.Normalize();

			orthonormal1 = Vector3.Cross(normal, w);
			orthonormal1.Normalize();
			orthonormal2 = Vector3.Cross(normal, orthonormal1);
			orthonormal2.Normalize();
		}

		public struct Frame
		{
			public Vector3 forward;
			public Vector3 left;
			public Vector3 up;
		}

		public static Frame constructOrthonormalFrameForwardUp(Vector3 forward, Vector3 up, bool inForwardDirection)
		{
			var frame = new Frame();

			frame.left = Vector3.Cross(up, forward).normalized;
			if (inForwardDirection)
			{
				frame.up = Vector3.Cross(forward, frame.left).normalized;
				frame.forward = forward.normalized;
			}
			else
			{
				frame.forward = Vector3.Cross(up, frame.left).normalized;
				frame.up = up.normalized;
			}

			return frame;
		}

		public static Frame constructOrthonormalFrameLeftUp(Vector3 left, Vector3 up, bool inForwardDirection)
		{
			var frame = new Frame();

			frame.forward = Vector3.Cross(up, left).normalized;
			if (inForwardDirection)
			{
				frame.up = Vector3.Cross(left, frame.forward).normalized;
				frame.left = left.normalized;
			}
			else
			{
				frame.left = Vector3.Cross(up, frame.forward).normalized;
				frame.up = up.normalized;
			}

			return frame;
		}


		public static int Mod(int x, int m)
		{
			return (x % m + m) % m;
		}

		public static float Mod(float x, float m)
		{
			return (x % m + m) % m;
		}

		// http://gregstanleyandassociates.com/whitepapers/FaultDiagnosis/Filtering/Exponential-Filter/exponential-filter.htm
		public static Vector3 ExponentialDecayFilter(Vector3 lastValue, Vector3 currentValue, float tau, float deltaT)
		{
			if (tau > 0)
			{
				var a = Mathf.Exp(-deltaT / tau);
				return a * lastValue + (1.0f - a) * currentValue;
			}
			else
			{
				return currentValue;
			}
		}

		public static Vector3 ExponentialDecayFilterDirectonalVector(Vector3 lastValue, Vector3 currentValue, float tau, float deltaT, Vector3 forward)
		{
			if (tau > 0)
			{
				var a = Mathf.Exp(-deltaT / tau);
				return MathUtils.LerpDirectionalVector(currentValue, lastValue, a, forward);
			}
			else
			{
				return currentValue;
			}
		}

		public static float ExponentialDecayFilter(float lastValue, float currentValue, float tau, float deltaT)
		{
			if (tau > 0)
			{
				var a = Mathf.Exp(-deltaT / tau);
				return a * lastValue + (1.0f - a) * currentValue;
			}
			else
			{
				return currentValue;
			}
		}

		public static Quaternion ExponentialDecayFilter(Quaternion lastValue, Quaternion currentValue, float tau, float deltaT)
		{
			if (tau > 0)
			{
				var t = Mathf.Exp(-deltaT / tau);
				return Quaternion.Lerp(currentValue, lastValue, t);
			}
			else
			{
				return currentValue;
			}
		}

		public static Tuple4 ExponentialDecayFilterTuble4(Tuple4 lastValue, Tuple4 currentValue, float tau, float deltaT)
		{
			if (tau > 0)
			{
				var t = Mathf.Exp(-deltaT / tau);
				return Tuple4.Lerp(currentValue, lastValue, t);
			}
			else
			{
				return currentValue;
			}
		}

		public static float exponentialDecayFilterDegrees(float lastValue, float currentValue, float tau, float deltaT)
		{
			var shiftedLastValue = (lastValue % 360) + 360;
			var shiftedCurrentValue = (currentValue % 360) + 360;

			return MathUtils.ExponentialDecayFilter(shiftedLastValue, shiftedCurrentValue, tau, deltaT) - 360;
		}

		public static float LinearDecayFilter(float lastValue, float currentValue, float increment, float deltaT)
		{
			var sign = Mathf.Sign(currentValue - lastValue);
			var diff = Mathf.Abs(currentValue - lastValue);
			if (diff > 30)
			{
				increment *= 2f;
			}
			var step = Mathf.Min(diff, increment * deltaT) * sign;
			return lastValue + step;
		}

		public static float NormNTuple(float[] tuple, float norm)
		{
			var sum = 0.0f;
			for (var i = 0; i < tuple.Length; i++)
			{
				sum += Mathf.Pow(tuple[i], norm);
			}

			return Mathf.Pow(sum, 1 / norm);
		}
		public static float[] NormalizeNTuple(float[] tuple, float norm)
		{
			var length = NormNTuple(tuple, norm);

			var result = new float[tuple.Length];
			for (var i = 0; i < tuple.Length; i++)
			{
				result[i] = tuple[i] / length;
			}

			return result;
		}

		public static float[] AddNTuple(float[] tuple1, float[] tuple2)
		{

			var result = new float[Math.Max(tuple1.Length, tuple2.Length)];
			for (int i = 0; i < tuple1.Length && i < tuple2.Length; i++)
			{
				result[i] = tuple1[i] + tuple2[i];
			}
			return result;
		}

		public static float[] LinSpace(float inclusiveLower, float inclusiveUpper, int length)
		{
			var result = new float[length];
			var increment = (inclusiveUpper - inclusiveLower) / (length - 1);
			for (var i = 0; i < length; i++)
			{
				result[i] = inclusiveLower + i * increment;
			}
			return result;
		}


		/**
	 * find the parameter t to the value x that approximates to x ~= animationCurve.evaluate(t) subject to
	 * abs(x - x') > eps
	 * this works only on strictly monotonic animation curves
	 */
		public static float findMonotonicAnimationCurveParameter(CachedAnimationCurve cachedCurve, float target, float eps)
		{
			var keys = cachedCurve.CachedKeys();
			var lower = keys[0].time;
			var upper = keys[keys.Length - 1].time;

			if (target <= cachedCurve.curve.Evaluate(lower))
			{
				return lower;
			}
			if (target >= cachedCurve.curve.Evaluate(upper))
			{
				return upper;
			}

			var t = (lower + upper) / 2.0f;
			var approximation = cachedCurve.curve.Evaluate(t);
			var iterations = 0;

			while (Mathf.Abs(target - approximation) > eps && iterations < 100)
			{
				iterations++;

				if (approximation < target)
				{
					lower = t;
				}
				else
				{
					upper = t;
				}
				t = (lower + upper) / 2.0f;
				approximation = cachedCurve.curve.Evaluate(t);
			}

			if (iterations == 99)
			{
				Log.Warn("Maximum iteration depth reached! Animation curve might not be monotonic");
			}
			return t;
		}

		public static float findMonotonicAnimationCurveParameterFalling(CachedAnimationCurve cachedCurve, float target, float eps)
		{
			var keys = cachedCurve.CachedKeys();
			var lower = keys[keys.Length - 1].time;
			var upper = keys[0].time;

			if (target <= cachedCurve.curve.Evaluate(lower))
			{
				return lower;
			}
			if (target >= cachedCurve.curve.Evaluate(upper))
			{
				return upper;
			}

			var t = (lower + upper) / 2.0f;
			var approximation = cachedCurve.curve.Evaluate(t);
			var iterations = 0;

			while (Mathf.Abs(target - approximation) > eps && iterations < 100)
			{
				iterations++;

				if (approximation < target)
				{
					lower = t;
				}
				else
				{
					upper = t;
				}
				t = (lower + upper) / 2.0f;
				approximation = cachedCurve.curve.Evaluate(t);
			}

			if (iterations == 99)
			{
				Log.Warn("Maximum iteration depth reached! Animation curve might not be monotonic");
			}
			return t;
		}

		public static float GetYRotFromVec(Vector3 dir)
		{
			return Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
		}

		public static Vector3 DirectionRectangleIntersection(Vector3 vec, Rect rect)
		{
			vec.Normalize();


			Vector3 center = rect.center;
			rect.center = Vector2.zero;

			Vector3 vertVec = vec * 1f / Math.Abs(vec.y);
			vertVec *= Math.Abs(rect.height) / 2f;


			Vector3 horVec = vec * 1f / Math.Abs(vec.x);
			horVec *= Math.Abs(rect.width) / 2f;

			if (Math.Abs(vertVec.x) > rect.width / 2f)
			{
				return horVec + center;
			}
			else
			{
				return vertVec + center;
			}

		}

		public static float dBToAmplitude(float db)
		{
			return Mathf.Pow(10.0f, db / 20.0f);
		}

		public static float AmplitudeToDB(float amplitude)
		{
			return 20 * Mathf.Log10(amplitude);
		}

		public static float signedAngleBetweenAngles(float angleA, float angleB)
		{
			var difference = angleA - angleB;

			while (difference < -180.0f)
			{
				difference += 360.0f;
			}
			while (difference > 180.0f)
			{
				difference -= 360.0f;
			}

			return difference;
		}

		public static Matrix4x4 LerpMatrix(Matrix4x4 from, Matrix4x4 to, float value)
		{
			Matrix4x4 ret = new Matrix4x4();
			for (int i = 0; i < 16; i++)
			{
				ret[i] = Mathf.Lerp(from[i], to[i], value);
			}

			return ret;
		}

		public static Vector3 LerpDirectionalVector(Vector3 from, Vector3 to, float t, Vector3 right)
		{
			if (Vector3.Dot(from, to) < -0.5)
			{
				var normal = Vector3.Cross(from, to);
				if (t < 0.5)
				{
					return Vector3.Lerp(from, normal, t * 2);
				}
				else
				{
					return Vector3.Lerp(normal, to, (t - 0.5f) * 2);
				}
			}
			else
			{
				return Vector3.Lerp(from, to, t);
			}
		}

		public static float[] Lerp(float[] from, float[] to, float t)
		{
			var result = new float[from.Length];

			for (var i = 0; i < from.Length; i++)
			{
				result[i] = (1 - t) * from[i] + t * to[i];
			}

			return result;
		}

		public static string ToOrdinal(int num)
		{
			int ones = num % 10;
			int tens = (int)Math.Floor((float)num / 10.0f) % 10;
			string suff = "";

			if (tens == 1)
			{
				suff = "th";
			}
			else
			{
				switch (ones)
				{
					case 1: suff = "st"; break;
					case 2: suff = "nd"; break;
					case 3: suff = "rd"; break;
					default: suff = "th"; break;
				}
			}
			return num.ToString() + suff;
		}

		public static string ToPrettyTime(long time)
		{
			DateTime raceTime = new DateTime(time * 10000);
			return raceTime.ToString("mm:ss.ff");
		}

		public static float SmoothSign(float x, float sharpness)
		{
			var sign = Mathf.Sign(x);
			var res = Math.Tanh(System.Convert.ToSingle(Mathf.Pow(Mathf.Abs(x), sharpness)) * sign);
			return System.Convert.ToSingle(res);
		}

		public static float SmoothStep(float x, float sharpness, float zeroPoint)
		{
			var value = (x * sharpness - zeroPoint * sharpness);
			var res = Math.Tanh(System.Convert.ToDouble(value));
			return System.Convert.ToSingle(res * 0.5 + 0.5);

		}

		public static Vector3 Average(IList<Vector3> list)
		{
			var sum = Vector3.zero;
			foreach (var element in list)
			{
				sum += element;
			}

			return sum / list.Count;
		}

		public static float VectorDisparity(Vector3 a, Vector3 b)
		{
			return Map(-1, 1, 1, 0, Vector3.Dot(a, b));
		}

		public static int Factorial(int n)
		{
			var prod = 1;
			for (var i = 1; i <= n; i++)
			{
				prod *= i;
			}
			return prod;
		}
		public static int Binomial(int n, int k)
		{
			return Factorial(n) / (Factorial(k) * Factorial(n - k));
		}


		public static List<int> IntegerToLehmerCode(int index, int permSize)
		{
			var resList = new List<int>();
			if (permSize <= 1)
			{
				resList.Add(0);

				return resList;
			}

			var multiplier = Factorial(permSize - 1);
			var digit = Mathf.FloorToInt(index / multiplier);
			resList.Add(digit);

			resList.AddRange(IntegerToLehmerCode(index % multiplier, permSize - 1));

			return resList;
		}

		public static List<int> LehmerToPermutation(List<int> lehmer, List<int> data)
		{
			if (lehmer.Count == 1)
			{
				return data;
			}

			var list = new List<int>();
			list.Add(data[lehmer[0]]);
			data.RemoveAt(lehmer[0]);
			lehmer.RemoveAt(0);

			list.AddRange(LehmerToPermutation(lehmer, data));

			return list;
		}

		public static List<int> IndexToPermutation(int index, List<int> data)
		{
			var lehmer = IntegerToLehmerCode(index, data.Count);
			var perm = LehmerToPermutation(lehmer, data);
			return perm;
		}

		public static bool ZeroCrossing(float a, float b)
		{
			return (a < 0 && 0 < b) || (a > 0 && 0 > b);
		}


		public static Quaternion DiffQuaternion(Quaternion a, Quaternion b)
		{
			return Quaternion.Inverse(a) * b;
		}



		public static int findMax(int[] values)
		{
			var max = int.MinValue;
			for (var i = 0; i < values.Length; i++)
			{
				if (values[i] > max)
				{
					max = values[i];
				}
			}
			return max;
		}

		public static int find(int needle, int[] heystack)
		{
			for (var i = 0; i < heystack.Length; i++)
			{
				if (heystack[i] == needle)
				{
					return i;
				}
			}
			return -1;
		}

		public static float findMax(float[] values)
		{
			var max = float.MinValue;
			for (var i = 0; i < values.Length; i++)
			{
				if (values[i] > max)
				{
					max = values[i];
				}
			}
			return max;
		}

		public static int find(float needle, float[] heystack)
		{
			for (var i = 0; i < heystack.Length; i++)
			{
				if (heystack[i] == needle)
				{
					return i;
				}
			}
			return -1;
		}

		/**
        * compute the delta v vector resulting of a force applied
        * to an object of mass m in unit time
        *
        * @param F vector force in newton
        * @param m mass in kg
        * @return delta v 
        */
		public static Vector3 ApplyForce(Vector3 F, float m)
		{
			return F / m;
		}

		public static float Distance(this Vector3 pos1, Vector3 pos2)
		{
			return (pos1 - pos2).magnitude;
		}

		public static float DistanceToLineSegment(Vector3 v, Vector3 w, Vector3 p)
		{
			// thank you, stack overflow!
			// http://stackoverflow.com/questions/849211/shortest-distance-between-a-point-and-a-line-segment

			var lengthSquared = Mathf.Pow(Distance(v, w), 2);
			if (lengthSquared == 0.0)
			{
				return Distance(p, v);
			}

			var t = Vector3.Dot(p - v, w - v) / lengthSquared;

			Vector3 projection = v + Mathf.Clamp01(t) * (w - v);
			return Distance(p, projection);
		}

		public static float OuterProduct(Vector3 a, Vector3 b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z;
		}

		public static bool IsCrossing(float lastValue, float currentValue, float crossingPoint)
		{
			if (lastValue < currentValue)
			{
				return (lastValue < crossingPoint) && (crossingPoint < currentValue);
			}
			else
			{
				return (currentValue < crossingPoint) && (crossingPoint < lastValue);
			}
		}

		public enum CrossDirection
		{
			Upwards,
			Downwards,
		}

		public static bool IsCrossing(float lastValue, float currentValue, float crossingPoint, CrossDirection direction)
		{
			if (lastValue < currentValue && direction == CrossDirection.Upwards)
			{
				return (lastValue < crossingPoint) && (crossingPoint < currentValue);
			}
			else if (currentValue < lastValue && direction == CrossDirection.Downwards)
			{
				return (currentValue < crossingPoint) && (crossingPoint < lastValue);
			}

			return false;
		}

		public static float SoftLimit(float lower, float upper, float width, float value)
		{
			if (value > upper)
			{
				var diff = value - upper;
				return upper + Mathf.Atan(diff / width) * width;
			}
			else if (value < lower)
			{
				var diff = value - lower;
				return lower + Mathf.Atan(diff / width) * width;
			}
			return value;
		}

		// https://en.wikipedia.org/wiki/Smoothstep
		public static float smoothstepAMD(float edge0, float edge1, float x)
		{
			// Scale, bias and saturate x to 0..1 range
			x = Mathf.Clamp01((x - edge0) / (edge1 - edge0));
			// Evaluate polynomial
			return x * x * (3 - 2 * x);
		}

		// https://en.wikipedia.org/wiki/Smoothstep
		public static float smoothstepPerlin(float edge0, float edge1, float x)
		{
			// Scale, bias and saturate x to 0..1 range
			x = Mathf.Clamp01((x - edge0) / (edge1 - edge0));
			// Evaluate polynomial
			return x * x * x * (x * (x * 6 - 15) + 10);
		}

		public static float GammaCorrect(this float a)
		{
			return Mathf.Pow(a, 2.2f);

		}

		public static Matrix4x4 Shear(float direction, float strength)
		{
			var mat = Matrix4x4.identity;
			mat[0, 2] = strength * Mathf.Cos(direction);
			mat[1, 2] = strength * Mathf.Sin(direction);
			return mat;
		}

		public static Matrix4x4 ShearZ(float phi, float rho)
		{
			var mat = Matrix4x4.identity;
			mat[2, 0] = Mathf.Tan(phi);
			mat[2, 1] = Mathf.Tan(rho);
			return mat;
		}

		public static int rangeStart(int groupIndex, int groupLength)
		{
			return groupIndex * groupLength;
		}
		public static int rangeEnd(int groupIndex, int groupLength)
		{
			return (groupIndex + 1) * groupLength;
		}

		public static float SignedAngle(Vector3 a, Vector3 b, Vector3 up)
		{
			AssertPrecondition.AssertNormalised(a, "first operand");
			AssertPrecondition.AssertNormalised(b, "second operand");
			AssertPrecondition.AssertNormalised(up, "up vector");

			var bRight = Vector3.Cross(b, up);
			var sign = Mathf.Sign(Vector3.Dot(a, bRight));

			return Vector3.Angle(a, b) * sign;
		}

		public static float LinearToGamma(float value)
		{
			return Mathf.Pow(value, 2.2f);
		}

		public static float GammaToLinear(float value)
		{
			return Mathf.Pow(value, 1f / 2.2f);
		}
	}
}