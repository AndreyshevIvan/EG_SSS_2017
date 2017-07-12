using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public class Rocket : Body
	{
		public void SetTarget(RocketData newData, Vector3 position)
		{
			m_data = newData;
			position.y = GameWorld.FLY_HEIGHT;
			this.position = position;
			transform.rotation = Quaternion.LookRotation(direction);
			touchDemage = m_data.demage;
		}

		protected override void OnDemageTaked()
		{
			Exit();
		}
		protected override void SmartPlayingUpdate()
		{
			if (!target || m_timer > m_data.diactivateTime)
			{
				Exit();
			}

			Quaternion lookRotation = Quaternion.LookRotation(direction);
			rotation = Quaternion.Slerp(rotation, lookRotation, rotationSpeed);
			position += transform.forward * m_data.speed * Time.fixedDeltaTime;
			m_timer += Time.fixedDeltaTime;
		}

		private RocketData m_data;
		private float m_timer = 0;

		private Transform target { get { return m_data.target; } }
		private Vector3 direction { get { return target.position - position; } }
		private Quaternion rotation
		{
			get { return transform.rotation; }
			set { transform.rotation = value; }
		}
		private float rotationSpeed
		{
			get { return m_data.rotationSpeed * Time.fixedDeltaTime; }
		}
	}

	public struct RocketData
	{
		public Transform target;
		public float diactivateTime;
		public float speed;
		public int demage;
		public float rotationSpeed;
		public float criticalAngle;
	}
}
