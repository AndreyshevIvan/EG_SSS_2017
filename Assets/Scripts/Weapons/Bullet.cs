using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class Bullet : Weapon
	{
		public override void Init(byte level)
		{

		}
		public override void Fire(Transform spawn)
		{
			if (!isReady)
			{
				return;
			}

			Bullet newBullet = Instantiate(this, spawn);
			newBullet.velocity = new Vector3(0, 0, m_speed);
			newBullet.transform.position = spawn.transform.position;
			
		}

		protected Vector3 velocity
		{
			set { m_body.velocity = value; }
		}

		protected override void Update()
		{

		}

		private Rigidbody m_body;
		private float m_speed = 10;

		private void Awake()
		{
			m_body = GetComponent<Rigidbody>();
		}
	}
}
