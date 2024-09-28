using UnityEngine;
using static TowerDefence.Towers.BalisticsCalculations;

namespace TowerDefence.Towers
{
	[CreateAssetMenu(menuName = "Towers/Cannon Tower", fileName = "New Cannon Tower Data")]
	public class CannonTowerData : TowerDataBase, ICannonTowerData
	{
		[SerializeField] private float _predictPrecision = 0.01f;
		[SerializeField] private Trajectory _trajectory;
		[SerializeField] private float _rotationSpeed = 5f;

		public float PredictPrecision => _predictPrecision;
		public Trajectory Trajectory => _trajectory;
		public float RotationSpeed => _rotationSpeed;
	}
}
