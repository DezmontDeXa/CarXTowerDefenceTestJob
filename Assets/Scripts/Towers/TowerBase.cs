using System.Linq;
using TowerDefence.Infrastructure.Pools;
using TowerDefence.Projectilies;
using UnityEngine;
using VContainer;

namespace TowerDefence.Towers
{
	public abstract class TowerBase<TProjectile> : MonoBehaviour 
		where TProjectile : Component, IPoolable
	{
		[SerializeField] protected float _shootInterval = 0.5f;
		[SerializeField] protected float _range = 4f;
		[SerializeField] protected Transform _shootPoint;

		private float _lastShotTime = -0.5f;
		private AliveMonstersTracker _aliveMonstersTracker;
		private PoolablesLinkedPool<TProjectile> _projectilesPool;

		[Inject]
		private void Constructor(
			AliveMonstersTracker aliveMonstersTracker, 
			PoolablesLinkedPool<TProjectile> projectilesPool)
		{
			_aliveMonstersTracker = aliveMonstersTracker;
			_projectilesPool = projectilesPool;
		}

		protected virtual void Update()
		{
			if ( _shootPoint == null)
			{
				Debug.LogWarning("SimpleTower: _shootPoint is null");
				return;
			}

			var target = SelectTarget();

			PrepareForTarget(target);

			if (target == null)
				return;

			if (_lastShotTime + _shootInterval > Time.time)
				return;

			Shoot(target);

			_lastShotTime = Time.time;
		}

		protected virtual void PrepareForTarget(Monster target)
		{
			// rotate towards target
			// loading animations
			// etc.
		}

		protected virtual Monster SelectTarget()
		{
			if (!_aliveMonstersTracker.AliveMonsters.Any())
				return null;

			var minDistance = _range;

			Monster selectedMonster = null;

			foreach (var monster in _aliveMonstersTracker.AliveMonsters)
			{
				var distance = Vector3.Distance(transform.position, monster.transform.position);
				if (distance < minDistance)
				{
					minDistance = distance;
					selectedMonster = monster;
				}
			}

			return selectedMonster;
		}

		private void Shoot(Monster monster)
		{
			var projectile = _projectilesPool.Spawn();

			projectile.transform.position = _shootPoint.transform.position;

			OnShoot(monster, projectile);
		}

		protected virtual void OnShoot(Monster monster, TProjectile projectile) { }
	}
}
