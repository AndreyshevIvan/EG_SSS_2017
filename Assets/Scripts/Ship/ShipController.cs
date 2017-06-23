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
			//HandleTouch();
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
			screenPosition.z = Camera.main.transform.position.y;
			screenPosition = Camera.main.ScreenToWorldPoint(screenPosition);
			m_ship.MoveTo(screenPosition);
		}
		private void InitSize()
		{
		}
		private bool IsShipTouch()
		{
			return true;
		}
	}
}
