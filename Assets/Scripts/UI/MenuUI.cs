using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameUtils;

namespace MyGame
{
	public class MenuUI : MonoBehaviour
	{
		public Text m_stars;
		public Text m_diamonds;
		public Image m_rank;
		public Image m_avatar;
		public Image m_expDisc;

		public uint stars { set { m_stars.text = Utils.ToMoney(value); } }
		public uint diamonds { set { m_diamonds.text = Utils.ToMoney(value); } }

		public void SetExpDisc(uint currentExp, ushort level)
		{
			uint neededExp = GameData.GetNeededExp(level);
			float factor = (float)currentExp / neededExp;
			m_expDisc.fillAmount = factor;
		}
	}
}
