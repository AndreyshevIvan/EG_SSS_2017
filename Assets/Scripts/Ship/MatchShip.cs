using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class MatchShip : MonoBehaviour, IHealthBody, IDemageBody
	{
		public float demage { get { return m_touchDemage; } }
		public float addDemage { set { m_health -= value; } }
		public bool isLive { get { return m_health > 0; } }
		public int health { get { return (int)m_health; } }
		public float healthPart { get { return m_health / m_maxhealth; } }
		public float touchDemage { get { return m_touchDemage; } }

		public Gun m_simpleGun;

		public Transform m_leftGun;
		public Transform m_rightGun;

		public void Init(IGunProperties properties)
		{
			m_simpleGun.SetLevel(properties.gunLevel);
		}

		private float m_health;
		private float m_maxhealth;
		private float m_touchDemage;
	}
}
