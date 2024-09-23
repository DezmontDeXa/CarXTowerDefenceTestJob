using UnityEngine;

namespace TowerDefence.DamageDealers
{
	public class OnTriggerDamageDealer : MonoBehaviour
	{
		[SerializeField] private int _damage = 10;

		private void OnTriggerEnter(Collider other)
		{
			if (!other.gameObject.TryGetComponent<IDamageTaker>(out var damageTaker))
				return;

			damageTaker.TakeDamage(_damage);
		}
	}
}