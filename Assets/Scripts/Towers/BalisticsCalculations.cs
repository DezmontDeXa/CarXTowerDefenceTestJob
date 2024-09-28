using System;
using UnityEngine;

namespace TowerDefence.Towers
{
	public static class BalisticsCalculations
	{
		public static float GetMinimalVelocity(Vector3 shootPoint, Vector3 target)
		{
			var toTargetVector = target - shootPoint;
			var height = toTargetVector.y;
			toTargetVector.y = 0;
			var distance = toTargetVector.magnitude;

			var minimumVelocity = GetMinimalVelocity(distance, height, Physics.gravity.magnitude);

			minimumVelocity += 0.5f; //buffer

			return minimumVelocity;
		}

		public static float GetMinimalVelocity(float distance, float height, float gravity)
		{
			var d = distance;
			var h = height;
			var g = gravity;
			return Mathf.Sqrt((g * (h + Mathf.Sqrt(d * d + h * h))));
		}
		
		public static ShootCalculationResult CalculateShoot(Vector3 shootPoint, Vector3 targetPoint, float velocity, Trajectory trajectory)
		{
			Vector2 dir;
			dir.x = targetPoint.x - shootPoint.x;
			dir.y = targetPoint.z - shootPoint.z;

			float x = dir.magnitude;
			float y = targetPoint.y - shootPoint.y;

			dir = dir.normalized;

			var g = Physics.gravity.magnitude;
			var v = velocity;
			var vv = v * v;
			var vvvv = vv * vv;
			var D = vvvv - g * (g * x * x + 2f * vv * y);

			if (D < 0)
			{
				float requiredVelocity = GetMinimalVelocity(x, y, g);

				Debug.LogWarning($"Velocity insuffience for range. Required velocity = {requiredVelocity}");

				return default;
			}

			switch (trajectory)
			{
				case Trajectory.Hightest:

					var tanAMax = (vv + Mathf.Sqrt(D)) / (g * x);
					var cosAMax = Mathf.Cos(Mathf.Atan(tanAMax));
					var sinAMax = cosAMax * tanAMax;

					CalculateResults(dir, y, g, v, tanAMax, cosAMax, sinAMax, out var shootDirectionMax, out var launchAngleMax, out var flytimeMax);

					return new ShootCalculationResult(shootDirectionMax, launchAngleMax, flytimeMax, shootPoint, targetPoint, velocity);

				case Trajectory.Lowest:

					var tanAMin = (vv - Mathf.Sqrt(D)) / (g * x);
					var cosAMin = Mathf.Cos(Mathf.Atan(tanAMin));
					var sinAMin = cosAMin * tanAMin;

					CalculateResults(dir, y, g, v, tanAMin, cosAMin, sinAMin, out var shootDirectionMin, out var launchAngleMin, out var flytimeMin);

					return new ShootCalculationResult(shootDirectionMin, launchAngleMin, flytimeMin, shootPoint, targetPoint, velocity);

				default:
					throw new System.NotImplementedException();
			}
		}

		public static PredictCalculationResult PredictTargetPosition(
			Vector3 shootPoint, Vector3 target, Vector3 targetVelocity, Trajectory trajectory,
			float flytimePrecision = 0.01f, float prevFlytime = 0, Vector3 prevPredicted = default)
		{
			if (prevPredicted == default)
				prevPredicted = target;

			var minimalVelocity = GetMinimalVelocity(shootPoint, prevPredicted);
			var shootCalculations = CalculateShoot(shootPoint, prevPredicted, minimalVelocity, trajectory);
			var flytime = shootCalculations.Flytime;
			var predictedPosition = target + targetVelocity * flytime;

			if (targetVelocity == Vector3.zero)
				return new PredictCalculationResult(shootCalculations, predictedPosition, minimalVelocity);

			if (Mathf.Abs(prevFlytime - flytime) < flytimePrecision)
				return new PredictCalculationResult(shootCalculations, predictedPosition, minimalVelocity);

			return PredictTargetPosition(shootPoint, target, targetVelocity, trajectory, flytimePrecision, flytime, predictedPosition);
		}

		private static void CalculateResults(Vector2 dir, float y, float g, float v, float tanAMax, float cosAMax, float sinAMax, out Vector3 shootDirectionMax, out float launchAngleMax, out float flytimeMax)
		{
			shootDirectionMax = new Vector3(v * cosAMax * dir.x, v * sinAMax, v * cosAMax * dir.y);
			launchAngleMax = Mathf.Atan(tanAMax) * Mathf.Rad2Deg;
			var vsina2 = (v * sinAMax) * (v * sinAMax);
			var h = -y;
			flytimeMax = (v * sinAMax + Mathf.Sqrt(vsina2 + 2 * g * h)) / g;
		}

		public static void VisualizeTrajectory(Vector3 launchPoint, Vector3 target, float velocity, float angle, Color color, int steps = 100)
		{
			Vector2 dir;
			dir.x = target.x - launchPoint.x;
			dir.y = target.z - launchPoint.z;

			dir = dir.normalized;

			var cosA = Mathf.Cos(angle * Mathf.Deg2Rad);
			var sinA = Mathf.Sin(angle * Mathf.Deg2Rad);
			var g = Physics.gravity.magnitude;
			Vector3 prev = launchPoint;
			Vector3 next;
			for (int i = 1; i <= steps; i++)
			{
				var t = i / 10f;
				var dx = velocity * cosA * t;
				var dy = velocity * sinA * t - 0.5f * g * t * t;
				next = launchPoint + new Vector3(dir.x * dx, dy, dir.y * dx);
				Debug.DrawLine(prev, next, color);
				prev = next;
			}
		}

		[Serializable]
		public enum Trajectory
		{
			Hightest,
			Lowest
		}
	}
}
