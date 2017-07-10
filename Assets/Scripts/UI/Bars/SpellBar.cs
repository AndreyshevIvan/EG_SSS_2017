using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
	public class SpellBar : UIBar
	{
		protected override void OnAwakeEnd()
		{
			isTimerWork = false;
		}
		protected override void OnSetNewValue()
		{
			Debug.Log("Update");
			m_progress.fillAmount = (float)value / 100;
		}

		[SerializeField]
		private Image m_progress;
		[SerializeField]
		private Image m_icon;
	}
}
