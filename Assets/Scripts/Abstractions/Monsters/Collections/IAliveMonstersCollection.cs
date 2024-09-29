using System.Collections.Generic;

namespace TowerDefence.Abstractions.Monsters.Collections
{
	public interface IAliveMonstersCollection
	{
		IEnumerable<IMonster> AliveMonsters { get; }
	}
}