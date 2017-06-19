using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class ShipModel : MonoBehaviour
	{
		public Transform m_leftGun;
		public Transform m_rightGun;
		public Boundary m_boundary;

		public Vector3 worldPosition
		{
			get { return transform.position; }
		}
		public Vector3 canvasPosition
		{
			get { return Camera.main.WorldToScreenPoint(worldPosition); }
		}

		public void SetBody(GameObject newBody)
		{
			ClearBody();
			m_body = Instantiate(newBody, transform);
		}
		public void SetPosition(Vector3 newPosition)
		{
			Vector3 origin = worldPosition;
			Vector2 currentPosition = new Vector3(newPosition.x, newPosition.z);
			Vector3 direction = (newPosition - origin).normalized;
			m_smoothDirection = Vector3.MoveTowards(m_smoothDirection, direction, SMOOTHING);
			direction = m_smoothDirection;
			Vector3 movement = new Vector3(direction.x, 0, direction.z);
			m_rigidBody.velocity = movement * SPEED;
		}

		private GameObject m_body;
		private Rigidbody m_rigidBody;
		private Vector3 m_smoothDirection;

		private const float SPEED = 15;
		private const float SMOOTHING = 5;
		private const float TILT = 2;
		private const float HEIGHT = 1;

		private void Awake()
		{
			ClearBody();
			m_rigidBody = GetComponent<Rigidbody>();
		}
		private void FixedUpdate()
		{
			transform.position = new Vector3 (
				Mathf.Clamp(worldPosition.x, m_boundary.xMin, m_boundary.xMax),
				HEIGHT,
				Mathf.Clamp(worldPosition.z, m_boundary.yMin, m_boundary.yMax)
			);

			float eulerZ = m_rigidBody.velocity.x * -HEIGHT;
			m_rigidBody.rotation = Quaternion.Euler(0, 0, eulerZ);
			m_rigidBody.velocity = Vector3.zero;
		}
		private void ClearBody()
		{
			if (m_body == null)
			{
				return;
			}

			Destroy(m_body);
		}
	}
}
