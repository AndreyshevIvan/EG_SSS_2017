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

		private User m_user;
		private ShipsFactory m_shipsFactory;

		private void Awake()
		{
			m_shipsFactory = GetComponent<ShipsFactory>();
			m_user = GameData.LoadUser();

			GameObject ship = m_shipsFactory.Spawn(m_user.ship);
			shipMind = ship.GetComponentInChildren<ShipMind>();
			shipBody = ship.GetComponent<ShipModel>();
			shipMind.Init(GameData.LoadShip(m_user.ship), this);
		}
		new private void FixedUpdate()
		{
			base.FixedUpdate();


		}
	}
}
