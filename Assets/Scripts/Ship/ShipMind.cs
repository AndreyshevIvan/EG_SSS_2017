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
		public int mods { get; protected set; }

		public const int MODIFICATION_COUNT = 12;

		public void ModificateByOne()
		{
			if (mods >= MODIFICATION_COUNT)
			{
				return;
			}

			mods++;
			m_properties.gunColdown -= GUN_COLDOWN_STEP;
			m_gunScatter += SCATTER_STEP;
		}
		public bool Bomb()
		{
			if (!isBombReady)
			{
				return false;
			}

			PlayerBomb bomb = world.factory.GetAmmo(AmmoType.PLAYER_BOMB) as PlayerBomb;
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

			if (isGunReady)
			{
				ShootByBaseGun();
				m_gunTimer = 0;
			}

			UpdateTimers();
		}
		protected override void OnExitFromWorld()
		{
		}

		[SerializeField]
		private Transform m_bulletSpawn;
		private ShipProperties m_properties;
		private EventDelegate m_update;
		private EventDelegate m_shooting;
		private float m_gunScatter = 0;
		private float m_gunTimer = 0;

		private float m_bombTimer = 0;

		private float m_laserTimer = 0;
		private float m_laserShootTimer = 0;
		private float m_laserDuration = 0;

		private Vector3 gunDirection
		{
			get { return Vector3.forward + Utils.RndDirBetween(90 - m_gunScatter, 90 + m_gunScatter); }
		}
		private bool isGunReady
		{
			get { return Utils.UpdateTimer(ref m_gunTimer, m_properties.gunColdown); }
		}
		private bool isBombReady { get; set; }
		private bool isLaserReady { get; set; }

		private const float SCATTER_STEP = 0.34f;
		private const float GUN_COLDOWN_STEP = 0.035f;

		private void UpdateTimers()
		{
			isBombReady = Utils.UpdateTimer(ref m_bombTimer, m_properties.bombColdown);
			isLaserReady = Utils.UpdateTimer(ref m_laserTimer, m_properties.laserColdown);
		}

		private Bullet CreateBullet(AmmoType type, BulletData data, Vector3 direction)
		{
			Bullet bullet = factory.GetAmmo(type) as Bullet;
			data.direction = direction;
			bullet.Shoot(data, m_bulletSpawn.position);
			return bullet;
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

			AmmoType type = AmmoType.PLAYER_LASER;
			BulletData data = m_properties.laserData;

			CreateBullet(type, data, Utils.RndDirBetween(30, 40));
			CreateBullet(type, data, Utils.RndDirBetween(84, 96));
			CreateBullet(type, data, Utils.RndDirBetween(140, 150));

			m_laserShootTimer = 0;
		}
		private void SetNewProperties()
		{
			m_properties.gunColdown = 0.48f;

			m_properties.bombColdown = 12;

			m_properties.laserColdown = 20;
			m_properties.laserData = new BulletData();
			m_properties.laserData.speed = 28;
			m_properties.laserData.demage = 1;
			m_properties.laserDuration = 2;
			m_properties.laserShootColdown = 0.03f;

			m_properties.gunData = new BulletData();
			m_properties.gunData.speed = 24;
			m_properties.gunData.demage = 1;
		}
		private void ShootByBaseGun()
		{
			Bullet bullet = CreateBullet(AmmoType.PLAYER_BULLET, m_properties.gunData, gunDirection);
			float modsPart = (float)mods / MODIFICATION_COUNT;
			GameplayUI.SetShipBulletColor(modsPart, bullet.trailRenderer);
		}

		private enum Mods
		{
			BASIC = 1,
			DOUBLE,
			TRIPLE,
			QUADRO,
		}
	}
}
