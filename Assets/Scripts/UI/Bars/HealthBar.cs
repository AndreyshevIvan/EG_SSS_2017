using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using MyGame.GameUtils;

namespace MyGame
{
	public sealed class HealthBar : UIBar
	{
		protected override void OnAwakeEnd()
		{
			layout = GetComponent<HorizontalLayoutGroup>();
			Fade(0, 0);

			if (m_isShip)
			{
				m_offset.x = Utils.GetFromSreen(OFFSET_FROM_SCREEN_X);
				m_offset.y = Utils.GetFromSreen(OFFSET_FROM_SCREEN_Y);
			}
		}
		protected override void InitSizing()
		{
			float barWidth = (m_isShip) ? PLAYER_WIDTH : ENEMY_WIDTH;
			barWidth = Utils.GetFromSreen(barWidth);
			Utils.SetWidth(rect, barWidth);
			Utils.SetHeight(rect, barWidth * HEIGHT_FROM_WIDTH);
			m_textField.fontSize = Utils.GetFromSreen(FONT_FACTOR);

			if (!m_isShip)
			{
				return;
			}

			int paddingByFactor = Utils.GetFromSreen(PADDING_FROM_SCREEN);
			int padding = (paddingByFactor >= 1) ? paddingByFactor : 1;
			layout.padding = new RectOffset(padding, padding, padding, padding);
		}
		protected override void SetPosition(Vector3 worldPosition)
		{
			Vector2 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);
			transform.position = screenPosition + m_offset;
		}
		protected override void OnSetNewValue()
		{
			m_textField.text = value.ToString() + PATTERN;
			if (m_healthLine) m_healthLine.fillAmount = (float)(value) / 100.0f;
		}

		[SerializeField]
		private Text m_textField;
		[SerializeField]
		private Image m_healthLine;
		[SerializeField]
		private bool m_isShip;
		private Vector2 m_offset = new Vector2();

		private HorizontalLayoutGroup layout { get; set; }

		private const float FONT_FACTOR = 0.03f;

		private const float PLAYER_WIDTH = 0.12f;
		private const float ENEMY_WIDTH = 0.07f;
		private const float HEIGHT_FROM_WIDTH = 0.3f;
		private const float PADDING_FROM_SCREEN = 0.0015f;
		private const float OFFSET_FROM_SCREEN_X = 0.1f;
		private const float OFFSET_FROM_SCREEN_Y = 0.1f;

		private const string PATTERN = "%";
	}
}
