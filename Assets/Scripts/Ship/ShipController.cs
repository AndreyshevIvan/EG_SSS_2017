using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyGame
{
	public class ShipController : MonoBehaviour
	{
		private ShipModel m_ship;

		private void Start()
		{
			m_ship = GetComponentInChildren<ShipModel>();
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
			screenPosition.z = Camera.main.transform.position.y;
			screenPosition = Camera.main.ScreenToWorldPoint(screenPosition);
			m_ship.MoveTo(screenPosition);
		}
	}
}
