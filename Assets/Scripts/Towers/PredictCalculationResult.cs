using UnityEngine;

namespace TowerDefence.Towers
{
	public class PredictCalculationResult
	{
		public PredictCalculationResult(ShootCalculationResult shootCalculationResult, Vector3 position, float minimalVelocity)
		{
			ShootCalculationResult = shootCalculationResult;
			Position = position;
			MinimalVelocity = minimalVelocity;

			Flytime = ShootCalculationResult.Flytime;
		}

		public Vector3 Position { get; private set; }
		public float Flytime { get; private set; }
		public float MinimalVelocity { get; private set; }
		public ShootCalculationResult ShootCalculationResult { get; private set; }

	}
}
