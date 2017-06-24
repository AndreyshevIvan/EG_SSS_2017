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
			body.velocity = new Vector3(0, 0, speed);
			touchDemage = 20;
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

			if (!Utils.IsContain(position.x, mapBox.xMin, mapBox.xMax))
			{
				DestroyMe();
			}
			if (!Utils.IsContain(position.z, mapBox.zMin, mapBox.zMax))
			{
				DestroyMe();
			}
		}
	}
}
