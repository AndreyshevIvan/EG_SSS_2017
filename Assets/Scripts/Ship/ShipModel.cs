using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class ShipModel : MonoBehaviour
	{
		public BoundingBox m_boundary;

		public Vector3 origin
		{
			get { return transform.position; }
		}
		public Vector3 canvasPosition
		{
			get { return Camera.main.WorldToScreenPoint(origin); }
		}
		public GameObject body { private get; set; }
		public ShipMind mind { get { return body.GetComponent<ShipMind>(); } }

		public void MoveTo(Vector3 newPosition)
		{
			Vector3 direction = (newPosition - origin).normalized;
			m_smoothDir = Vector3.MoveTowards(m_smoothDir, direction, SMOOTHING);
			direction = m_smoothDir;
			Vector3 movement = new Vector3(direction.x, 0, direction.z);
			m_rigidBody.velocity = movement * SPEED;
		}

		private Rigidbody m_rigidBody;
		private Vector3 m_smoothDir;

		private const float SPEED = 15;
		private const float SMOOTHING = 15;
		private const float TILT = 2;
		private const float HEIGHT = 1;

		private void Awake()
		{
			m_rigidBody = GetComponent<Rigidbody>();
		}
		private void FixedUpdate()
		{
			UpdatePositionOnField();
			UpdateRotation();
		}
		private void UpdatePositionOnField()
		{
			transform.position = new Vector3(
				Mathf.Clamp(origin.x, m_boundary.xMin, m_boundary.xMax),
				HEIGHT,
				Mathf.Clamp(origin.z, m_boundary.zMin, m_boundary.zMax)
			);
		} 
		private void UpdateRotation()
		{
			float eulerZ = m_rigidBody.velocity.x * -TILT;
			m_rigidBody.rotation = Quaternion.Euler(0, 0, eulerZ);
			m_rigidBody.velocity = Vector3.zero;
		}
	}
}
