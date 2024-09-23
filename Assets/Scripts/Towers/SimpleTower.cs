using System;
using System.Threading;
using TowerDefence.Projectilies;

namespace TowerDefence.Towers
{
	public class SimpleTower : TowerBase<GuidedProjectile>
	{
		protected override void OnShoot(Monster monster, GuidedProjectile projectile)
		{
			base.OnShoot(monster, projectile);

			projectile.SetTarget(monster.transform);
		}
	}
}
