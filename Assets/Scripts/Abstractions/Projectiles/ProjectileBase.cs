using UnityEngine;
using TowerDefence.Infrastructure.Pools;
using System;
using TowerDefence.Abstractions.Projectilies;
using TowerDefence.Abstractions.Monsters;

namespace TowerDefence.Abstractions.Projectilies
{
	public abstract class ProjectileBase : MonoBehaviour, IPoolable
	{
		[SerializeField] private int _damage = 10;
		[SerializeField] private float _yDeadZone = 0;
		private Action _releaseAction;

		public Vector3 Position
		{
			get => transform.position;
			set => transform.position = value;
		}

		protected virtual void Update()
		{
			if(transform.position.y < _yDeadZone)
				Release();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.gameObject.TryGetComponent<IMonster>(out var monster))
				return;

			monster.TakeDamage(_damage);

			Release();
		}

		public virtual void OnSpawn(Action releaseAction)
		{
			_releaseAction = releaseAction;
			gameObject.SetActive(true);
		}

		public virtual void OnDespawn()
		{
			gameObject.SetActive(false);
		}

		protected void Release()
		{
			_releaseAction?.Invoke();
		}
	}
}
