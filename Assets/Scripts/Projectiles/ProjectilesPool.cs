using UnityEngine;
using TowerDefence.Infrastructure.Pools;
using System;
using TowerDefence.Infrastructure;
using VContainer;

namespace TowerDefence.Projectilies
{
	public class ProjectilesPool<TProjectile> : PoolablesLinkedPool<TProjectile> 
		where TProjectile : Component, IPoolable
	{
		public ProjectilesPool(IFactory<TProjectile> factory, int maxPoolSize) : base(factory, maxPoolSize)
		{
		}

		[Inject]
        public ProjectilesPool(IFactory<TProjectile> factory, Settings settings) : base(factory, settings.MaxPoolSize)
		{
            
        }


        [Serializable]
		public class Settings
		{
			[SerializeField] private int _maxPoolSize = 10;

			public int MaxPoolSize => _maxPoolSize;
		}
	}
}