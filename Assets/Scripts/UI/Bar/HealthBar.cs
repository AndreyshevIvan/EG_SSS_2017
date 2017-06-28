using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
	public sealed class HealthBar : UIBar
	{
		public Text m_textField;
		public Image m_healthLine;
		public bool m_isShip;

		public override void SetPosition(Vector3 worldPosition)
		{
			Vector3 screenPosition = Utils.WorldToCanvas(worldPosition);
			transform.position = screenPosition;
		}

		protected override void OnAwakeEnd()
		{
			layout = GetComponent<HorizontalLayoutGroup>();
			maxValue = 1;
		}
		protected override void OnSetNewValue()
		{
			m_textField.text = value.ToString("0%");
			if (m_healthLine != null) m_healthLine.fillAmount = value;
		}
		protected override void InitSizing()
		{
			float barWidth = ((m_isShip) ? PLAYER_WIDTH : ENEMY_WIDTH);
			barWidth = Utils.GetFromSreen(barWidth);
			Utils.SetWidth(rect, barWidth);
			Utils.SetHeight(rect, barWidth / HEIGHT_FACTOR);
			m_textField.fontSize = Utils.GetFromSreen(FONT_FACTOR);

			if (!m_isShip)
			{
				return;
			}

			int paddingByFactor = Utils.GetFromSreen(PADDING_FACTOR);
			int padding = (paddingByFactor >= 1) ? paddingByFactor : 1;
			layout.padding = new RectOffset(padding, padding, padding, padding);
		}

		private HorizontalLayoutGroup layout { get; set; }

		private const float FONT_FACTOR = 0.03f;
		private const float PLAYER_WIDTH = 0.12f;
		private const float ENEMY_WIDTH = 0.06f;
		private const float HEIGHT_FACTOR = 3.2f;
		private const float PADDING_FACTOR = 0.0015f;
	}
}
