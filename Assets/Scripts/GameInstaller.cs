using TowerDefence;
using TowerDefence.Infrastructure;
using TowerDefence.Infrastructure.Pools;
using TowerDefence.Projectilies;
using TowerDefence.Towers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Assets.Scripts
{
	public class GameInstaller : MonoInstallerBase
	{
		[Header("Monsters")]
		[SerializeField] private MonstersFactory.Settings _monstersFactorySettings;
		[SerializeField] private MonstersPool.Settings _monstersPoolSettings;
		[SerializeField] private MonstersSpawner.Settings _monstersSpawnerSettings;

		[Header("Projectiles")]
		[SerializeField] private ProjectilesPool<CannonProjectile>.Settings _cannonProjetilesPoolSettings;
		[SerializeField] private ProjectilesPool<GuidedProjectile>.Settings _guidedProjetilesPoolSettings;
		[SerializeField] private ProjectilesFactory<CannonProjectile>.Settings _cannonProjetilesFactorySettings;
		[SerializeField] private ProjectilesFactory<GuidedProjectile>.Settings _guidedProjetilesFactorySettings;

		[Header("Towers")]
		[SerializeField] private CannonTowerData _cannonTowerData;

		public override void Install(IContainerBuilder builder)
		{
			BindMonsters(builder);

			BindProjectiles(builder);

			BindTowers(builder);
		}

		private void BindTowers(IContainerBuilder builder)
		{
			builder.RegisterInstance(_cannonTowerData).AsImplementedInterfaces().AsSelf();
		}

		private void BindMonsters(IContainerBuilder builder)
		{
			builder
				.Register<IFactory<Monster>, MonstersFactory>(Lifetime.Singleton)
				.WithParameter("settings", _monstersFactorySettings);

			builder
				.Register<PoolablesLinkedPool<Monster>, MonstersPool>(Lifetime.Singleton)
				.WithParameter("settings", _monstersPoolSettings);

			builder
				.RegisterEntryPoint<MonstersSpawner>(Lifetime.Singleton)
				.WithParameter("settings", _monstersSpawnerSettings);

			builder
				.Register<AliveMonstersList>(Lifetime.Singleton)
				.AsImplementedInterfaces()
				.AsSelf();
		}

		private void BindProjectiles(IContainerBuilder builder)
		{
			builder
				.Register<IFactory<CannonProjectile>, ProjectilesFactory<CannonProjectile>>(Lifetime.Singleton)
				.WithParameter("settings", _cannonProjetilesFactorySettings);

			builder
				.Register<IFactory<GuidedProjectile>, ProjectilesFactory<GuidedProjectile>>(Lifetime.Singleton)
				.WithParameter("settings", _guidedProjetilesFactorySettings);

			builder
				.Register<PoolablesLinkedPool<CannonProjectile>, ProjectilesPool<CannonProjectile>>(Lifetime.Transient)
				.WithParameter("settings", _cannonProjetilesPoolSettings)
				.AsImplementedInterfaces()
				.AsSelf();

			builder
				.Register<PoolablesLinkedPool<GuidedProjectile>, ProjectilesPool<GuidedProjectile>>(Lifetime.Transient)
				.WithParameter("settings", _guidedProjetilesPoolSettings)
				.AsImplementedInterfaces()
				.AsSelf();
		}
	}
}
