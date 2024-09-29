using TowerDefence.Abstractions.Monsters;
using TowerDefence.Abstractions.Towers.TargetSelectors;
using TowerDefence.Projectilies;
using TowerDefence.Towers.Magics.Data;
using UnityEngine;

namespace TowerDefence.Towers.Magics
{
	[RequireComponent(typeof(ISingleTargetSelector))]
	public class MagicTower : TowerBase<MagicTowerData, GuidedProjectile>
	{
		protected override void OnShoot(GuidedProjectile projectile, IMonster monster)
		{
			base.OnShoot(projectile, monster);

			projectile.SetTarget(monster);
		}
	}
}
