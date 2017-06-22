using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public sealed class SimpleBullet : Ammo
	{
		public float speed { get; set; }

		public override void Start()
		{
			m_body.velocity = new Vector3(0, 0, speed);
			demage = 1000;
		}
		public override void OnDemageTaked()
		{
			Destroy(gameObject);
		}

		private void FixedUpdate()
		{
		}
	}
}
