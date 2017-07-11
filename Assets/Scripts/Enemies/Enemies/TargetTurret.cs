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
			health = maxHealth = 46 + (int)(world.time / 10);
			coldown = 2.4f;
			points = 120;
			healthBar = world.factory.GetBar(BarType.ENEMY_HEALTH);
			isTimerWork = true;

			m_bulletData.demage = 10;
			m_bulletData.speed = 5;

			AddTactic(RotateGun);
		}
		protected override void Shoot()
		{
			if (!inGameBox)
			{
				return;
			}

			Bullet bullet = factory.GetAmmo<Bullet>(AmmoType.TARGET_TURRET);
			Vector3 direction = Vector3.Normalize(world.shipPosition - position);
			m_bulletData.direction = direction;
			bullet.Shoot(m_bulletData, position);
		}

		[SerializeField]
		private Transform m_gun;
		private BulletData m_bulletData = new BulletData();

		private void RotateGun()
		{
			if (!inGameBox)
			{
				return;
			}

			Vector3 direction = world.shipPosition - position;
			Quaternion rotation = Quaternion.LookRotation(direction);
			Quaternion newRotation = Quaternion.Slerp(m_gun.rotation, rotation, 1);
			m_gun.transform.rotation = newRotation;
		}
	}
}
