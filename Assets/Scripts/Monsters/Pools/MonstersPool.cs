using System;
using TowerDefence.Abstractions.Monsters;
using TowerDefence.Abstractions.Monsters.Pools;
using TowerDefence.Infrastructure;
using TowerDefence.Infrastructure.Pools;
using UnityEngine;
using VContainer;

namespace TowerDefence.Monsters.Pools
{
	public class MonstersPool : PoolablesLinkedPool<Monster>, IMonstersPool
	{
		public MonstersPool(IFactory<Monster> factory, int maxPoolSize) : base(factory, maxPoolSize)
		{
		}

		[Inject]
		public MonstersPool(IFactory<Monster> factory, Settings settings) : base(factory, settings.MaxPoolSize)
		{
		}

		public void Release(IMonster poolable)=>base.Release(poolable as Monster);

		IMonster IMonstersPool.Spawn() => base.Spawn();

		[Serializable]
		public class Settings
		{
			[SerializeField] private int _maxPoolSize;

			public int MaxPoolSize => _maxPoolSize;
		}
	}
}