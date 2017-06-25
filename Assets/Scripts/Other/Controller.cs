using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyGame
{
	public class Controller : MonoBehaviour
	{
		public ShipModel ship { get; set; }

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
			ship.MoveTo(screenPosition);
		}
	}
}
