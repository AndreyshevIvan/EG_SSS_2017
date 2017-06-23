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
			DestroyMe();
		}

		private void FixedUpdate()
		{
			CheckValidArea();
		}
		private void CheckValidArea()
		{
			Vector3 position = transform.position;

			if (!Utils.IsContain(position.x, m_mapBox.xMin, m_mapBox.xMax))
			{
				DestroyMe();
			}
			if (!Utils.IsContain(position.z, m_mapBox.zMin, m_mapBox.zMax))
			{
				DestroyMe();
			}
		}
	}
}
