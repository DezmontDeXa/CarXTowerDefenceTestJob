using System;
using UnityEngine;

namespace TowerDefence.Towers
{
	public interface ISingleTargetSelector
	{
		IMovableTarget SelectTarget(Vector3 towerPosition, ITowerData towerData);
	}
}
