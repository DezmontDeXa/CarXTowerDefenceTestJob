using System;
using System.Threading;
using TowerDefence.Projectilies;
using UnityEngine;

namespace TowerDefence.Towers
{
	public class SimpleTower : TowerBase<GuidedProjectile>
	{
		protected override void OnShoot(IMovableTarget monster, GuidedProjectile projectile)
		{
			base.OnShoot(monster, projectile);

			projectile.SetTarget(monster);
		}
	}
}
