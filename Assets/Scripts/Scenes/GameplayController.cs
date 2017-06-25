using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public sealed class GameplayController : MonoBehaviour
	{
		public Controller m_controller;

		private Map m_gameMap;
		private User m_user;
		private ShipsFactory m_shipsFactory;
		private MapsFactory m_mapsFactory;
		private EnemiesFactory m_enemiesFactory;

		private void Awake()
		{
			m_shipsFactory = GetComponent<ShipsFactory>();
			m_mapsFactory = GetComponent<MapsFactory>();
			m_enemiesFactory = GetComponent<EnemiesFactory>();
			m_user = GameData.LoadUser();
			m_gameMap = m_mapsFactory.GetMap();
		}
		private void Start()
		{
			m_gameMap.enemies = m_enemiesFactory;
			m_shipsFactory.Spawn(m_user.ship, m_gameMap);
			m_controller.ship = m_gameMap.shipModel;
		}
	}
}
