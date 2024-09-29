using AYellowpaper;
using TowerDefence.Abstractions.Towers;
using UnityEngine;

namespace TowerDefence.Towers.UI
{
	public class TowerUIPresenter : MonoBehaviour
	{
		[SerializeField] private InterfaceReference<ITower> _tower;
		[SerializeField] private TowerUIView _view;

		private void OnEnable()
		{
			_tower.Value.LoadingProgressChanged += TowerLoadingProgressChanged;
			_view.SetProgress(0);
		}

		private void OnDisable()
		{
			_tower.Value.LoadingProgressChanged -= TowerLoadingProgressChanged;
		}

		private void TowerLoadingProgressChanged(ITower arg1, float arg2)
		{
			_view.SetProgress(arg2);
		}
	}
}
