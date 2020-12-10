namespace LostInTheGarden.Utils.Filter
{
	public interface IFilterPushPull<T>
	{
		T Hit(T currentValue, float tauPushAwayFromZero, float tauPullTowardsZero, float deltaT);
		void Reset(T value);
	}
}
