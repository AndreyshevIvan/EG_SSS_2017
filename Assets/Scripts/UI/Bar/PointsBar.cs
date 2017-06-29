using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
	public class PointsBar : UIBar
	{
		protected override void OnAwakeEnd()
		{
			m_field = GetComponent<Text>();
			OnSetNewValue();
		}
		protected override void OnSetNewValue()
		{
			int intValue = (int)value;
			m_field.text = intValue.ToString(PATTERN);
		}

		private Text m_field;

		private const string PATTERN = "000 000 000 000";
	}
}
