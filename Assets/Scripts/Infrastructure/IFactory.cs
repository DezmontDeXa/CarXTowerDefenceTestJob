namespace TowerDefence.Infrastructure
{
	public interface IFactory<T>
	{
		T Create();
	}
}
