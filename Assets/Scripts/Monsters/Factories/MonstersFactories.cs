using System.Collections.Generic;
using TowerDefence;
using TowerDefence.Monsters;
using TowerDefence.Monsters.Factories;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TowerDefence.Monsters.Factories
{
	public class MonstersFactories
	{
		private readonly IObjectResolver _objectResolver;
		private Dictionary<Monster, MonstersFactory> _factories
			= new Dictionary<Monster, MonstersFactory>();
		private GameObject _parent;

		public MonstersFactories(IObjectResolver objectResolver)
		{
			_objectResolver = objectResolver;
			_parent = _objectResolver.Instantiate(new GameObject("Monsters"));
		}

		public MonstersFactory GetFactoryByPrefab<TMonster>(Monster prefab)
			where TMonster : Monster
		{
			if (_factories.ContainsKey(prefab))
				return _factories[prefab];

			var newFactory = new MonstersFactory(_parent.transform, prefab, _objectResolver);

			_factories.Add(prefab, newFactory);

			return newFactory;
		}
	}
}
