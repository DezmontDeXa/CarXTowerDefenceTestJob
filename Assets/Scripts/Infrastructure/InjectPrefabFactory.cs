using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TowerDefence.Infrastructure
{
	public class InjectPrefabFactory<TPrefab> : IFactory<TPrefab>
		where TPrefab : Component
	{
		private readonly Transform _parent;
		private readonly TPrefab _prefab;
		private readonly IObjectResolver _objectResolver;

		public InjectPrefabFactory(Transform parent, TPrefab prefab, IObjectResolver objectResolver)
		{
			_parent = parent;
			_prefab = prefab;
			_objectResolver = objectResolver;
		}

		public TPrefab Create()
		{
			return _objectResolver.Instantiate(_prefab, _parent);
		}
	}
}