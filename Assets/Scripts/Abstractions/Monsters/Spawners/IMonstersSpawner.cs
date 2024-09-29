using System;

namespace TowerDefence.Abstractions.Monsters.Spawners
{
	public interface IMonstersSpawner
	{
		event Action<IMonstersSpawner, IMonster> MonsterSpawned;
	}
}