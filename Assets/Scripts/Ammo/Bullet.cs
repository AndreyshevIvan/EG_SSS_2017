using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public sealed class Bullet : Body
	{
		public BulletData data { get; set; }
		public Vector3 direction { get; set; }

		public void Shoot(Vector3 shootPosition, Vector3 shootDirection)
		{
			touchDemage = data.demage;

			shootPosition.y = GameWorld.FLY_HEIGHT;
			position = shootPosition;
			shootDirection.y = 0;
			direction = shootDirection;
		}

		protected override void OnInitEnd()
		{
			MoveToSky();
		}
		protected override void OnDemageTaked()
		{
		}
		protected override void PlayingUpdate()
		{
			position += direction * Time.fixedDeltaTime * data.speed;
		}
	}

	public struct BulletData
	{
		public float speed;
		public int demage;
	}
}
