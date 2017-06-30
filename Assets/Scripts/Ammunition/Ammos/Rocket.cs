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
		public void Init(Body target, float speed, int demage, float factor)
		{
			this.speed = speed;
			this.demage = demage;
			this.target = target;
			this.factor = factor;
		}
		public override void StartAmmo()
		{
		}
		public override void OnDemageTaked()
		{
			ExitFromWorld();
		}

		protected override void NotSleepUpdate()
		{
			if (!target)
			{
				ExitFromWorld();
			}

			Vector3 targetPosition = target.transform.position;
			Vector3 direction = Vector3.Normalize(targetPosition - position);
			physicsBody.velocity = direction * speed * factor;
		}

		private float factor { get; set; }
		private float speed { get; set; }
		private Body target { get; set; }
	}
}
