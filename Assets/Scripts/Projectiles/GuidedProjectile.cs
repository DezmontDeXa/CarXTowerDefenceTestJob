using UnityEngine;

namespace TowerDefence.Projectilies
{
	public class GuidedProjectile : ProjectileBase
	{
		private Transform _target;

		protected override void Update()
		{
			if (_target == null)
			{
				Destroy(gameObject);
				return;
			}

			var translation = _target.transform.position - transform.position;

			translation = CeilingSpeed(translation);

			transform.Translate(translation);

			base.Update();
		}

		public void SetTarget(Transform target)
		{
			_target = target;
		}

		private Vector3 CeilingSpeed(Vector3 translation)
		{
			if (translation.magnitude > _speed)
				translation = translation.normalized * _speed;
			return translation;
		}

	}
}
