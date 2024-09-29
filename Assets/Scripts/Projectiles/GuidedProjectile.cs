using System;
using TowerDefence.Abstractions.Monsters;
using TowerDefence.Abstractions.Projectilies;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace TowerDefence.Projectilies
{
	public class GuidedProjectile : ProjectileBase
	{
		[SerializeField] protected float _speed = 0.2f;

		private IMonster _target;
		private Vector3 _targetPosition;

		protected override void Update()
		{
			if (_target != null)
				_targetPosition = _target.Position;

			MoveToTarget(_targetPosition);

			if (_target == null && IsPointReached(_targetPosition))
				Release();

			base.Update();
		}

		private bool IsPointReached(Vector3 targetPosition)
		{
			return Vector3.Distance(transform.position, targetPosition) < 0.1f;
		}

		public void SetTarget(IMonster target)
		{
			if (target == null)
			{
				Debug.LogWarning("GuidedProjectile: Target cannon be null");
				return;
			}

			if (_target != null)
			{
				_target.Died -= OnTargetLost;
				_target.Finished -= OnTargetLost;
			}

			_target = target;

			_target.Died += OnTargetLost;
			_target.Finished += OnTargetLost;
		}

		private void MoveToTarget(Vector3 targetPosition)
		{
			var moveDirection = targetPosition - transform.position;

			transform.position += moveDirection.normalized * _speed * Time.deltaTime;
		}

		private void OnTargetLost(IMonster monster)
		{
			_target.Died -= OnTargetLost;
			_target.Finished -= OnTargetLost;

			_target = null;
		}

		public override void OnDespawn()
		{
			base.OnDespawn();

			if (_target == null)
				return;

			_target.Died -= OnTargetLost;
			_target.Finished -= OnTargetLost;

			_target = null;
		}
	}
}
