using UnityEngine;
using System;
using TowerDefence.Infrastructure;
using VContainer;
using VContainer.Unity;

namespace TowerDefence.Projectilies
{
	public class ProjectilesFactory<TProjectile> : IFactory<TProjectile>
		where TProjectile: ProjectileBase
	{
		private readonly Settings _settings;
		private readonly IObjectResolver _objectResolver;

		public ProjectilesFactory(Settings settings, IObjectResolver objectResolver)
        {
			_settings = settings;
			_objectResolver = objectResolver;
		}

        public TProjectile Create()
		{
			var projectile = _objectResolver.Instantiate(_settings.ProjectilePrefab);

			return (TProjectile)projectile;
		}

		[Serializable]
		public class Settings
		{
			[SerializeField] private ProjectileBase _projectilePrefab;

			public ProjectileBase ProjectilePrefab => _projectilePrefab;
		}
	}
}