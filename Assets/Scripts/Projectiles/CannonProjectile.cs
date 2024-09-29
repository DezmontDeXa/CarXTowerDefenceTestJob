using TowerDefence.Abstractions.Projectilies;
using UnityEngine;

namespace TowerDefence.Projectilies
{
	public class CannonProjectile : ProjectileBase
	{
		private float _calculatedFlyTime;
		private float _startTime;
		private Rigidbody _rigidbody;

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		public override void OnDespawn()
		{
			base.OnDespawn();
			_rigidbody.velocity = Vector3.zero;
			var real = Time.time - _startTime;
			var diff = _calculatedFlyTime - real;
			//Debug.Log($"Calculated: {_calculatedFlyTime}, Real: {real}, Diff: {diff}");
		}

		public void Push(Vector3 force)
		{
			_rigidbody.AddForce(force, ForceMode.VelocityChange);
		}

		public void DebugFlyTime(float calculatedFlyTime)
		{
			_calculatedFlyTime = calculatedFlyTime;
			_startTime = Time.time;
		}
	}
}