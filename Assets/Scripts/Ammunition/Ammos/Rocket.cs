using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public class Rocket : Ammo
	{
		public void Init(Body target, float speed, float demage, float factor)
		{
			this.speed = speed;
			this.demage = demage;
			this.target = target;
			this.factor = factor;
		}
		public override void Start()
		{
		}
		public override void OnDemageTaked()
		{
			Destroy(gameObject);
		}

		protected override void OnUpdate()
		{
			Vector3 targetPosition = target.position;
			Vector3 direction = Vector3.Normalize(targetPosition - position);
			physicsBody.velocity = direction * speed * factor;
		}

		private float factor { get; set; }
		private float speed { get; set; }
		private Body target { get; set; }
	}
}
