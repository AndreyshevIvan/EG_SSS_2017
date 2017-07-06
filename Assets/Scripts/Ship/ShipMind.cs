using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

		public float magnetFactor { get { return m_properties.magnetFactor; } }
		public float magnetDistance { get { return m_properties.magnetDistance; } }

		public byte ModificateByOne()
		{
			if (modification >= GameWorld.MODIFICATION_COUNT)
			{
				return modification;
			}

			modification++;
			m_properties.gunColdown -= GUN_COLDOWN_STEP;

			return modification;
		}

		protected override void OnInitEnd()
		{
			modification = 0;
		}
		protected override void PlayingUpdate()
		{
			ShootByBaseGun();
		}

		private ShipProperties m_properties;
		private float m_gunTimer = 0;

		private void ShootByBaseGun()
		{
			if (!Utils.UpdateTimer(ref m_gunTimer, m_properties.gunColdown))
			{
				return;
			}

			Bullet bullet = world.factory.GetAmmo<Bullet>(AmmoType.PLAYER_BULLET);
			bullet.data = m_properties.gunData;
			bullet.Shoot(position, Vector3.forward);
			m_gunTimer = 0;
		}
		private void UpdateProperties()
		{
			m_properties.gunColdown = 0.7f;

			m_properties.gunData = new BulletData();
			m_properties.gunData.speed = 20;
			m_properties.gunData.demage = 10;
		}

		protected override void OnExitFromWorld()
		{
		}

		private byte modification { get; set; }

		private const float GUN_COLDOWN_STEP = 0.05f;
	}
}
