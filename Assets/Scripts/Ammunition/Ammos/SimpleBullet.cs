using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame.World;

namespace MyGame
{
	public sealed class SimpleBullet : Ammo
	{
		public override void OnDemageTaked()
		{
			world.EraseAmmo(this);
		}

		protected override void PlayingUpdate()
		{
			position += direction * speed * Time.fixedDeltaTime;
		}

		public Vector3 direction { get; private set; }
		public float speed { get; private set; }
	}
}
