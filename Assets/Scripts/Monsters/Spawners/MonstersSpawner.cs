using System;
using TowerDefence.Abstractions.Monsters;
using TowerDefence.Abstractions.Monsters.Spawners;
using TowerDefence.Monsters.Pools;
using UnityEngine;
using VContainer.Unity;

namespace TowerDefence.Monsters.Spawners
{
	public class MonstersSpawner : ITickable, IMonstersSpawner
	{
		private float m_lastSpawn = -1;
		private MonstersPools _monstersPools;
		private readonly Settings _settings;

		public event Action<IMonstersSpawner, IMonster> MonsterSpawned;

		public MonstersSpawner(Settings settings, MonstersPools monstersPools)
		{
			_settings = settings;
			_monstersPools = monstersPools;
			m_lastSpawn = -_settings.Interval;
		}

		public void Tick()
		{
			if (Time.time <= m_lastSpawn + _settings.Interval)
				return;

			Spawn();
		}

		private void Spawn()
		{
			var randomMonsterPrefab = _settings.Monsters[UnityEngine.Random.Range(0, _settings.Monsters.Length)];

			var pool = _monstersPools.GetPoolByPrefab(randomMonsterPrefab);

			var monster = pool.Spawn();
			monster.transform.position = _settings.SpawnPosition.position;
			monster.SetMoveTarget(_settings.MoveTarget);
			m_lastSpawn = Time.time;

			MonsterSpawned?.Invoke(this, monster);
		}

		[Serializable]
		public class Settings
		{
			[SerializeField] private float _interval = 3f;
			[SerializeField] private Transform _moveTarget;
			[SerializeField] private Transform _spawnPosition;
			[SerializeField] private Monster[] _monsters;

			public float Interval => _interval;
			public Transform MoveTarget => _moveTarget;
			public Transform SpawnPosition => _spawnPosition;
			public Monster[] Monsters => _monsters;
		}
	}
}
