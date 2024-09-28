using System;
using System.Linq;
using UnityEngine;
using VContainer;

namespace TowerDefence.Towers
{
	[Serializable]
	public class NearestSingleTargetSelector : MonoBehaviour, ISingleTargetSelector
	{
		private AliveMonstersList _aliveMonstersList;
		
		[Inject]
		private void Constructor(AliveMonstersList aliveMonstersList)
		{
			_aliveMonstersList = aliveMonstersList;
		}

        public IMovableTarget SelectTarget(Vector3 towerPosition, ITowerData towerData)
		{
			if (!_aliveMonstersList.AliveMonsters.Any())
				return null;

			var minDistance = towerData.Range;

			Monster selectedMonster = null;

			foreach (var monster in _aliveMonstersList.AliveMonsters)
			{
				var distance = Vector3.Distance(towerPosition, monster.transform.position);
				if (distance < minDistance)
				{
					minDistance = distance;
					selectedMonster = monster;
				}
			}

			return selectedMonster;
		}
	}
}
