using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public sealed class GameplayController : MonoBehaviour
	{
		public MapPhysics m_mapPhysics;
		public GameplayUI m_interface;

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
			ShipModel model = factories.ships.Spawn(user.ship);
			model.mind.Init(gameMap.world);
			gameMap.Init(model);
			InitUIEvents();
		}
		private void InitUIEvents()
		{
			m_interface.onPause += gameMap.Pause;
			m_interface.onRestart += gameMap.Restart;
			m_interface.onControllPlayer += gameMap.shipModel.MoveTo;
			m_interface.onBeginControllPlayer += m_mapPhysics.SetSlowMode;
		}
	}
}
