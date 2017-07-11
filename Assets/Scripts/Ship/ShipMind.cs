using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame.GameUtils;

namespace MyGame.Hero
{
	public class ShipMind : WorldObject
	{
		public ShipType type { get; set; }
		public ShipProperties properties
		{
			set
			{
				m_properties = value;
				SetNewProperties();
			}
		}

		public float magnetFactor { get { return 1; } }
		public float magnetDistance { get { return 5; } }
		public float bombProcess { get { return m_bombTimer / m_properties.bombColdown; } }
		public float laserProcess { get { return m_laserTimer / m_properties.laserColdown; } }

		public void ModificateByOne()
		{
			m_properties.gunColdown -= GUN_COLDOWN_STEP;
			m_gunScatter += SCATTER_STEP;
		}
		public bool Bomb()
		{
			if (!isBombReady)
			{
				return false;
			}

			PlayerBomb bomb = world.factory.GetAmmo<PlayerBomb>(AmmoType.PLAYER_BOMB);
			bomb.parent = transform;
			m_bombTimer = 0;
			return true;
		}
		public bool Laser()
		{
			if (!isLaserReady)
			{
				return false;
			}

			m_laserDuration = 0;
			m_laserTimer = 0;
			if (m_update != null) m_update -= LaserShoot;
			m_update += LaserShoot;
			return true;
		}

		protected override void OnInitEnd()
		{
		}
		protected override void PlayingUpdate()
		{
			if (m_update != null) m_update();
			ShootByBaseGun();
			UpdateTimers();
		}
		protected override void OnExitFromWorld()
		{
		}

		[SerializeField]
		private Transform m_bulletSpawn;
		private ShipProperties m_properties;
		EventDelegate m_update;
		private float m_gunScatter = 0;
		private float m_gunTimer = 0;

		private float m_bombTimer = 0;

		private float m_laserTimer = 0;
		private float m_laserShootTimer = 0;
		private float m_laserDuration = 0;

		private bool isBaseGunReady { get; set; }
		private bool isBombReady { get; set; }
		private bool isLaserReady { get; set; }

		private const float SCATTER_STEP = 0.335f;
		private const float GUN_COLDOWN_STEP = 0.055f;

		private void UpdateTimers()
		{
			isBaseGunReady = Utils.UpdateTimer(ref m_gunTimer, m_properties.gunColdown);
			isBombReady = Utils.UpdateTimer(ref m_bombTimer, m_properties.bombColdown);
			isLaserReady = Utils.UpdateTimer(ref m_laserTimer, m_properties.laserColdown);
		}
		private void ShootByBaseGun()
		{
			if (!isBaseGunReady)
			{
				return;
			}

			Bullet bullet = factory.GetAmmo<Bullet>(AmmoType.PLAYER_BULLET);
			m_properties.gunData.direction = Utils.RndDirBetween(90 - m_gunScatter, 90 + m_gunScatter);
			bullet.Shoot(m_properties.gunData, m_bulletSpawn.position);
			m_gunTimer = 0;
		}
		private void LaserShoot()
		{
			if (m_laserDuration >= m_properties.laserDuration)
			{
				if (m_update != null) m_update -= LaserShoot;
				return;
			}

			float shootColdown = m_properties.laserShootColdown;
			m_laserDuration += Time.fixedDeltaTime;

			if (!Utils.UpdateTimer(ref m_laserShootTimer, shootColdown))
			{
				return;
			}

			m_properties.laserData.direction = Utils.RndDirBetween(30, 40);
			Bullet left = factory.GetAmmo<Bullet>(AmmoType.PLAYER_LASER);
			left.Shoot(m_properties.laserData, m_bulletSpawn.position);

			m_properties.laserData.direction = Utils.RndDirBetween(84, 96);
			Bullet middle = factory.GetAmmo<Bullet>(AmmoType.PLAYER_LASER);
			middle.Shoot(m_properties.laserData, m_bulletSpawn.position);

			m_properties.laserData.direction = Utils.RndDirBetween(140, 150);
			Bullet right = factory.GetAmmo<Bullet>(AmmoType.PLAYER_LASER);
			right.Shoot(m_properties.laserData, m_bulletSpawn.position);

			m_laserShootTimer = 0;
		}
		private void SetNewProperties()
		{
			m_properties.gunColdown = 0.67f;

			m_properties.bombColdown = 10;

			m_properties.laserColdown = 16;
			m_properties.laserData = new BulletData();
			m_properties.laserData.speed = 28;
			m_properties.laserData.demage = 1;
			m_properties.laserDuration = 3;
			m_properties.laserShootColdown = 0.03f;

			m_properties.gunData = new BulletData();
			m_properties.gunData.speed = 20;
			m_properties.gunData.demage = 6;
		}
	}
}
