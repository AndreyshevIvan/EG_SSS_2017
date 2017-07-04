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
				m_bullet.data = value.gunData;
				gunColdown = value.gunColdown;

				magnetic = value.magnet;
			}
		}

		public float magnetic { get; protected set; }
		public float magnetDistance { get; protected set; }

		public void Modificate(byte modNumber)
		{
		}

		protected float gunColdown { get; set; }

		protected override void OnInitEnd()
		{
			magnetDistance = 5;
		}
		protected override void PlayingUpdate()
		{
			ShootByBaseGun();
		}

		[SerializeField]
		private Bullet m_bullet;
		private float m_gunTimer = 0;

		private void ShootByBaseGun()
		{
			if (!Utils.UpdateTimer(ref m_gunTimer, gunColdown))
			{
				return;
			}

			Bullet bullet = m_bullet.copy;
			world.Add(bullet);
			bullet.Shoot(position, Vector3.forward);
		}
		private void UpdateColdowns()
		{
		}
	}
}
