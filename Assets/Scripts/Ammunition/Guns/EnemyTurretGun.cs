using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public class EnemyTurretGun : Gun
	{
		public Transform m_bulletsSpawn;
		public SimpleBullet m_bullet;

		public float speed { get; set; }
		public float demage { get; set; }

		protected override void DoAfterInit()
		{
			coldown = 1.3f;
		}
		protected override void Shoot()
		{
			SimpleBullet bullet = Instantiate(m_bullet);
			bullet.position = m_bulletsSpawn.position;
			bullet.Init(gameMap.shipPosition, speed, demage);
			bullet.Start();
			gameMap.AddAmmo(bullet);
		}
	}
}
