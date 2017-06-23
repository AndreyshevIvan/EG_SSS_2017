using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class ShipModel : Body
	{
		public Vector3 origin { get { return transform.position; } }
		public GameObject body { private get; set; }
		public ShipMind mind { get { return body.GetComponent<ShipMind>(); } }

		public void MoveTo(Vector3 newPosition)
		{
			Vector3 direction = (newPosition - origin).normalized;
			m_smoothDir = Vector3.MoveTowards(m_smoothDir, direction, SMOOTHING);
			direction = m_smoothDir;
			Vector3 movement = new Vector3(direction.x, 0, direction.z);
			m_physicsBody.velocity = movement * SPEED;
			m_isMoved = true;
		}

		private Vector3 m_smoothDir;
		private bool m_isMoved = false;

		private const float SPEED = 15;
		private const float SMOOTHING = 15;
		private const float TILT = 2;
		private const float HEIGHT = 1;

		private void FixedUpdate()
		{
			UpdatePositionOnField();
			UpdateRotation();
			UpdateMoveingSpeed();
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
			float zEuler = m_physicsBody.velocity.x * -TILT;
			m_physicsBody.rotation = Quaternion.Euler(0, 0, zEuler);
		}
		private void UpdateMoveingSpeed()
		{
			Vector3 velocity = m_physicsBody.velocity;
			m_physicsBody.velocity = (m_isMoved) ? velocity : Vector3.zero;
			m_isMoved = false;
		}
	}
}
