using System;
using TowerDefence.Abstractions.Monsters;
using TowerDefence.Infrastructure.Pools;
using UnityEngine;

namespace TowerDefence.Monsters
{
	public class Monster : MonoBehaviour, IPoolable, IMonster
	{
		[SerializeField][Min(0)] private float _speed = 1f;
		[SerializeField][Min(1)] private int _maxHP = 30;

		private const float REACH_DISTANCE = 0.3f;

		private Transform _moveTarget;
		private float _lostDistanace = 0;
		private int _hp;
		private Action _releaseAction;
		private Vector3 _direction;

		public Vector3 Position => transform.position;

		public Vector3 Direction => _direction;

		public Vector3 Velocity => _direction * _speed;

		public float Speed => _speed;

		public int MaxHP => _maxHP;

		public int HP=> _hp;

		public event Action<int> HpChanged;


		public event Action<IMonster> Died;
		public event Action<IMonster> Finished;


		private void Update()
		{
			if (_moveTarget == null)
			{
				Debug.LogWarning("Monster: Move target is null. Die.");
				Die();
				return;
			}

			if (IsFinished())
			{
				Finished?.Invoke(this);
				Release();
				return;
			}

			MoveToTarget();
		}


		private void MoveToTarget()
		{
			_direction = (_moveTarget.transform.position - transform.position).normalized;

			var moveOffset = Velocity * Time.deltaTime;

			transform.position += moveOffset;
		}

		private bool IsFinished()
		{
			_lostDistanace = Vector3.Distance(transform.position, _moveTarget.transform.position);

			return _lostDistanace <= REACH_DISTANCE;
		}

		private void Release()
		{
			_releaseAction?.Invoke();
		}

		public void SetMoveTarget(Transform target)
		{
			_moveTarget = target;
		}

		public void TakeDamage(int damage)
		{
			_hp -= damage;

			HpChanged?.Invoke(_hp);

			if (_hp <= 0)
				Die();
		}

		private void Die()
		{
			Died?.Invoke(this);
			Release();
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