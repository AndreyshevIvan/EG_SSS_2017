using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame.World;

namespace MyGame.Hero
{
	public class ShipMind : MonoBehaviour
	{
		public Gun m_firstGun;

		public ShipType type { get; internal set; }
		public float magnetic { get; protected set; }
		public float magnetDistance { get; protected set; }

		public void Modificate(byte modNumber)
		{
		}

		private IGameWorld world { get; set; }

		private void Awake()
		{
		}
		private void FixedUpdate()
		{
			//m_firstGun.isTimerWork = !world.gameplay.isMapStay;
		}

		internal void Init(IGameWorld newWorld)
		{
			world = newWorld;
			//IShipProperties properties = GameData.LoadShip(type);
			m_firstGun.Init(world);//properties.firstGunLevel, world);

			magnetic = 1;
			magnetDistance = 5;
		}
	}
}
