using UnityEngine;

namespace TowerDefence.Projectilies
{
	public class GuidedProjectile : ProjectileBase
	{
		[SerializeField] protected float _speed = 0.2f;

		private IMovableTarget _target;

		protected override void Update()
		{
			if (_target == null)
			{
				Destroy(gameObject);
				return;
			}

			var translation = _target.Position - transform.position;

			translation = CeilingSpeed(translation);

			transform.Translate(translation);

			base.Update();
		}

		public void SetTarget(IMovableTarget target)
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
