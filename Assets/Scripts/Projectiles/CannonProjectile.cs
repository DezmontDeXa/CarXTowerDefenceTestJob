namespace TowerDefence.Projectilies
{
	public class CannonProjectile : ProjectileBase
	{
		protected override void Update()
		{
			var translation = transform.forward * _speed;

			transform.Translate(translation);

			base.Update();
		}
	}
}