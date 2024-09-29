using System;

namespace TowerDefence.Abstractions.Monsters.Spawners
{
	public interface IMonstersSpawner
	{
		bool IsRunning { get; }

		event Action<IMonstersSpawner, IMonster> MonsterSpawned;

		void StartSpawning();
		void StopSpawning();
	}
}