namespace TowerDefence.Abstractions.Projectilies.Pools
{
	public interface IProjectilesPool<TProjectile>
		where TProjectile : ProjectileBase
	{
		public TProjectile Spawn();

		public void Release(TProjectile poolable);

	}
}