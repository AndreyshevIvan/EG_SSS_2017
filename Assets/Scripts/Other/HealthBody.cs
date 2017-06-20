using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class HealthBody
	{
		public float demage
		{
			set { m_health -= value; }
		}
		public bool isLive
		{
			get { return m_health > 0; }
		}
		public int health
		{
			get { return (int)m_health; }
		}
		public float healthPart
		{
			get { return health / m_maxHealth; }
		}

		protected float m_health;
		protected float m_maxHealth = 100;
	}
}