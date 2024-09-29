using System;
using System.Linq;
using TowerDefence.Abstractions.Monsters;
using TowerDefence.Abstractions.Monsters.Collections;
using TowerDefence.Abstractions.Towers.TargetSelectors;
using UnityEngine;
using VContainer;

namespace TowerDefence.Towers.TargetSelectors
{
	[Serializable]
	public class NearestSingleTargetSelector : MonoBehaviour, ISingleTargetSelector
	{
		private IAliveMonstersCollection _aliveMonstersCollection;

		[Inject]
		private void Constructor(IAliveMonstersCollection aliveMonstersList)
		{
			_aliveMonstersCollection = aliveMonstersList;
		}

		public IMonster SelectTarget(Vector3 towerPosition, float range)
		{
			if (!_aliveMonstersCollection.AliveMonsters.Any())
				return null;

			var minDistance = range;

			IMonster selectedMonster = null;

			foreach (var monster in _aliveMonstersCollection.AliveMonsters)
			{
				var distance = Vector3.Distance(towerPosition, monster.Position);
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
