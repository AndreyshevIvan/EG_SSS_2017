﻿using System;
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
			health = maxHealth = 46;
			coldown = 2.4f;
			points = 120;
			healthBar = world.factory.GetBar(BarType.ENEMY_HEALTH);
			isTimerWork = true;

			m_bulletData.demage = 10;
			m_bulletData.speed = 5;
		}
		protected override void UpdateTaktic()
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
		protected override void Shoot()
		{
			if (!inGameBox)
			{
				return;
			}

			Bullet bullet = world.factory.GetAmmo<Bullet>(AmmoType.TARGET_TURRET);
			Vector3 direction = Vector3.Normalize(world.shipPosition - position);
			m_bulletData.direction = direction;
			bullet.Shoot(m_bulletData, position);
		}

		private BulletData m_bulletData = new BulletData();
		[SerializeField]
		private Transform m_gun;

		private const float SHOOT_DISTANCE = 20;
	}
}
