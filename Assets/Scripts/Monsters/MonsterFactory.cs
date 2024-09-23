using System;
using TowerDefence.Infrastructure;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TowerDefence
{
	public class MonstersFactory : IFactory<Monster>
	{
		private readonly Settings _settings;
		private readonly IObjectResolver _objectResolver;

		public MonstersFactory(Settings settings, IObjectResolver objectResolver)
        {
			_settings = settings;
			_objectResolver = objectResolver;
		}

        public Monster Create()
		{
			var monster = _objectResolver.Instantiate(_settings.MonsterPrefab);

			return monster;
		}

		[Serializable]
		public class Settings
		{
			[SerializeField] private Monster _monsterPrefab;

			public Monster MonsterPrefab=> _monsterPrefab;
		}
	}
}