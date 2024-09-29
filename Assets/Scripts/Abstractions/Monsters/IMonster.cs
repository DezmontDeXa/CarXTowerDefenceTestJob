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

		event Action<IMonster> Died;

		void TakeDamage(int damage);
	}
}