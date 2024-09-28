using UnityEngine;

namespace TowerDefence.Towers
{
	public class ShootCalculationResult
	{
		public Vector3 Direction { get; private set; }
		public float Flytime { get; private set; }
		public Vector3 ShootPoint { get; }
		public Vector3 TargetPoint { get; }
		public float Velocity { get; }
		public float Angle { get; private set; }

		public ShootCalculationResult(
			Vector3 direction, float launchAngle, float flytime, Vector3 shootPoint, Vector3 targetPoint, float velocity)
		{
			Direction = direction;
			Flytime = flytime;
			ShootPoint = shootPoint;
			TargetPoint = targetPoint;
			Velocity = velocity;
			Angle = launchAngle;
		}
	}
}
