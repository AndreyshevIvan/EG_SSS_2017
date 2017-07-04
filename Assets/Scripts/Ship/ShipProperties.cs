using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace MyGame
{
	[System.Serializable]
	public class ShipProperties
	{
		public int health { get { return 100; } }
		public BulletData gunData { get { return new BulletData(); } }
		public float gunColdown { get { return 1; } }
		public int rocketsDemage { get { return 10; } }
		public float magnet { get { return 1; } }

		private byte m_healthLvl;
		private byte m_baseGunLvl;
		private byte m_rocketsLvl;
		private byte m_magnetLvl;
	}
}
