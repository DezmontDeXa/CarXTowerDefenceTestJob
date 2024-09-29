using System;

namespace TowerDefence.Abstractions.Towers
{
	public interface ITower
	{
		float LoadingProgress { get; }

		event Action<ITower, float> LoadingProgressChanged;
	}
}