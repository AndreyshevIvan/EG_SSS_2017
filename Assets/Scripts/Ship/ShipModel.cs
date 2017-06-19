using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class ShipModel : MonoBehaviour
	{
		public Transform m_leftGun;
		public Transform m_rightGun;

		public float m_speed;
		public float m_tilt;
		public float m_smoothing;

		public Vector3 worldPosition
		{
			get { return transform.position; }
		}
		public Vector3 canvasPosition
		{
			get
			{
				return Camera.main.WorldToScreenPoint(worldPosition);
			}
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
			Vector3 directionRaw = newPosition - origin;
			Vector3 direction = directionRaw.normalized;
			m_smoothDirection = Vector3.MoveTowards(m_smoothDirection, direction, m_smoothing);
			direction = m_smoothDirection;
			Vector3 movement = new Vector3(direction.x, 0, direction.z);
			m_rigidBody.velocity = movement * m_speed;
		}

		private GameObject m_body;
		private Rigidbody m_rigidBody;
		private Vector3 m_smoothDirection;

		private void Awake()
		{
			ClearBody();
			m_rigidBody = GetComponent<Rigidbody>();
		}
		private void FixedUpdate()
		{
			/*
			transform.position = new Vector3
				(
					Mathf.Clamp(transform.position.x, m_boundary.xMin, m_boundary.xMax),
					0.0f,
					Mathf.Clamp(transform.position.z, m_boundary.zMin, m_boundary.zMax)
				);
				
			*/
			float eulerZ = m_rigidBody.velocity.x * -m_tilt;
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
