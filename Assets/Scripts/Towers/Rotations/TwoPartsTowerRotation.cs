using UnityEngine;

namespace TowerDefence.Towers.Rotations
{
	public class TwoPartsTowerRotation : MonoBehaviour, ITowerRotation
	{
		[SerializeField] private Transform _aroundPoint;
		[SerializeField] private Transform _yawTransform;
		[SerializeField] private Transform _pitchTransform;

		private Quaternion _yawDefautRotation;
		private Quaternion _pitchDefaultRotation;
		private bool _firstTargetPositionSaved;
		private Vector3 _yaw;
		private Vector3 _pitch;

		private void Awake()
		{
			_yawDefautRotation = _yawTransform.rotation;
			_pitchDefaultRotation = _pitchTransform.rotation;
		}

		public void ToDefault(float speed)
		{
			if (_firstTargetPositionSaved)
			{
				RotateYaw(_yaw, speed);
				RotatePitch(_pitch, speed);
			}
			else
			{
				RotateYaw(_yawDefautRotation.eulerAngles, speed);
				RotatePitch(_pitchDefaultRotation.eulerAngles, speed);
			}
		}

		public bool ToTarget(Vector3 targetPosition, float pitchAngle, float speed)
		{
			var yawDirection = targetPosition - _aroundPoint.position;
			yawDirection.y = 0;

			pitchAngle = -pitchAngle;

			var pitchDirection = new Vector3(pitchAngle, 0, 0);

			if (!_firstTargetPositionSaved)
			{
				_yaw = yawDirection;
				_pitch = pitchDirection;
				_firstTargetPositionSaved = true;
			}

			var yawReached = RotateYaw(yawDirection, speed);

			var pitchReached = RotatePitch(pitchDirection, speed);

			return yawReached && pitchReached;
		}

		private bool RotatePitch(Vector3 pitchDirection, float speed)
		{
			if (pitchDirection == Vector3.zero)
				return false;

			var frameDegress = speed * Time.deltaTime;

			var targetPitch = Quaternion.Euler(pitchDirection);

			var pitchReached = RotateAxis(_pitchTransform, targetPitch, frameDegress);

			return pitchReached;
		}

		private bool RotateYaw(Vector3 yawDirection, float speed)
		{
			if (yawDirection == Vector3.zero)
				return false;

			var frameDegress = speed * Time.deltaTime;

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
