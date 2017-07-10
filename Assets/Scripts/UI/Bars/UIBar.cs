using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using MyGame.GameUtils;
using UnityEngine.EventSystems;

namespace MyGame
{
	public abstract class UIBar : MonoBehaviour
	{
		public UIContainer controller { get; set; }
		public Vector3 position { get; set; }
		public int value { get; protected set; }
		public bool isActive { get; protected set; }
		public bool isFadable { get; set; }

		public void SetActive(bool isActive)
		{
			this.isActive = isActive;

			if (isActive)
			{
				OnActivete();
				return;
			}

			OnDisactivate();
		}
		public void SetValue(int newValue)
		{
			if (newValue == value)
			{
				return;
			}

			value = newValue;
			OnSetNewValue();
			lastUpdateTimer = 0;
			if (isFadable && isFirstSetComplete) Fade(1, 0);
			if (!isFirstSetComplete) isFirstSetComplete = true;
		}
		public void Fade(float fade, float duration)
		{
			if (m_fadeElements == null)
			{
				return;
			}

			m_fadeElements.ForEach(element => element.CrossFadeAlpha(fade, duration, true));
		}
		public void Close()
		{
			controller.Erase(this);
			Destroy(gameObject);
		}

		protected Camera mainCamera { get; private set; }
		protected EventTrigger trigger { get; private set; }
		protected RectTransform rect { get; set; }
		protected float lastUpdateTimer { get; private set; }
		protected bool isFirstSetComplete { get; private set; }
		protected bool isTimerWork { get; set; }

		protected void Awake()
		{
			ResetFadeElements();
			mainCamera = Camera.main;
			rect = GetComponent<RectTransform>();
			trigger = GetComponent<EventTrigger>();
			isFirstSetComplete = false;
			lastUpdateTimer = 0;
			isTimerWork = true;
			OnAwakeEnd();
			InitSizing();
		}
		protected virtual void OnAwakeEnd() { }
		protected virtual void InitSizing() { }
		protected virtual void OnActivete() { }
		protected virtual void OnDisactivate() { }

		protected abstract void OnSetNewValue();

		protected void FixedUpdate()
		{
			OnUpdate();
			SetPosition(position);
			UpdateFading();
			if (isTimerWork) lastUpdateTimer += Time.fixedDeltaTime;
		}
		protected virtual void OnUpdate() { }
		protected virtual void SetPosition(Vector3 worldPosition) { }

		protected void ResetFadeElements()
		{
			m_fadeElements = Utils.GetAllComponents<Graphic>(gameObject.transform);
		}

		private List<Graphic> m_fadeElements;

		private const float CLOSE_FADE_TIME = 0.15f;
		private const float VISIBLE_TIME = 1;
		private const float FADE_TIME = 0.3f;

		private void UpdateFading()
		{
			if (!isFadable || lastUpdateTimer < VISIBLE_TIME)
			{
				return;
			}

			Fade(0, FADE_TIME);
		}
	}

	public delegate void CloseEvent<IEnumerator>();
}
