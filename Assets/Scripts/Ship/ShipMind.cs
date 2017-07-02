using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Hero
{
	public class ShipMind : WorldObject
	{
		public ShipType type { get; internal set; }
		public float magnetic { get; protected set; }
		public float magnetDistance { get; protected set; }

		public void Modificate(byte modNumber)
		{
		}

		protected override void OnInitEnd()
		{
			magnetic = 1;
			magnetDistance = 5;
		}
		protected override void PlayingUpdate()
		{
			UpdateColdowns();
			ShootByBaseGun();
		}

		[SerializeField]
		private SimpleBullet m_bullet;
		private float m_baseTimer = 0;
		private float m_baseColdown = 0.5f;

		private bool isBaseReady { get; set; }

		private void ShootByBaseGun()
		{
			Debug.Log("try shoot");
			if (!isBaseReady)
			{
				Debug.Log("Not now");
				return;
			}

			SimpleBullet bullet = Instantiate(m_bullet);
			bullet.direction = Vector3.forward;
			bullet.speed = 10;
			world.Add(bullet);
		}
		private void UpdateColdowns()
		{
			isBaseReady = Utils.UpdateTimer(ref m_baseTimer, m_baseColdown, Time.fixedDeltaTime);
		} 
	}
}
