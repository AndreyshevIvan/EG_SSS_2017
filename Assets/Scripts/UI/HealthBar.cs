using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
	public class HealthBar : MonoBehaviour
	{
		public Text m_textField;
		public Image m_healthLine;
		public bool m_isShip;

		public void SetPosition(Vector3 worldPosition)
		{
			Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
			transform.position = screenPosition;
		}
		public void SetValue(float healthPart)
		{
			m_textField.text = healthPart.ToString("0%");
			if (m_healthLine != null) m_healthLine.fillAmount = healthPart;
		}

		private HorizontalLayoutGroup layout { get; set; }
		private RectTransform rect { get; set; }

		private const float PLAYER_WIDTH = 0.12f;
		private const float ENEMY_WIDTH = 0.05f;
		private const float HEIGHT_FACTOR = 3.2f;
		private const float PADDING_FACTOR = 0.0015f;

		private void Awake()
		{
			rect = GetComponent<RectTransform>();
			layout = GetComponent<HorizontalLayoutGroup>();
			InitSize();
		}
		private void InitSize()
		{
			float barWidth = ((m_isShip) ? PLAYER_WIDTH : ENEMY_WIDTH) * Screen.width;
			Utils.SetWidth(rect, barWidth);
			Utils.SetHeight(rect, barWidth / HEIGHT_FACTOR);

			if (!m_isShip)
			{
				return;
			}

			int paddingByFactor = (int)(Screen.width * PADDING_FACTOR);
			int padding = (paddingByFactor >= 1) ? paddingByFactor : 1;
			layout.padding = new RectOffset(padding, padding, padding, padding);
		}
	}
}
