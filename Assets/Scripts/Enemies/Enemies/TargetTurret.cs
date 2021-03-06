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
			health = maxHealth = 5;
			coldown = 2.4f;
			points = 120;
			healthBar = world.factory.GetEnemyHealthBar();
			bonuses.Add(Pair<BonusType, int>.Create(BonusType.STAR, 3));
			isTimerWork = true;

			m_bulletData.demage = 15;
			m_bulletData.speed = 6;

			playingUpdate += RotateGun;
		}
		protected override void Shoot()
		{
			if (!inGameBox)
			{
				return;
			}

			Bullet bullet = factory.GetAmmo(AmmoType.TARGET_TURRET) as Bullet;
			Vector3 direction = Vector3.Normalize(world.shipPosition - spawnPos);
			m_bulletData.direction = direction;
			bullet.Shoot(m_bulletData, spawnPos);
		}

		[SerializeField]
		private Transform m_gun;
		[SerializeField]
		private Transform m_bulletSpawn;
		private BulletData m_bulletData = new BulletData();

		private Vector3 spawnPos { get { return m_bulletSpawn.position; } }

		private void RotateGun()
		{
			if (!inGameBox)
			{
				return;
			}

			Vector3 direction = world.shipPosition - position;
			Quaternion rotation = Quaternion.LookRotation(direction);
			m_gun.rotation = Quaternion.Lerp(m_gun.rotation, rotation, 1);
		}
	}
}
