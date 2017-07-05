using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame.Enemies
{
	public class TargetTurret : Enemy
	{
		protected override void InitProperties()
		{
			health = maxHealth = 100;
			starsCount = 2;
			points = 157;
			coldown = 1;
			healthBar = world.factory.GetBar(BarType.ENEMY_HEALTH);
			isTimerWork = true;

			m_bulletData.demage = 15;
			m_bulletData.speed = 15;
		}
		protected override void Shoot()
		{
			Bullet bullet = world.factory.GetAmmo<Bullet>(AmmoType.TARGET_TURRET);
			bullet.data = m_bulletData;
			Vector3 direction = Vector3.Normalize(world.ship.position - position);
			bullet.Shoot(position, direction);
		}

		private BulletData m_bulletData = new BulletData();

		private const float SHOOT_DISTANCE = 20;
	}
}
