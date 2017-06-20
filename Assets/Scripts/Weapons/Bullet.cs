using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class Bullet : Ammo
	{
		public override void Init(byte level)
		{

		}

		protected Vector3 velocity
		{
			set { m_body.velocity = value; }
		}
		protected override void Update()
		{

		}
		protected override bool IsWeaponReady()
		{
			return true;
		}
		protected override void Fire(Transform spawn)
		{
			Bullet newBullet = Instantiate(this, spawn);
			newBullet.velocity = new Vector3(0, 0, m_speed);
			newBullet.transform.position = spawn.transform.position;

		}

		private Rigidbody m_body;
		private float m_speed = 10;

		private void Awake()
		{
			m_body = GetComponent<Rigidbody>();
		}
	}
}
