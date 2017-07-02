using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MyGame.World;

namespace MyGame
{
	public class Rocket : Ammo
	{
		public override void OnDemageTaked()
		{
			world.Remove(this, false);
		}

		protected override void PlayingUpdate()
		{
			if (!target)
			{
				world.Remove(this, false);
			}

			Vector3 direction = Vector3.Normalize(target.position - position);
			position += direction * speed * factor;
		}

		private float factor { get; set; }
		private float speed { get; set; }
		private WorldObject target { get; set; }
	}
}
