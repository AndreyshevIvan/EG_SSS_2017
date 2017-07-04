using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public sealed class Bullet : Body
	{
		public BulletData data { set { m_data = value; } }
		public Vector3 direction { get; set; }
		public Bullet copy
		{
			get
			{
				Bullet myCopy = Instantiate(this);
				myCopy.m_data = m_data;
				myCopy.direction = direction;
				return myCopy;
			}
		}

		public void Shoot(Vector3 shootPosition, Vector3 shootDirection)
		{
			position = shootPosition;
			direction = shootDirection;
		}

		protected override void OnInitEnd()
		{
			touchDemage = 10;//m_data.demage;
			MoveToSky();
		}
		protected override void OnDemageTaked()
		{
			world.Remove(this, false);
		}
		protected override void PlayingUpdate()
		{
			position += direction * Time.fixedDeltaTime * 10;// * m_data.speed;
		}

		private BulletData m_data;
	}

	public struct BulletData
	{
		public float speed;
		public int demage;
	}
}
