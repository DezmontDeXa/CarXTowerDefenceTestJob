using UnityEngine;
using TowerDefence.Infrastructure.Pools;
using System;

namespace TowerDefence.Projectilies
{
	public abstract class ProjectileBase : MonoBehaviour, IPoolable
	{
		[SerializeField] protected float _speed = 0.2f;
		[SerializeField] private float _lifetime = 5f;

		private Action _releaseAction;
		private float _spawnedOn;

		protected virtual void Update()
		{
			if (_spawnedOn + _lifetime < Time.time)
				Release();
		}

		private void OnTriggerEnter(Collider other)
		{
			Release();
		}

		public void OnSpawn(Action releaseAction)
		{
			_releaseAction = releaseAction;
			_spawnedOn = Time.time;
			gameObject.SetActive(true);
		}

		public void OnDespawn()
		{
			gameObject.SetActive(false);
		}

		protected void Release()
		{
			_releaseAction?.Invoke();
		}
	}
}
