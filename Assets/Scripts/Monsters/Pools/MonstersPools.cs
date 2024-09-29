using System;
using System.Collections.Generic;
using TowerDefence.Monsters.Factories;
using UnityEngine;

namespace TowerDefence.Monsters.Pools
{
	public class MonstersPools
	{
		private readonly MonstersFactories _factories;
		private readonly Settings _poolsSettings;
		private Dictionary<Monster, MonstersPool> _pools
			= new Dictionary<Monster, MonstersPool>();

		public MonstersPools(MonstersFactories factories, Settings settings)
		{
			_factories = factories;
			_poolsSettings = settings;
		}

		public MonstersPool GetPoolByPrefab<TMonster>(TMonster prefab)
			where TMonster : Monster
		{
			if (_pools.ContainsKey(prefab))
				return _pools[prefab];

			var factory = _factories.GetFactoryByPrefab<TMonster>(prefab); ;

			var newPool = new MonstersPool(factory, _poolsSettings.MaxPoolSize);

			_pools.Add(prefab, newPool);

			return newPool;
		}

		[Serializable]
		public class Settings
		{
			[SerializeField] private int _maxPoolSize = 50;

			public int MaxPoolSize => _maxPoolSize;
		}
	}
}
