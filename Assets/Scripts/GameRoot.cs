using TowerDefence.Infrastructure;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TowerDefence
{
	public class GameRoot : LifetimeScope
	{
		[SerializeField] private MonoInstallerBase[] _monoInstallers;

		protected override void Configure(IContainerBuilder builder)
		{
			base.Configure(builder);
			foreach (var monoInstaller in _monoInstallers)
			{
				monoInstaller.Install(builder);
			}
		}
	}
}
