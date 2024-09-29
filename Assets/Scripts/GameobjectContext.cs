using TowerDefence.Infrastructure;
using UnityEngine;
using VContainer.Unity;

namespace TowerDefence
{
	public class GameobjectContext : LifetimeScope
	{
		[SerializeField] private MonoInstallerBase[] _monoInstallers;

	}
}
