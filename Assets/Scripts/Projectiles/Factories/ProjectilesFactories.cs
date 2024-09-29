using System.Collections.Generic;
using TowerDefence.Abstractions.Projectilies;
using TowerDefence.Abstractions.Projectilies.Factories;
using TowerDefence.Infrastructure;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TowerDefence.Projectilies.Factories
{
	public class ProjectilesFactories
	{
		private GameObject _parent;
		private readonly IObjectResolver _objectResolver;
		private Dictionary<ProjectileBase, IFactory<ProjectileBase>> _factories
			= new Dictionary<ProjectileBase, IFactory<ProjectileBase>>();

		public ProjectilesFactories(IObjectResolver objectResolver)
		{
			_objectResolver = objectResolver;
			_parent = _objectResolver.Instantiate(new GameObject("Projectiles"));
		}

		public IFactory<ProjectileBase> GetFactoryByPrefab<TProjetile>(TProjetile prefab)
			where TProjetile : ProjectileBase
		{
			if (_factories.ContainsKey(prefab))
				return _factories[prefab];

			var newFactory = new ProjectilesFactory(_parent.transform, prefab, _objectResolver);

			_factories.Add(prefab, newFactory);

			return newFactory;
		}
	}
}
