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
		[SerializeField] private TTowerData _towerData;
		[SerializeField] private InterfaceReference<ISingleTargetSelector> _singleTargetSelector;

		private float _lastShotTime;
		private IProjectilesPool<ProjectileBase> _projectilesPool;
		private float _loadingProgress;


		public TTowerData TowerData => _towerData;

		public float LoadingProgress
		{
			get => _loadingProgress;
			private set
			{
				_loadingProgress = value;

				LoadingProgressChanged?.Invoke(this, LoadingProgress);
			}
		}

		protected Transform ShootPoint => _shootPoint;

		public event Action<ITower, float> LoadingProgressChanged;


		[Inject]
		private void Constructor(IProjectilesPools projectilesPools)
		{
			_projectilesPool = projectilesPools.GetPoolByPrefab(_towerData.ProjectilePrefab);
		}


		protected void Update()
		{
			if (_shootPoint == null)
			{
				Debug.LogWarning($"{GetType().Name}: _shootPoint is null");
				return;
			}

			var target = SelectTarget();

			LoadingProgress = GetLoadingProgress();

			PrepareForTarget(target);

			if (!CanShoot(target))
				return;

			Shoot(target);

			_lastShotTime = Time.time;
		}

		private float GetLoadingProgress()
		{
			var loadedTime = Time.time - _lastShotTime;

			var amount = loadedTime / _towerData.ShootInterval;

			if (amount > 1f)
				amount = 1;

			return amount;
		}

		protected virtual void PrepareForTarget(IMonster target) { }

		protected virtual bool OnCanShoot() { return true; }

		protected virtual void OnShoot(TProjectile projectile, IMonster target) { }


		private IMonster SelectTarget()
		{
			return _singleTargetSelector.Value.SelectTarget(transform.position, _towerData.Range);
		}

		private bool CanShoot(IMonster target)
		{
			if (target == null)
				return false;

			if (LoadingProgress<1f)
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
