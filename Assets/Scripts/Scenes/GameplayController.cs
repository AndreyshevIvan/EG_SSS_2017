using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public sealed class GameplayController : MapPhysics
	{
		public override void AddPoints(ushort pointsCount)
		{
		}
		public void Pause(bool isPause)
		{
		}

		protected override ShipMind shipMind { get; set; }
		protected override Body shipBody { get; set; }

		private User m_user;
		private ShipsFactory m_shipsFactory;
		private ShipController m_controller;

		private void Start()
		{
			m_shipsFactory = GetComponent<ShipsFactory>();
			m_controller = GetComponent<ShipController>();
			m_user = GameData.LoadUser();

			GameObject ship = m_shipsFactory.Spawn(m_user.ship);
			shipMind = ship.GetComponent<ShipMind>();
			shipBody = ship.GetComponent<Body>();
			shipMind.Init(GameData.LoadShip(m_user.ship), this);
		}
		new private void FixedUpdate()
		{
			base.FixedUpdate();


		}
	}
}
