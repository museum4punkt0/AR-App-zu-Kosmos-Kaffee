namespace LostInTheGarden.Utils.Filter
{
	public interface IFilter<T>
	{
		T Hit(T currentValue, float tau, float deltaT);
		void Reset(T value);

		T CurrentValue { get; }
	}
}
