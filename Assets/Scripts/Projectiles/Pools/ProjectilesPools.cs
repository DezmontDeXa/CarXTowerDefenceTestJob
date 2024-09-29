using System;
using System.Collections.Generic;
using TowerDefence.Abstractions.Projectilies;
using TowerDefence.Abstractions.Projectilies.Pools;
using TowerDefence.Projectilies.Factories;
using UnityEngine;

namespace TowerDefence.Projectilies.Pools
{
	public class ProjectilesPools : IProjectilesPools
	{
		private readonly ProjectilesFactories _projectilesFactories;
		private readonly Settings _poolsSettings;
		private Dictionary<ProjectileBase, IProjectilesPool<ProjectileBase>> _pools
			= new Dictionary<ProjectileBase, IProjectilesPool<ProjectileBase>>();

		public ProjectilesPools(ProjectilesFactories projectilesFactories, Settings settings)
		{
			_projectilesFactories = projectilesFactories;
			_poolsSettings = settings;
		}

		public IProjectilesPool<ProjectileBase> GetPoolByPrefab(ProjectileBase prefab)
		{
			if (_pools.ContainsKey(prefab))
				return _pools[prefab];

			var factory = _projectilesFactories.GetFactoryByPrefab(prefab);

			var newPool = new ProjectilesPool<ProjectileBase>(factory, _poolsSettings.MaxPoolSize);

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
