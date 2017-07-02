using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public sealed class SimpleBullet : Body
	{
		public Vector3 direction { get; set; }
		public float speed { get; set; }

		protected override void OnDemageTaked()
		{
			world.Remove(this, false);
		}
		protected override void PlayingUpdate()
		{
			position += direction * speed * Time.fixedDeltaTime;
		}
	}
}
