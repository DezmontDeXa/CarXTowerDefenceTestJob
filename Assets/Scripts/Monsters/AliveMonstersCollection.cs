using System;
using System.Collections.Generic;
using TowerDefence.Abstractions.Monsters;
using TowerDefence.Abstractions.Monsters.Collections;
using TowerDefence.Abstractions.Monsters.Spawners;
using TowerDefence.Monsters.Spawners;
using VContainer.Unity;

namespace TowerDefence
{
	public class AliveMonstersCollection : IInitializable, IDisposable, IAliveMonstersCollection
	{
		private List<IMonster> _aliveMonsters;
		private readonly IMonstersSpawner _spawner;

		public IEnumerable<IMonster> AliveMonsters => _aliveMonsters;

		public AliveMonstersCollection(MonstersSpawner spawner)
		{
			_aliveMonsters = new List<IMonster>();
			_spawner = spawner;
		}

		public void Initialize()
		{
			_spawner.MonsterSpawned += Spawner_MonsterSpawned;
		}

		private void Spawner_MonsterSpawned(IMonstersSpawner spawner, IMonster monster)
		{
			monster.Died += MonsterDespawned;
			monster.Finished += MonsterDespawned;
			_aliveMonsters.Add(monster);
		}

		private void MonsterDespawned(IMonster monster)
		{
			_aliveMonsters.Remove(monster);
			monster.Died -= MonsterDespawned;
			monster.Finished -= MonsterDespawned;
		}

		public void Dispose()
		{
			foreach (var monster in _aliveMonsters)
			{
				monster.Died -= MonsterDespawned;
				monster.Finished -= MonsterDespawned;
			}
		}
	}
}