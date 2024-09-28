using AYellowpaper;
using TowerDefence.Infrastructure.Pools;
using TowerDefence.Projectilies;
using TowerDefence.Tools;
using UnityEngine;
using VContainer;

namespace TowerDefence.Towers
{
	[RequireComponent(typeof(ISingleTargetSelector))]
	public class CannonTower : MonoBehaviour
	{
		[SerializeField] private Transform _shootPoint;
		[SerializeField] private Transform _yawTransform;
		[SerializeField] private Transform _pitchTransform;
		[SerializeField] private InterfaceReference<ISingleTargetSelector> _singleTargetSelector;
		[SerializeField] private bool _showDebugInfo = false;

		private Vector3 _shootVector;
		private Vector3 _predictedPosition;
		private Quaternion _yawDefautRotation;
		private Quaternion _pitchDefaultRotation;
		private float _flytime;
		private ICannonTowerData _cannonTowerData;
		private PoolablesLinkedPool<CannonProjectile> _projectilesPool;
		private GizmosDrawer _gizmosDrawer;
		private bool _targetCapruted;
		private float _lastShotTime;

		[Inject]
		private void Constructor(ICannonTowerData cannonTowerData, PoolablesLinkedPool<CannonProjectile> projectilesPool, GizmosDrawer gizmosDrawer)
		{
			_cannonTowerData = cannonTowerData;
			_projectilesPool = projectilesPool;
			_gizmosDrawer = gizmosDrawer;
		}

		private void Awake()
		{
			_yawDefautRotation = _yawTransform.rotation;
			_pitchDefaultRotation = _pitchTransform.rotation;
		}

		private void Update()
		{
			if (_shootPoint == null)
			{
				Debug.LogWarning("SimpleTower: _shootPoint is null");
				return;
			}

			var target = _singleTargetSelector.Value.SelectTarget(transform.position, _cannonTowerData);

			PrepareForTarget(target);

			if (target == null)
				return;

			if (_lastShotTime + _cannonTowerData.ShootInterval > Time.time)
				return;

			if (!_targetCapruted)
				return;

			Shoot();

			_lastShotTime = Time.time;
		}

		private void Shoot()
		{
			var projectile = _projectilesPool.Spawn();

			projectile.transform.position = _shootPoint.transform.position;

			projectile.Rigidbody.useGravity = true;

			projectile.Rigidbody.AddForce(_shootPoint.forward * _shootVector.magnitude, ForceMode.VelocityChange);

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

		private void PrepareForTarget(IMovableTarget target)
		{
			if (target == null)
			{
				_targetCapruted = false;

				// to defaults
				RotateYaw(_yawDefautRotation.eulerAngles);
				RotatePitch(_pitchDefaultRotation.eulerAngles);

				return;
			}

			var predictResult = BalisticsCalculations.PredictTargetPosition(_shootPoint.position, target.Position, target.Velocity, _cannonTowerData.Trajectory, _cannonTowerData.PredictPrecision);

			_flytime = predictResult.Flytime;

			_shootVector = predictResult.ShootCalculationResult.Direction;

			_predictedPosition = predictResult.Position;

			var yawDirection = predictResult.Position - _shootPoint.position;
			yawDirection.y = 0;

			var pitchDirection = new Vector3(-predictResult.ShootCalculationResult.Angle, 0, 0);

			var yawReached = RotateYaw(yawDirection);
			var pitchReached = RotatePitch(pitchDirection);

			var pitchAndYawReached = yawReached && pitchReached;

			_targetCapruted = pitchAndYawReached;

			if (_showDebugInfo)
			{
				BalisticsCalculations.VisualizeTrajectory(
					_shootPoint.position, _predictedPosition, predictResult.MinimalVelocity,
					predictResult.ShootCalculationResult.Angle, Color.blue);
			}
		}

		private bool RotatePitch(Vector3 pitchDirection)
		{
			var frameDegress = _cannonTowerData.RotationSpeed * Time.deltaTime;

			var targetPitch = Quaternion.Euler(pitchDirection);

			var pitchReached = RotateAxis(_pitchTransform, targetPitch, frameDegress);

			return pitchReached;
		}

		private bool RotateYaw(Vector3 yawDirection)
		{
			var frameDegress = _cannonTowerData.RotationSpeed * Time.deltaTime;

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
