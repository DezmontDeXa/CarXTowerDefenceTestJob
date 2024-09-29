using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.Towers.UI
{
	public class TowerUIView : MonoBehaviour
	{
		[SerializeField] private Image _fillerImage;

		public void SetProgress(float progress)
		{
			_fillerImage.fillAmount = progress;
		}
	}
}
