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
				UpdateProperties();
			}
		}

		public float magnetFactor { get { return 1; } }
		public float magnetDistance { get { return 5; } }

		public void ModificateByOne()
		{
			m_properties.gunColdown -= GUN_COLDOWN_STEP;
			m_scatter += SCATTER_STEP;
		}

		protected override void OnInitEnd()
		{
		}
		protected override void PlayingUpdate()
		{
			ShootByBaseGun();
		}
		protected override void OnExitFromWorld()
		{
		}

		[SerializeField]
		private Transform m_bulletSpawn;
		private ShipProperties m_properties;
		private float m_scatter = 0;
		private float m_gunTimer = 0;

		private const float SCATTER_STEP = 0.01f;
		private const float GUN_COLDOWN_STEP = 0.07f;

		private void ShootByBaseGun()
		{
			if (!Utils.UpdateTimer(ref m_gunTimer, m_properties.gunColdown))
			{
				return;
			}

			Bullet bullet = world.factory.GetAmmo<Bullet>(AmmoType.PLAYER_BULLET);
			bullet.data = m_properties.gunData;
			float randomScatter = UnityEngine.Random.Range(-m_scatter, m_scatter);
			Vector3 direction = Vector3.forward + new Vector3(randomScatter, 0, 0);
			bullet.Shoot(m_bulletSpawn.position, direction);
			m_gunTimer = 0;
		}
		private void UpdateProperties()
		{
			m_properties.gunColdown = 0.7f;

			m_properties.gunData = new BulletData();
			m_properties.gunData.speed = 20;
			m_properties.gunData.demage = 10;
		}
	}
}
