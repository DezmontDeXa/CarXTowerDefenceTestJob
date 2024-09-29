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
			_tower.Value.LoadingProgressChanged += Value_LoadingProgressChanged;
		}

		private void OnDisable()
		{
			_tower.Value.LoadingProgressChanged -= Value_LoadingProgressChanged;
		}

		private void Value_LoadingProgressChanged(ITower arg1, float arg2)
		{
			_view.SetProgress(arg2);
		}
	}
}
