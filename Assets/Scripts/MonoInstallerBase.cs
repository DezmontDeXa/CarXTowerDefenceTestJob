using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Assets.Scripts
{
	[Serializable]
	public abstract class MonoInstallerBase : MonoBehaviour, IInstaller
	{
		public abstract void Install(IContainerBuilder builder);
	}
}
