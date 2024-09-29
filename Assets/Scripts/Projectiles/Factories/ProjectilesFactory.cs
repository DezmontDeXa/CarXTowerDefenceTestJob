using TowerDefence.Abstractions.Projectilies;
using TowerDefence.Abstractions.Projectilies.Factories;
using TowerDefence.Infrastructure;
using UnityEngine;
using VContainer;

namespace TowerDefence.Projectilies.Factories
{
	public class ProjectilesFactory : InjectPrefabFactory<ProjectileBase>, IProjectilesFactory
	{
		public ProjectilesFactory(Transform parent, ProjectileBase prefab, IObjectResolver objectResolver) : base(parent, prefab, objectResolver)
		{
		}
	}
}