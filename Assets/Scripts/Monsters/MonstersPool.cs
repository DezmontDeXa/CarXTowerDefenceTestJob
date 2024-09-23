using System;
using TowerDefence.Infrastructure;
using TowerDefence.Infrastructure.Pools;
using UnityEngine;
using VContainer;

namespace TowerDefence
{
	public class MonstersPool : PoolablesLinkedPool<Monster>
	{
		public MonstersPool(IFactory<Monster> factory, int maxPoolSize) : base(factory, maxPoolSize)
		{
		}

		[Inject]
		public MonstersPool(IFactory<Monster> factory, Settings settings) : base(factory, settings.MaxPoolSize)
		{
		}

		[Serializable]
		public class Settings
		{
			[SerializeField] private int _maxPoolSize;

			public int MaxPoolSize => _maxPoolSize;
		}
	}
}