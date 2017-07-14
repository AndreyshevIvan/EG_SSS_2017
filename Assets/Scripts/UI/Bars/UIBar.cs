using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using GameUtils;

namespace MyGame
{
	public abstract class UIBar<T> : MonoBehaviour
	{
		public UIContainer controller { get; set; }
		public Vector3 position { get; set; }
		public T value { get; protected set; }
		public bool isActive { get; protected set; }

		public const float HP_BAR_FADE_DUR = 0.4f;

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
		public void SetValue(T newValue)
		{
			if (newValue.Equals(value) && isFirstSetComplete)
			{
				return;
			}

			value = newValue;

			if (!isFirstSetComplete)
			{
				OnFirstSet();
				isFirstSetComplete = true;
			}

			OnSetNewValue();
			lastUpdateTimer = 0;
		}
		public void Fade(float fade, float duration)
		{
			Utils.FadeList(m_fadeElements, fade, duration);
		}
		public void Close()
		{
			controller.Erase(this);
			Destroy(gameObject);
		}

		protected Camera mainCamera { get; private set; }
		protected RectTransform rect { get; set; }
		protected float lastUpdateTimer { get; private set; }
		protected bool isFirstSetComplete { get; private set; }
		protected bool isTimerWork { get; set; }

		protected void Awake()
		{
			ResetFadeElements();
			mainCamera = Camera.main;
			rect = GetComponent<RectTransform>();
			isFirstSetComplete = false;
			lastUpdateTimer = 0;
			isTimerWork = false;
			OnAwakeEnd();
			InitSizing();
		}
		protected virtual void OnAwakeEnd() { }
		protected virtual void InitSizing() { }
		protected virtual void OnActivete() { }
		protected virtual void OnDisactivate() { }
		protected virtual void OnFirstSet() { }

		protected abstract void OnSetNewValue();

		protected void FixedUpdate()
		{
			OnUpdate();
			SetPosition(position);
			if (isTimerWork) lastUpdateTimer += Time.fixedDeltaTime;
		}
		protected virtual void OnUpdate() { }
		protected virtual void SetPosition(Vector3 worldPosition) { }

		protected void ResetFadeElements()
		{
			m_fadeElements = Utils.GetAllComponents<Graphic>(gameObject.transform);
		}

		private List<Graphic> m_fadeElements;
	}

	public abstract class HealthBar : UIBar<int> { }
}
