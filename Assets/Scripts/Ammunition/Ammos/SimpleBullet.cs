using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public sealed class SimpleBullet : Ammo
	{
		public float speed { get; private set; }
		public Vector3 direction { get; private set; }

		public void Init(Vector3 target, float speed, float demage)
		{
			direction = Vector3.Normalize(target - position);
			this.speed = speed;
			this.demage = demage;
		}
		public override void Start()
		{
			physicsBody.velocity = direction * speed;
		}
		public override void OnDemageTaked()
		{
			Destroy(gameObject);
		}
	}
}
