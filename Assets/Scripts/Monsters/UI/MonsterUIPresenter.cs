using AYellowpaper;
using TowerDefence.Abstractions.Monsters;
using UnityEngine;

namespace TowerDefence.Monsters.UI
{
	public class MonsterUIPresenter: MonoBehaviour
	{
		[SerializeField] private InterfaceReference<IMonster> _monster;
		[SerializeField] private MonsterUIView _view;

		private void OnEnable()
		{
			_monster.Value.HpChanged += OnHpChanged;

			OnHpChanged(_monster.Value.MaxHP);
		}

		private void OnDisable()
		{
			_monster.Value.HpChanged -= OnHpChanged;
		}

		private void OnHpChanged(int currentHp)
		{
			var amount = (float)currentHp / _monster.Value.MaxHP;

			_view.SetHPFillerAmount(amount);
		}
	}
}