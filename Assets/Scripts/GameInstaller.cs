using TowerDefence.Abstractions.Monsters.Collections;
using TowerDefence.Infrastructure;
using TowerDefence.Monsters.Factories;
using TowerDefence.Monsters.Pools;
using TowerDefence.Monsters.Spawners;
using TowerDefence.Projectilies.Factories;
using TowerDefence.Projectilies.Pools;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TowerDefence
{
	public class GameInstaller : MonoInstallerBase
	{
		[Header("Monsters")]
		[SerializeField] private MonstersPools.Settings _monstersPoolSettings;
		[SerializeField] private MonstersSpawner.Settings _monstersSpawnerSettings;

		[Header("Projectiles")]
		[SerializeField] private ProjectilesPools.Settings _projetilesPoolSettings;

		public override void Install(IContainerBuilder builder)
		{
			BindMonsters(builder);

			BindProjectiles(builder);
		}

		private void BindMonsters(IContainerBuilder builder)
		{
			builder
				.Register<MonstersFactories>(Lifetime.Singleton)
				.AsSelf();

			builder
				.Register<MonstersPools>(Lifetime.Singleton)
				.WithParameter("settings", _monstersPoolSettings)
				.AsImplementedInterfaces()
				.AsSelf();


			builder
				.RegisterEntryPoint<MonstersSpawner>(Lifetime.Singleton)
				.WithParameter("settings", _monstersSpawnerSettings)
				.AsSelf();

			builder
				.Register<AliveMonstersCollection>(Lifetime.Singleton)
				.AsImplementedInterfaces();
		}

		private void BindProjectiles(IContainerBuilder builder)
		{
			builder
				.Register<ProjectilesFactories>(Lifetime.Singleton)
				.AsSelf();

			builder
				.Register<ProjectilesPools>(Lifetime.Singleton)
				.WithParameter("settings", _projetilesPoolSettings)
				.AsImplementedInterfaces()
				.AsSelf();
		}


	}
}
