using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public class Rocket : Body
	{
		protected override void OnDemageTaked()
		{
			Exit();
		}
		protected override void SmartPlayingUpdate()
		{
			if (!target)
			{
				Exit();
			}

			Vector3 direction = Vector3.Normalize(target.position - position);
			position += direction * speed * factor;
		}

		private float factor { get; set; }
		private float speed { get; set; }
		private WorldObject target { get; set; }
	}
}
