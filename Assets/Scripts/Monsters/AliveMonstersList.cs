using System;
using System.Collections.Generic;
using TowerDefence.Infrastructure.Pools;
using VContainer.Unity;

namespace TowerDefence
{
	public class AliveMonstersList : IInitializable, IDisposable
	{
		private PoolablesLinkedPool<Monster> _pool;
		private List<Monster> _aliveMonsters;

		public IEnumerable<Monster> AliveMonsters => _aliveMonsters;

		public AliveMonstersList(PoolablesLinkedPool<Monster> pool)
		{
			_pool = pool;
			_aliveMonsters = new List<Monster>();
		}

		public void Initialize()
		{
			_pool.OnSpawned += Pool_OnSpawned;
			_pool.OnReleased += Pool_OnReleased;
		}

		public void Dispose()
		{
			_pool.OnSpawned -= Pool_OnSpawned;
			_pool.OnReleased -= Pool_OnReleased;
		}

		private void Pool_OnSpawned(Monster monster)
		{
			_aliveMonsters.Add(monster);
		}

		private void Pool_OnReleased(Monster monster)
		{
			_aliveMonsters.Remove(monster);
		}
	}
}