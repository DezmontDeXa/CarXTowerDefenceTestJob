using System;
using UnityEngine;

namespace TowerDefence.Projectilies
{
	public class CannonProjectile : ProjectileBase
	{
		private float _calculatedFlyTime;
		private float _startTime;

		public Rigidbody Rigidbody { get; private set; }

		private void Awake()
		{
			Rigidbody = GetComponent<Rigidbody>();
		}

		public override void OnDespawn()
		{
			base.OnDespawn();
			Rigidbody.velocity = Vector3.zero;
			var real = Time.time - _startTime;
			var diff = _calculatedFlyTime - real;
			Debug.Log($"Calculated: {_calculatedFlyTime}, Real: {real}, Diff: {diff}");
		}

		internal void DebugFlyTime(float calculatedFlyTime)
		{
			_calculatedFlyTime = calculatedFlyTime;
			_startTime = Time.time;
		}
	}
}