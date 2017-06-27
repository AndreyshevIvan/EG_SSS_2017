using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class ShipMind : MonoBehaviour
	{
		public Gun m_firstGun;

		public ShipType type { get; set; }
		public float magnetic { get; set; }
		public float magnetDistance { get; set; }
		public bool isSleep { get; set; }

		public void Init(MapPhysics world)
		{
			magnetic = 1;
			magnetDistance = 5;

			IShipProperties properties = GameData.LoadShip(type);
			m_firstGun.Init(properties.firstGunLevel, world);
		}

		private void Awake()
		{
			isSleep = true;
		}
		private void FixedUpdate()
		{
			m_firstGun.isTimerWork = !isSleep;
		}
	}
}
