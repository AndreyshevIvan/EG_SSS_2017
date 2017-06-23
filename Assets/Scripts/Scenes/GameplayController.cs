using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public sealed class GameplayController : MapPhysics
	{
		public ShipModel m_ship;
		public ShipController m_controller;

		public Transform m_playerBullets;

		public ShipMind ship { get { return m_shipMind; } }

		public override void AddPoints(ushort pointsCount)
		{
		}
		public void Pause(bool isPause)
		{
		}

		private User m_user;
		private ShipMind m_shipMind;
		private ShipsFactory m_shipsFactory;

		private void Start()
		{
			m_shipsFactory = GetComponent<ShipsFactory>();
			m_user = GameData.LoadUser();
			m_ship.body = m_shipsFactory.Get(m_user.ship, m_ship.transform);
			m_shipMind = m_ship.mind;
			m_shipMind.Init(GameData.LoadShip(m_user.ship), this);
		}
		new private void FixedUpdate()
		{
			base.FixedUpdate();


		}
	}
}
