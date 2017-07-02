using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
	public abstract class UIBar : MonoBehaviour
	{
		public float value { get; protected set; }
		public Vector3 position { get; set; }
		public bool isFadable { get; set; }

		public void SetValue(float newValue)
		{
			if (newValue == value)
			{
				return;
			}

			if (newValue >= minValue && newValue <= maxValue)
			{
				value = newValue;
			}
			else
			{
				value = (newValue < minValue) ? minValue : maxValue;
			}


			OnSetNewValue();
			lastUpdateTimer = 0;
			if (isFadable && isFirstSetComplete) SetFade(1, 0);
			if (!isFirstSetComplete) isFirstSetComplete = true;
		}

		protected float lastUpdateTimer { get; private set; }
		protected float minValue { get; set; }
		protected float maxValue { get; set; }
		protected Vector2 offset { get; set; }
		protected RectTransform rect { get; set; }
		protected bool isFirstSetComplete { get; private set; }

		protected void Awake()
		{
			m_fadeElements = Utils.GetAllComponents<Graphic>(gameObject.transform);
			rect = GetComponent<RectTransform>();
			isFirstSetComplete = false;
			lastUpdateTimer = 0;
			minValue = 0;
			maxValue = float.MaxValue;
			OnAwakeEnd();
			InitSizing();
			SetFade(0, 0);
		}
		protected virtual void OnAwakeEnd() { }
		protected virtual void InitSizing() { }
		protected abstract void OnSetNewValue();
		protected virtual void SetPosition(Vector3 worldPosition) { }
		protected virtual void OnUpdate() { }
		protected void SetFade(float fade, float duration)
		{
			m_fadeElements.ForEach(element =>
			{
				element.CrossFadeAlpha(fade, duration, true);
			});
		}

		private List<Graphic> m_fadeElements;

		private const float VISIBLE_TIME = 1;
		private const float FADE_TIME = 0.3f;

		private void FixedUpdate()
		{
			OnUpdate();
			SetPosition(position);
			UpdateFade();
			lastUpdateTimer += Time.fixedDeltaTime;
		}
		private void UpdateFade()
		{
			if (!isFadable || !Utils.IsTimerReady(lastUpdateTimer, VISIBLE_TIME))
			{
				return;
			}

			SetFade(0, FADE_TIME);
		}
	}
}
