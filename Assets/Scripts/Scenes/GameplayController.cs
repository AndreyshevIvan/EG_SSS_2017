using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public sealed class GameplayController : MonoBehaviour
	{
		public Controller m_controller;
		public MapPhysics m_mapPhysics;

		private Map gameMap { get; set; }
		private User user { get; set; }
		private Factories factories { get; set; }

		private void Awake()
		{
			factories = GetComponent<Factories>();
			user = GameData.LoadUser();
			gameMap = factories.maps.GetMap();
		}
		private void Start()
		{
			gameMap.world = m_mapPhysics;
			gameMap.enemies = factories.enemies;
			gameMap.isPlay = true;
			gameMap.roads = factories.roads;
			factories.ships.Spawn(user.ship, gameMap);
			m_controller.ship = gameMap.shipModel;
		}
	}
}
