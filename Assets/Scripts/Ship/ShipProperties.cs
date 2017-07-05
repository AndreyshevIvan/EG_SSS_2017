using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace MyGame
{
	[System.Serializable]
	public struct ShipProperties
	{
		public BulletData gunData;
		public float gunColdown;
		public int health;
		public int rocketsDemage;
		public float magnetDistance;
		public float magnetFactor;

		private byte m_healthLvl;
		private byte m_baseGunLvl;
		private byte m_rocketsLvl;
		private byte m_magnetLvl;
	}
}
