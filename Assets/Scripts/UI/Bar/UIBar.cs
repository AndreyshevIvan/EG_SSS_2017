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
		}

		protected float minValue { get; set; }
		protected float maxValue { get; set; }
		protected Vector2 offset { get; set; }
		protected RectTransform rect { get; set; }

		protected void Awake()
		{
			rect = GetComponent<RectTransform>();
			minValue = 0;
			maxValue = float.MaxValue;
			OnAwakeEnd();
			InitSizing();
		}
		protected virtual void OnAwakeEnd() { }
		protected virtual void InitSizing() { }
		protected abstract void OnSetNewValue();
		protected virtual void SetPosition(Vector3 worldPosition) { }

		private void Update()
		{
			SetPosition(position);
		}
	}
}
