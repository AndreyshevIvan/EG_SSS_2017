using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class ShipMind : MonoBehaviour
	{
		public Gun m_firstGun;
		public Gun m_secondGun;
		public Spell m_firstSpell;
		public Spell m_secondSpell;

		public float magnetic { get; set; }
		public float magneticDistance { get; set; }

		public void Init(IShipProperties properties, MapPhysics mapPhysics)
		{
			magnetic = 1;
			magneticDistance = 5;

			m_firstGun.Init(properties.firstGunLevel, mapPhysics);
			//m_secondGun.Init(properties.secondGunLevel, mapPhysics);
			//m_firstSpell.Init(properties.firstSpellLevel, mapPhysics);
			//m_secondSpell.Init(properties.secondSpellLevel, mapPhysics);
		}

		private void FixedUpdate()
		{
		}
	}
}
