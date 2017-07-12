using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public sealed class Bullet : Body
	{
		public Vector3 direction { get; set; }
		public TrailRenderer trailRenderer { get; private set; }

		public void Shoot(BulletData data, Vector3 position)
		{
			this.data = data;
			touchDemage = data.demage;
			position.y = GameWorld.FLY_HEIGHT;
			this.position = position;
			data.direction.y = 0;
			direction = data.direction;
		}

		protected override void OnInitEnd()
		{
			trailRenderer = GetComponent<TrailRenderer>();
			MoveToSky();
		}
		protected override void OnDemageTaked()
		{
		}
		protected override void SmartPlayingUpdate()
		{
			position += direction * Time.fixedDeltaTime * data.speed;
		}

		private BulletData data { get; set; }
	}

	public struct BulletData
	{
		public Vector3 direction;
		public float speed;
		public int demage;
	}
}
