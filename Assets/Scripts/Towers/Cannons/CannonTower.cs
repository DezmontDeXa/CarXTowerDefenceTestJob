using TowerDefence.Abstractions.Monsters;
using TowerDefence.Abstractions.Towers.TargetSelectors;
using TowerDefence.Projectilies;
using TowerDefence.Tools;
using TowerDefence.Towers.Cannon.Balistics;
using TowerDefence.Towers.Cannon.Data;
using UnityEngine;
using VContainer;

namespace TowerDefence.Towers.Cannon
{
	[RequireComponent(typeof(ISingleTargetSelector))]
	public class CannonTower : TowerBase<CannonTowerData, CannonProjectile>
	{
		[SerializeField] private Transform _yawTransform;
		[SerializeField] private Transform _pitchTransform;
		[SerializeField] private bool _showDebugInfo = false;

		private Vector3 _shootVector;
		private Vector3 _predictedPosition;
		private Quaternion _yawDefautRotation;
		private Quaternion _pitchDefaultRotation;
		private float _flytime;
		private GizmosDrawer _gizmosDrawer;
		private bool _targetCapruted;

		[Inject]
		private void Constructor(GizmosDrawer gizmosDrawer)
		{
			_gizmosDrawer = gizmosDrawer;
		}

		private void Awake()
		{
			_yawDefautRotation = _yawTransform.rotation;
			_pitchDefaultRotation = _pitchTransform.rotation;
		}

		protected override void OnShoot(CannonProjectile projectile, IMonster target)
		{
			base.OnShoot(projectile, target);

			projectile.Push(ShootPoint.forward * _shootVector.magnitude);

			if (_showDebugInfo)
			{
				projectile.DebugFlyTime(_flytime);

				var drawPosition = _predictedPosition;
				_gizmosDrawer.AddTemporaryTask(() =>
				{
					Gizmos.color = Color.yellow;
					Gizmos.DrawSphere(drawPosition, 0.4f);
				}, _flytime);
			}
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

				// to defaults
				RotateYaw(_yawDefautRotation.eulerAngles);
				RotatePitch(_pitchDefaultRotation.eulerAngles);

				return;
			}

			var predictResult = BalisticsCalculations.PredictTargetPosition(
				ShootPoint.position, target.Position, target.Velocity,
				TowerData.Trajectory, TowerData.PredictPrecision);

			_flytime = predictResult.Flytime;

			_shootVector = predictResult.ShootCalculationResult.Direction;

			_predictedPosition = predictResult.Position;

			var yawDirection = predictResult.Position - ShootPoint.position;
			yawDirection.y = 0;

			var pitchDirection = new Vector3(-predictResult.ShootCalculationResult.Angle, 0, 0);

			var yawReached = RotateYaw(yawDirection);
			var pitchReached = RotatePitch(pitchDirection);

			var pitchAndYawReached = yawReached && pitchReached;

			_targetCapruted = pitchAndYawReached;

			if (_showDebugInfo)
			{
				BalisticsCalculations.VisualizeTrajectory(
					ShootPoint.position, _predictedPosition, predictResult.MinimalVelocity,
					predictResult.ShootCalculationResult.Angle, Color.blue);
			}
		}

		private bool RotatePitch(Vector3 pitchDirection)
		{
			if (pitchDirection == Vector3.zero)
				return false;

			var frameDegress = TowerData.RotationSpeed * Time.deltaTime;

			var targetPitch = Quaternion.Euler(pitchDirection);

			var pitchReached = RotateAxis(_pitchTransform, targetPitch, frameDegress);

			return pitchReached;
		}

		private bool RotateYaw(Vector3 yawDirection)
		{
			if (yawDirection == Vector3.zero)
				return false;

			var frameDegress = TowerData.RotationSpeed * Time.deltaTime;

			var targetYaw = Quaternion.LookRotation(yawDirection);

			var yawReached = RotateAxis(_yawTransform, targetYaw, frameDegress);

			return yawReached;
		}

		private bool RotateAxis(Transform rotatedPart, Quaternion targetRotation, float frameDegress)
		{
			var actualRotation = rotatedPart.localRotation;

			var a = Quaternion.Angle(actualRotation, targetRotation);

			if (a == 0)
				return true;

			var t = Mathf.InverseLerp(0, a, frameDegress);

			var rotationReached = false;

			if (t >= 1)
			{
				t = 1;
				rotationReached = true;
			}

			var newRotation = Quaternion.Lerp(actualRotation, targetRotation, t);

			rotatedPart.localRotation = newRotation;

			return rotationReached;
		}
	}
}
