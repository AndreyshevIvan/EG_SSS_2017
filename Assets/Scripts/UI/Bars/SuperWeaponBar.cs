using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
	public class SuperWeaponBar : UIBar
	{
		protected override void OnAwakeEnd()
		{
			isTimerWork = false;
		}
		protected override void OnSetNewValue()
		{
			if (isActive)
			{
				return;
			}

			m_progress.fillAmount = (float)value / 100;
		}
		protected override void OnActivete()
		{
			m_icon.sprite = m_activeIcon;
			m_progress.CrossFadeAlpha(0, 0, true);
		}
		protected override void OnDisactivate()
		{
			m_icon.sprite = m_inactiveIcon;
			m_progress.CrossFadeAlpha(1, 0, true);
		}

		[SerializeField]
		private Image m_progress;
		[SerializeField]
		private Image m_icon;
		[SerializeField]
		private Sprite m_activeIcon;
		[SerializeField]
		private Sprite m_inactiveIcon;
	}
}
