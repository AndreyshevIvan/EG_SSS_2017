using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class GameplayController : MonoBehaviour, IMapPhysics
	{
		public ShipModel m_ship;
		public ShipsManager m_shipsManager;
		public ShipController m_controller;

		public Transform m_playerBullets;

		public ShipMind ship { get { return m_shipMind; } }

		public void AddPlayerBullet(Ammo bullet)
		{
			bullet.transform.SetParent(m_playerBullets);
		}
		public void AddEnemyBullet(Ammo bullet)
		{
		}

		public void Pause(bool isPause)
		{
		}

		private User m_user;
		private ShipMind m_shipMind;

		private void Start()
		{
			m_user = GameData.LoadUser();
			m_ship.body = m_shipsManager.Get(m_user.ship, m_ship.transform);
			m_shipMind = m_ship.mind;
			m_shipMind.Init(GameData.LoadShip(m_user.ship), this);
		}
		private void FixedUpdate()
		{
		}
	}
}
