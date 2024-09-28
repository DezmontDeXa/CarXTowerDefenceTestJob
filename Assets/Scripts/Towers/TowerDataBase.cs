using UnityEngine;

namespace TowerDefence.Towers
{
	public abstract class TowerDataBase : ScriptableObject, ITowerData
	{
		[SerializeField] private float _shootInterval = 0.5f;
		[SerializeField] private float _range = 4f;

		public float Range => _range;
		public float ShootInterval => _shootInterval;
	}

	public interface ITowerData
	{
		float Range { get; }
		float ShootInterval { get; }
	}
}
