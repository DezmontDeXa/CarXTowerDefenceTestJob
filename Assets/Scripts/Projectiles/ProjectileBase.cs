using UnityEngine;
using TowerDefence.Infrastructure.Pools;
using System;
using TowerDefence.DamageDealers;

namespace TowerDefence.Projectilies
{
	public abstract class ProjectileBase : MonoBehaviour, IPoolable
	{
		[SerializeField] private float _yDeadZone = 0;
		private Action _releaseAction;

		protected virtual void Update()
		{
			if(transform.position.y < _yDeadZone)
				Release();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.gameObject.TryGetComponent<IDamageTaker>(out var damageTaker))
				return;

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
