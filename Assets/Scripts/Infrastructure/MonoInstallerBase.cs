using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TowerDefence.Infrastructure
{
	[Serializable]
	public abstract class MonoInstallerBase : MonoBehaviour, IInstaller
	{
		public abstract void Install(IContainerBuilder builder);
	}
}
