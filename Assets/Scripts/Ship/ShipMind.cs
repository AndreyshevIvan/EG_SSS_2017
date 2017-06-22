using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class ShipMind : MonoBehaviour, ILivingBody, IDemageBody
	{
		public Gun m_baseGun;
		//public Gun m_specificGun;
		//public ActiveSpell m_activeSpell;
		//public PassiveSpell m_passiveSpell;

		public float demage { get { return m_touchDemage; } }
		public float addDemage { set { m_health -= value; } }
		public bool isLive { get { return m_health > 0; } }
		public int health { get { return (int)m_health; } }
		public float healthPart { get { return m_health / m_maxhealth; } }

		public void Init(IShipProperties properties, IMapPhysics mapPhysics)
		{
			m_baseGun.Init(properties.baseGunLevel, mapPhysics);
			//m_specificGun.Init(properties.specificGunLevel);
			//m_activeSpell.Init(properties.activeSpellLevel);
			//m_passiveSpell.Init(properties.passiveSpellLevel);
		}

		private float m_health;
		private float m_maxhealth;
		private float m_touchDemage;

		private void FixedUpdate()
		{
			m_baseGun.Modify();
			m_baseGun.Shoot();
		}
	}
}
