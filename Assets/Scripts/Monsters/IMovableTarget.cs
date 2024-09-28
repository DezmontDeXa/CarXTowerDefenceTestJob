using UnityEngine;

namespace TowerDefence
{
	public interface IMovableTarget
	{
		Vector3 Direction { get; }

		Vector3 Position { get; }

		float Speed { get; }
		Vector3 Velocity { get; }
	}
}