using System;
using TowerDefence.Abstractions.Monsters;
using TowerDefence.Infrastructure.Pools;
using UnityEngine;

namespace TowerDefence.Monsters
{
	public class Monster : MonoBehaviour, IPoolable, IMonster
	{
		[SerializeField] private float _speed = 1f;
		[SerializeField] private int _maxHP = 30;

		private Transform _moveTarget;
		const float _reachDistance = 0.3f;
		private float _lostDistanace = 0;

		private int _hp;
		private Rigidbody _rigidbody;
		private Action _releaseAction;
		private Vector3 _direction;
		private Vector3 _actualVelocity;

		public Vector3 Position => transform.position;

		public Vector3 Direction => _direction;

		public Vector3 Velocity => _direction * _speed;

		public float Speed => _speed;

		public event Action<IMonster> Died;

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		private void Update()
		{
			if (_moveTarget == null)
				return;

			_lostDistanace = Vector3.Distance(transform.position, _moveTarget.transform.position);

			if (_lostDistanace <= _reachDistance)
			{
				_releaseAction?.Invoke();
				return;
			}

			_direction = (_moveTarget.transform.position - transform.position).normalized;

			var moveOffset = Velocity * Time.deltaTime;

			transform.position += moveOffset;
		}

		public void SetMoveTarget(Transform target)
		{
			_moveTarget = target;
		}

		public void TakeDamage(int damage)
		{
			_hp -= damage;

			if (_hp <= 0)
				Die();
		}

		private void Die()
		{
			Died?.Invoke(this);
			_releaseAction?.Invoke();
		}

		public void OnSpawn(Action releaseAction)
		{
			_releaseAction = releaseAction;

			_hp = _maxHP;

			gameObject.SetActive(true);
		}

		public void OnDespawn()
		{
			// NOTHING
			gameObject.SetActive(false);
		}
	}
}