using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public sealed class SimpleBullet : Ammo
	{
		protected override void OnDemageTaked()
		{
			world.Remove(this, false);
		}
		protected override void PlayingUpdate()
		{
			position += direction * speed * Time.fixedDeltaTime;
		}

		public Vector3 direction { get; private set; }
		public float speed { get; private set; }
	}
}
