using TowerDefence.Abstractions.Monsters;
using UnityEngine;

namespace TowerDefence.Abstractions.Towers.TargetSelectors
{
	public interface ISingleTargetSelector
	{
		IMonster SelectTarget(Vector3 towerPosition, float range);
	}
}
