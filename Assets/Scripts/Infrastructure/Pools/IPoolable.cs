using System;

namespace TowerDefence.Infrastructure.Pools
{
	public interface IPoolable
	{
		void OnSpawn(Action releaseAction);
		void OnDespawn();
	}
}
