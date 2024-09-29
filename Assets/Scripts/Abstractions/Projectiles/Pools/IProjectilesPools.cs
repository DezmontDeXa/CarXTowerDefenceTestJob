namespace TowerDefence.Abstractions.Projectilies.Pools
{
	public interface IProjectilesPools
	{
		IProjectilesPool<ProjectileBase> GetPoolByPrefab(ProjectileBase prefab);
	}
}