using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using GameUtils;

namespace MyGame
{
	public sealed class PlayerHealthBar : HealthBar
	{
		protected override void OnAwakeEnd()
		{
			Fade(0, 0);
			m_offset.x = Utils.GetFromSreen(OFFSET_FROM_SCREEN_X);
			m_offset.y = Utils.GetFromSreen(OFFSET_FROM_SCREEN_Y);
		}
		protected override void InitSizing()
		{
			float barWidth = Utils.GetFromSreen(WIDTH_FACTOR);
			Utils.SetWidth(rect, barWidth);
			Utils.SetHeight(rect, barWidth * HEIGHT_FROM_WIDTH);
			m_textField.fontSize = Utils.GetFromSreen(FONT_FACTOR);
		}
		protected override void SetPosition(Vector3 worldPosition)
		{
			Vector2 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);
			transform.position = screenPosition + m_offset;
		}
		protected override void OnSetNewValue()
		{
			m_textField.text = value.ToString() + PATTERN;
		}

		[SerializeField]
		private Text m_textField;
		private Vector2 m_offset = new Vector2();

		private const float FONT_FACTOR = 0.03f;

		private const float WIDTH_FACTOR = 0.12f;
		private const float HEIGHT_FROM_WIDTH = 0.3f;
		private const float PADDING_FROM_SCREEN = 0.0015f;
		private const float OFFSET_FROM_SCREEN_X = 0.1f;
		private const float OFFSET_FROM_SCREEN_Y = 0.1f;

		private const string PATTERN = "%";
	}
}
