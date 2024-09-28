using TowerDefence.Infrastructure.Pools;
using UnityEngine;
using VContainer;

namespace TowerDefence.Towers
{
	public abstract class TowerBase<TProjectile> : MonoBehaviour 
		where TProjectile : Component, IPoolable
	{
		[SerializeField] private Transform _shootPoint;

		private float _lastShotTime = -0.5f;
		private ITowerData _towerData;
		private ISingleTargetSelector _targetSelector;
		private PoolablesLinkedPool<TProjectile> _projectilesPool;

		protected Transform ShootPoint => _shootPoint;

		[Inject]
		private void Constructor(
			ITowerData towerData,
			PoolablesLinkedPool<TProjectile> projectilesPool)
		{
			_towerData = towerData;
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

			if (_lastShotTime + _towerData.ShootInterval > Time.time)
				return;

			if (!CanShoot())
				return;

			Shoot(target);

			_lastShotTime = Time.time;
		}

		protected virtual void PrepareForTarget(IMovableTarget target) { }

		protected virtual void OnShoot(IMovableTarget monster, TProjectile projectile) { }

		private IMovableTarget SelectTarget()
		{
			return _targetSelector.SelectTarget(transform.position, _towerData);
		}

		protected virtual bool CanShoot()
		{
			return true;
		}

		private void Shoot(IMovableTarget monster)
		{
			var projectile = _projectilesPool.Spawn();

			projectile.transform.position = _shootPoint.transform.position;

			OnShoot(monster, projectile);
		}
	}
}
