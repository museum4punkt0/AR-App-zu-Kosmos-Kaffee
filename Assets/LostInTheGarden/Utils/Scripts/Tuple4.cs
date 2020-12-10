namespace LostInTheGarden.Utils
{
	public struct Tuple4
	{
		public float e1, e2, e3, e4;

		public Tuple4(float elem1, float elem2, float elem3, float elem4)
		{
			e1 = elem1;
			e2 = elem2;
			e3 = elem3;
			e4 = elem4;
		}

		public static Tuple4 Add(Tuple4 a, Tuple4 b)
		{
			return new Tuple4(a.e1 + b.e1, a.e2 + b.e2, a.e3 + b.e3, a.e4 + b.e4);
		}
		
		public static Tuple4 Mul(Tuple4 t, float factor)
		{
			return new Tuple4(t.e1 * factor, t.e2 * factor, t.e3 * factor, t.e4 * factor);
		}

		public static Tuple4 Normalized(Tuple4 t)
		{
			var magnitude = t.e1 + t.e2 + t.e3 + t.e4;
			return new Tuple4(t.e1 / magnitude, t.e2 / magnitude, t.e3 / magnitude, t.e4 / magnitude);
		}

		public static Tuple4 Lerp(Tuple4 a, Tuple4 b, float t)
		{
			var oneMinusT = 1f - t;

			return new Tuple4(
				a.e1 * oneMinusT + b.e1 * t,
				a.e2 * oneMinusT + b.e2 * t,
				a.e3 * oneMinusT + b.e3 * t,
				a.e4 * oneMinusT + b.e4 * t
				);
		}
	}
}
