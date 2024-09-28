using System;
using UnityEngine.Pool;

namespace TowerDefence.Infrastructure.Pools
{
	public class PoolablesLinkedPool<T>  where T : class, IPoolable
	{
		private LinkedPool<T> _pool;
		private readonly IFactory<T> _factory;

		public event Action<T> OnSpawned;

		public event Action<T> OnReleased;

		public PoolablesLinkedPool(IFactory<T> factory, int maxPoolSize)
		{
			_factory = factory;
			_pool = new LinkedPool<T>(CreateItem, OnGetItem, OnReleaseItem, null, true, maxPoolSize);
		}

		public T Spawn()
		{
			var poolable = _pool.Get();

			OnSpawned?.Invoke(poolable);

			return poolable;
		}

		public void Release(T poolable)
		{
			_pool.Release(poolable);

			OnReleased?.Invoke(poolable);
		}

		private T CreateItem()
		{
			return _factory.Create();
		}

		private void OnGetItem(T t)
		{
			t.OnSpawn(()=>Release(t));
		}

		private void OnReleaseItem(T t)
		{
			t.OnDespawn();
		}

	}
}
