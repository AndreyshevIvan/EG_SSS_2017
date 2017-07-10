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

			PlayerBomb bomb = Instantiate(m_bombPrefab);
			world.Add(bomb);
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

			m_laserTimer = 0;
			return true;
		}

		protected override void OnInitEnd()
		{
		}
		protected override void PlayingUpdate()
		{
			ShootByBaseGun();
			UpdateTimers();
		}
		private void UpdateTimers()
		{
			isBombReady = Utils.UpdateTimer(ref m_bombTimer, m_properties.bombColdown);
			isLaserReady = Utils.UpdateTimer(ref m_laserTimer, m_properties.laserColdown);
		}
		protected override void OnExitFromWorld()
		{
		}

		[SerializeField]
		private Transform m_bulletSpawn;
		[SerializeField]
		private PlayerBomb m_bombPrefab;
		private ShipProperties m_properties;
		private float m_gunScatter = 0;
		private float m_gunTimer = 0;
		private float m_bombTimer = 0;
		private float m_laserTimer = 0;

		private bool isBombReady { get; set; }
		private bool isLaserReady { get; set; }

		private const float SCATTER_STEP = 0.01f;
		private const float GUN_COLDOWN_STEP = 0.07f;
		private const float BOMB_CAST_TIME = 1.4f;

		private void ShootByBaseGun()
		{
			if (!Utils.UpdateTimer(ref m_gunTimer, m_properties.gunColdown))
			{
				return;
			}

			Bullet bullet = world.factory.GetAmmo<Bullet>(AmmoType.PLAYER_BULLET);
			bullet.data = m_properties.gunData;
			float randomScatter = UnityEngine.Random.Range(-m_gunScatter, m_gunScatter);
			Vector3 direction = Vector3.forward + new Vector3(randomScatter, 0, 0);
			bullet.Shoot(m_bulletSpawn.position, direction);
			m_gunTimer = 0;
		}
		private void SetNewProperties()
		{
			m_properties.gunColdown = 0.7f;
			m_properties.bombColdown = 3;
			m_properties.laserColdown = 5;

			m_properties.gunData = new BulletData();
			m_properties.gunData.speed = 20;
			m_properties.gunData.demage = 10;
		}
	}
}
