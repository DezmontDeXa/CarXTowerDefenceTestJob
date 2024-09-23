using System;
using TowerDefence.Infrastructure.Pools;
using UnityEngine;
using VContainer.Unity;

namespace TowerDefence
{
	public class MonstersSpawner : ITickable
	{
		private float m_lastSpawn = -1;
		private PoolablesLinkedPool<Monster> _monstersPool;
		private readonly Settings _settings;

        public MonstersSpawner(Settings settings, PoolablesLinkedPool<Monster> monstersPool)
        {
			_settings = settings;
			_monstersPool = monstersPool;
		}

		public void Tick()
		{
			if (Time.time <= m_lastSpawn + _settings.Interval)
				return;

			Spawn();
		}

		private void Spawn()
		{
			var monster = _monstersPool.Spawn();
			monster.transform.position = _settings.SpawnPosition.position;
			monster.SetMoveTarget(_settings.MoveTarget);
			m_lastSpawn = Time.time;
		}

		[Serializable]
		public class Settings
		{
			[SerializeField] private float _interval = 3f;
			[SerializeField] private Transform _moveTarget;
			[SerializeField] private Transform _spawnPosition;

			public float Interval => _interval;
			public Transform MoveTarget => _moveTarget;
			public Transform SpawnPosition => _spawnPosition;
		}
	}	
}
