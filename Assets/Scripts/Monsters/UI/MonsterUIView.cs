using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace TowerDefence.Monsters.UI
{
	public class MonsterUIView : MonoBehaviour
	{
		[SerializeField] private Image _hpFiller;
		private Camera _camera;

		[Inject]
		private void Constructor(Camera camera)
		{
			_camera = camera;
		}

		private void Update()
		{
			transform.LookAt(_camera.transform.position);
		}

		public void SetHPFillerAmount(float amount)
		{
			_hpFiller.fillAmount = amount;
		}
	}
}