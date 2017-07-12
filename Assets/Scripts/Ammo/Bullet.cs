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

		public void Shoot(BulletData data, Vector3 spawnPosition)
		{
			this.data = data;
			touchDemage = data.demage;
			spawnPosition.y = GameWorld.FLY_HEIGHT;
			position = spawnPosition;
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
