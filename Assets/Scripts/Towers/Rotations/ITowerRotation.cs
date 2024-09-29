using UnityEngine;

namespace TowerDefence.Towers.Rotations
{
	public  interface ITowerRotation
	{
		void ToDefault(float speed);

		bool ToTarget(Vector3 targetPosition, float pitchAngle, float speed);
	}
}
