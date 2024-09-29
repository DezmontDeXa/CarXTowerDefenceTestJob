namespace TowerDefence.Abstractions.Monsters.Pools
{
	public interface IMonstersPool
	{
		public IMonster Spawn();

		public void Release(IMonster poolable);
	}
}