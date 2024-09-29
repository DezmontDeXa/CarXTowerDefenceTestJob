using TowerDefence.Infrastructure.Pools;
using TowerDefence.Abstractions.Projectilies.Pools;
using TowerDefence.Abstractions.Projectilies;
using TowerDefence.Infrastructure;

namespace TowerDefence.Projectilies.Pools
{
	public class ProjectilesPool<TProjectile> : PoolablesLinkedPool<TProjectile>, IProjectilesPool<TProjectile>
		where TProjectile : ProjectileBase
	{
		public ProjectilesPool(IFactory<TProjectile> factory, int maxPoolSize) : base(factory, maxPoolSize)
		{
		}
	}
}