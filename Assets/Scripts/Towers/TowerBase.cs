using AYellowpaper;
using System;
using TowerDefence.Abstractions.Monsters;
using TowerDefence.Abstractions.Projectilies;
using TowerDefence.Abstractions.Projectilies.Pools;
using TowerDefence.Abstractions.Towers;
using TowerDefence.Abstractions.Towers.TargetSelectors;
using UnityEngine;
using VContainer;

namespace TowerDefence.Towers
{
	public abstract class TowerBase<TTowerData, TProjectile> : MonoBehaviour, ITower
		where TTowerData : TowerData<TProjectile>
		where TProjectile : ProjectileBase
	{
		[SerializeField] private Transform _shootPoint;
		[SerializeField] private InterfaceReference<TTowerData> _towerData;
		[SerializeField] private InterfaceReference<ISingleTargetSelector> _singleTargetSelector;

		private float _lastShotTime;
		private IProjectilesPool<ProjectileBase> _projectilesPool;

		protected Transform ShootPoint => _shootPoint;

		protected TTowerData TowerData => _towerData.Value;

		public float LoadingProgress { get; private set; }

		public event Action<ITower, float> LoadingProgressChanged;

		[Inject]
		private void Constructor(IProjectilesPools projectilesPools)
		{
			_projectilesPool = projectilesPools.GetPoolByPrefab(_towerData.Value.ProjectilePrefab);
		}


		protected void Update()
		{
			if (_shootPoint == null)
			{
				Debug.LogWarning($"{GetType().Name}: _shootPoint is null");
				return;
			}

			var target = SelectTarget();

			PrepareForTarget(target);

			UpdateProgress();

			if (!CanShoot(target))
				return;

			Shoot(target);

			_lastShotTime = Time.time;
		}

		private void UpdateProgress()
		{
			LoadingProgress = GetLoadingProgress();

			LoadingProgressChanged.Invoke(this, LoadingProgress);
		}

		private float GetLoadingProgress()
		{
			var loadedTime = Time.time - _lastShotTime;

			var amount = loadedTime / _towerData.Value.ShootInterval;

			if (amount > 1f)
				amount = 1;

			return amount;
		}

		protected virtual void PrepareForTarget(IMonster target) { }

		protected virtual bool OnCanShoot() { return true; }

		protected virtual void OnShoot(TProjectile projectile, IMonster target) { }


		private IMonster SelectTarget()
		{
			return _singleTargetSelector.Value.SelectTarget(transform.position, _towerData.Value.Range);
		}

		private bool CanShoot(IMonster target)
		{
			if (target == null)
				return false;

			if (_lastShotTime + _towerData.Value.ShootInterval > Time.time)
				return false;

			return OnCanShoot();
		}

		private void Shoot(IMonster target)
		{
			var projectile = (TProjectile)_projectilesPool.Spawn();

			projectile.Position = _shootPoint.transform.position;

			OnShoot(projectile, target);
		}
	}
}
