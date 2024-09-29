using TowerDefence.Abstractions.Monsters;
using TowerDefence.Abstractions.Monsters.Factories;
using TowerDefence.Infrastructure;
using UnityEngine;
using VContainer;

namespace TowerDefence.Monsters.Factories
{
	public class MonstersFactory : InjectPrefabFactory<Monster> , IMonstersFactory
	{
		public MonstersFactory(Transform parent, Monster prefab, IObjectResolver objectResolver) : base(parent, prefab, objectResolver)
		{
		}

		IMonster IMonstersFactory.Create() => base.Create();
	}
}