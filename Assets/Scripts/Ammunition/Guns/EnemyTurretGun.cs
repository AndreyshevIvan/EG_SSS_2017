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

		protected override void DoAfterInit()
		{
			coldown = 1;
		}
		protected override void Shoot()
		{
			SimpleBullet bullet = Instantiate(m_bullet);
			bullet.position = m_bulletsSpawn.position;
			bullet.Init(gameMap.shipPosition, 5, 10);
			bullet.Start();
			gameMap.AddEnemyBullet(bullet.gameObject);
		}
	}
}
