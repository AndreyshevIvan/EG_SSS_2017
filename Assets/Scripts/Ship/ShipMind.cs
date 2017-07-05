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

		public void Modificate(byte modNumber)
		{
		}

		protected float gunColdown { get; set; }

		protected override void OnInitEnd()
		{
		}
		protected override void PlayingUpdate()
		{
			ShootByBaseGun();
		}

		private ShipProperties m_properties;
		private float m_gunTimer = 0;

		private void ShootByBaseGun()
		{
			if (!Utils.UpdateTimer(ref m_gunTimer, gunColdown))
			{
				return;
			}

			Bullet bullet = world.factory.GetAmmo<Bullet>(AmmoType.PLAYER_BULLET);
			bullet.data = m_properties.gunData;
			bullet.Shoot(position, Vector3.forward);
		}
		private void UpdateProperties()
		{
			gunColdown = 0.6f;//m_properties.gunColdown;

			m_properties.gunData = new BulletData();
			m_properties.gunData.speed = 30;
			m_properties.gunData.demage = 10;
		}
	}
}
