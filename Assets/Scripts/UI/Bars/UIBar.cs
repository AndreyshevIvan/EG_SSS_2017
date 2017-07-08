using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
	public abstract class UIBar : MonoBehaviour
	{
		public int value { get; protected set; }
		public Vector3 position { get; set; }
		public bool isFadable { get; set; }
		public UIContainer controller { get; set; }

		public void SetValue(int newValue)
		{
			if (newValue == value)
			{
				return;
			}

			value = newValue;
			OnSetNewValue();
			lastUpdateTimer = 0;
			if (isFadable && isFirstSetComplete) SetFade(1, 0);
			if (!isFirstSetComplete) isFirstSetComplete = true;
		}
		public void Close()
		{
			controller.Erase(this);
			Destroy(gameObject);
		}

		protected float lastUpdateTimer { get; private set; }
		protected Vector2 offset { get; set; }
		protected RectTransform rect { get; set; }
		protected bool isFirstSetComplete { get; private set; }
		protected bool isTimerWork { get; set; }

		protected void Awake()
		{
			m_fadeElements = Utils.GetAllComponents<Graphic>(gameObject.transform);
			rect = GetComponent<RectTransform>();
			isFirstSetComplete = false;
			lastUpdateTimer = 0;
			isTimerWork = true;
			OnAwakeEnd();
			InitSizing();
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

		private const float CLOSE_FADE_TIME = 0.15f;
		private const float VISIBLE_TIME = 1;
		private const float FADE_TIME = 0.3f;

		private void FixedUpdate()
		{
			OnUpdate();
			SetPosition(position);
			UpdateFading();
			if (isTimerWork) lastUpdateTimer += Time.fixedDeltaTime;
		}
		private void UpdateFading()
		{
			if (!isFadable || lastUpdateTimer < VISIBLE_TIME)
			{
				return;
			}

			SetFade(0, FADE_TIME);
		}
	}

	public delegate void CloseEvent<IEnumerator>();
}
