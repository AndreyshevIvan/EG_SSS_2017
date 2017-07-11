﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MyGame.GameUtils;

namespace MyGame.Enemies
{
	public class AngleTurret : Enemy
	{
		protected override void InitProperties()
		{
			health = maxHealth = 38;
			coldown = 2.25f;
			points = 120;
			healthBar = world.factory.GetBar(BarType.ENEMY_HEALTH);
			isTimerWork = true;

			m_bulletData.demage = 10;
			m_bulletData.speed = 6.5f;
		}
		protected void Start()
		{
			bool isInLeft = position.x < 0;

			Vector3 fromLeft = new Vector3(0.5f, 0, -0.5f);
			Vector3 fromRight = new Vector3(-0.5f, 0, -0.5f);
			m_bulletData.direction = (isInLeft) ? fromLeft : fromRight;
			ExtraReady();
		}

		protected override void Shoot()
		{
			if (position.z > FIRE_POSITION)
			{
				return;
			}

			m_shootsTimer = 0;
			m_shootsCount = 0;
			RemoveTactic(Shooting);
			AddTactic(Shooting);
		}

		[SerializeField]
		private Transform m_gun;
		private BulletData m_bulletData = new BulletData();
		private byte m_shootsCount;
		private float m_shootsTimer;

		private const float SHOOTS_COUNT = 3;
		private const float FIRE_PAUSE = 0.215f;
		private const float FIRE_POSITION = 25;

		private void Shooting()
		{
			if (!Utils.UpdateTimer(ref m_shootsTimer, FIRE_PAUSE))
			{
				return;
			}

			Bullet bullet = factory.GetAmmo<Bullet>(AmmoType.ANGLE_TURRET);
			bullet.Shoot(m_bulletData, position);
			bullet.MoveToGround();
			m_shootsCount++;
			m_shootsTimer = 0;

			if (m_shootsCount == SHOOTS_COUNT) RemoveTactic(Shooting);
		}
	}
}
