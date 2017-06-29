using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame.World;

namespace MyGame
{
	public class ShipMind : MonoBehaviour
	{
		public Gun m_firstGun;

		public ShipType type { get; set; }
		public IPlayerBar playerBar { get; set; }
		public float magnetic { get; set; }
		public float magnetDistance { get; set; }
		public bool isSleep { get; set; }

		public void Init(MapPhysics world)
		{
			IShipProperties properties = GameData.LoadShip(type);
			m_firstGun.Init(properties.firstGunLevel, world);

			magnetic = 1;
			magnetDistance = 5;
		}
		public void Modificate()
		{

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
