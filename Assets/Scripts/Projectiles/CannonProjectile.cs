using TowerDefence.Abstractions.Projectilies;
using UnityEngine;

namespace TowerDefence.Projectilies
{
	public class CannonProjectile : ProjectileBase
	{
		private Rigidbody _rigidbody;

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		public override void OnDespawn()
		{
			base.OnDespawn();

			_rigidbody.velocity = Vector3.zero;
		}

		public void Push(Vector3 force)
		{
			_rigidbody.AddForce(force, ForceMode.VelocityChange);
		}
	}
}