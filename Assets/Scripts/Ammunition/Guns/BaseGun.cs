using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public sealed class BaseGun : Gun
	{
		public SimpleBullet m_ammo;

		protected override void DoAfterInit()
		{
			isTimerWork = true;
			coldown = 0.6f;
			bulletsSpeed = 16;
		}
		protected override void Shoot()
		{
			SimpleBullet bullet = Instantiate(m_ammo);
			bullet.position = transform.position;
			bullet.speed = bulletsSpeed;
			mapPhysics.AddPlayerBullet(bullet.gameObject);
			bullet.Start();
		}

		private float bulletsSpeed { get; set; }
	}
}