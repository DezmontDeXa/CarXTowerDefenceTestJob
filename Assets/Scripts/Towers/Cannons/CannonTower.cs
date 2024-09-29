using TowerDefence.Abstractions.Monsters;
using TowerDefence.Abstractions.Towers.TargetSelectors;
using TowerDefence.Projectilies;
using TowerDefence.Tools;
using TowerDefence.Towers.Cannon.Balistics;
using TowerDefence.Towers.Cannon.Balistics.Entities;
using TowerDefence.Towers.Cannon.Data;
using TowerDefence.Towers.Rotations;
using UnityEngine;
using VContainer;

namespace TowerDefence.Towers.Cannon
{
	[RequireComponent(typeof(ISingleTargetSelector))]
	[RequireComponent(typeof(ITowerRotation))]
	public class CannonTower : TowerBase<CannonTowerData, CannonProjectile>
	{
		private Vector3 _shootVector;
		private Vector3 _predictedPosition;
		private float _flytime;
		private bool _targetCapruted;
		private ITowerRotation _towerRotation;

#if UNITY_EDITOR
		[SerializeField] private bool _showDebugInfo = false;
		private GizmosDrawer _gizmosDrawer;

		[Inject]
		private void Constructor(GizmosDrawer gizmosDrawer)
		{
			_gizmosDrawer = gizmosDrawer;
		}
#endif

		protected void Awake()
		{
			_towerRotation = GetComponent<ITowerRotation>();
		}

		protected override void OnShoot(CannonProjectile projectile, IMonster target)
		{
			base.OnShoot(projectile, target);

			projectile.Push(ShootPoint.forward * _shootVector.magnitude);

#if UNITY_EDITOR

			if (_showDebugInfo)
			{
				var drawPosition = _predictedPosition;
				_gizmosDrawer.AddTemporaryTask(() =>
				{
					Gizmos.color = Color.yellow;
					Gizmos.DrawSphere(drawPosition, 0.4f);
				}, _flytime);
			}
#endif
		}

		protected override bool OnCanShoot()
		{
			return _targetCapruted;
		}

		protected override void PrepareForTarget(IMonster target)
		{
			if (target == null)
			{
				_targetCapruted = false;

				if (_towerRotation != null)
					_towerRotation.ToDefault(TowerData.RotationSpeed);

				return;
			}

			var predictResult = BalisticsCalculations.PredictTargetPosition(
				ShootPoint.position, target.Position, target.Velocity,
				TowerData.Trajectory, TowerData.PredictPrecision);

			_flytime = predictResult.Flytime;

			_shootVector = predictResult.ShootCalculationResult.Direction;

			_predictedPosition = predictResult.Position;

			var pitchAndYawReached = Rotate(predictResult);

			_targetCapruted = pitchAndYawReached;

#if UNITY_EDITOR
			if (_showDebugInfo)
			{
				BalisticsCalculations.VisualizeTrajectory(
					ShootPoint.position, _predictedPosition, predictResult.MinimalVelocity,
					predictResult.ShootCalculationResult.Angle, Color.blue);
			}
#endif
		}

		private bool Rotate(PredictCalculationResult predictResult)
		{
			if (_towerRotation == null)
				return true;

			var pitchAndYawReached = _towerRotation.ToTarget(predictResult.Position, predictResult.ShootCalculationResult.Angle, TowerData.RotationSpeed);

			return pitchAndYawReached;
		}
	}
}
