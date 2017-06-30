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

		protected override void OnAwakeEnd()
		{
			layout = GetComponent<HorizontalLayoutGroup>();
			m_fadeElements = Utils.GetAllComponents<Graphic>(gameObject.transform);
			maxValue = 1;
			SetFade(0, 0);
		}
		protected override void InitSizing()
		{
			float barWidth = (m_isShip) ? PLAYER_WIDTH : ENEMY_WIDTH;
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
		protected override void SetPosition(Vector3 worldPosition)
		{
			Vector3 screenPosition = Utils.WorldToCanvas(worldPosition);
			transform.position = screenPosition;
		}
		protected override void OnSetNewValue()
		{
			m_textField.text = value.ToString(PATTERN);
			if (m_healthLine != null) m_healthLine.fillAmount = value;
			if (isFirstSetComplete)
			{
				SetFade(1, 0);
			}
		}
		protected override void OnUpdate()
		{
			if (Utils.IsTimerReady(lastUpdateTimer, TIME_TO_BE_UNVISIBLE))
			{
				SetFade(0, FADE_TIME);
			}
		}

		private List<Graphic> m_fadeElements;

		private HorizontalLayoutGroup layout { get; set; }

		private const float TIME_TO_BE_UNVISIBLE = 1;
		private const float FADE_TIME = 0.3f;

		private const float FONT_FACTOR = 0.03f;
		private const float PLAYER_WIDTH = 0.12f;
		private const float ENEMY_WIDTH = 0.06f;
		private const float HEIGHT_FACTOR = 3.2f;
		private const float PADDING_FACTOR = 0.0015f;
		private const string PATTERN = "0%";

		private void SetFade(float fade, float duration)
		{
			m_fadeElements.ForEach(element => {
				element.CrossFadeAlpha(fade, duration, true);
			});
		}
	}
}
