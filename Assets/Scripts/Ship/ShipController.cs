using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyGame
{
	public class ShipController : MonoBehaviour
	{
		public ShipModel m_ship;
		public GameplayUI m_gameplayUI;

		private bool m_isTouchInit = false;
		private bool m_isGameStart = false;
		private float m_areaRadius;

		private void Start()
		{
			InitSize();
		}
		private void FixedUpdate()
		{
			HandleMouse();
			HandleTouch();
		}
		private void HandleMouse()
		{
			if (!Input.GetMouseButton(0))
			{
				return;
			}

			SetPosition(Input.mousePosition);
		}
		private void HandleTouch()
		{
			if (Input.touchCount == 0)
			{
				return;
			}

			SetPosition(Input.GetTouch(0).position);
		}
		private void SetPosition(Vector3 screenPosition)
		{
			screenPosition.z = Camera.main.transform.position.y + 1;
			screenPosition = Camera.main.ScreenToWorldPoint(screenPosition);
			m_ship.SetPosition(screenPosition);
		}
		private void InitSize()
		{
			float width = m_ship.transform.localScale.x;
			Vector3 shipLeftPos = m_ship.worldPosition + new Vector3(width, 0, 0);
			Vector3 leftPoint = Camera.main.WorldToScreenPoint(shipLeftPos);
			m_areaRadius = Mathf.Abs(leftPoint.x - m_ship.canvasPosition.x);
			m_gameplayUI.areaSize = m_areaRadius;
		}
		private bool IsShipTouch()
		{
			return true;
		}
	}
}
