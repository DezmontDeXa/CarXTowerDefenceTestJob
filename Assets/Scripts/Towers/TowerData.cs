using TowerDefence.Abstractions.Projectilies;
using UnityEngine;

namespace TowerDefence.Towers
{
	public abstract class TowerData<TProjectile> : ScriptableObject
		where TProjectile : ProjectileBase
	{
		[SerializeField] private float _shootInterval = 0.5f;
		[SerializeField] private float _range = 4f;
		[SerializeField] private TProjectile _projectilePrefab;

		public float Range => _range;
		public float ShootInterval => _shootInterval;
		public TProjectile ProjectilePrefab => _projectilePrefab;
	}
}
