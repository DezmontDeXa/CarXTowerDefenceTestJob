using System;
using UnityEngine;

namespace TowerDefence.Abstractions.Monsters
{
	public interface IMonster
	{
		Vector3 Direction { get; }

		Vector3 Position { get; }

		float Speed { get; }

		Vector3 Velocity { get; }

		int MaxHP { get; }

		int HP { get; }

		event Action<IMonster> Died;
		event Action<IMonster> Finished;
		event Action<int> HpChanged;

		void TakeDamage(int damage);
	}
}