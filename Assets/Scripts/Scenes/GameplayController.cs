using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyGame
{
	public sealed class GameplayController : MonoBehaviour
	{
		public MapPhysics world;
		public GameplayUI m_interface;

		private Ship ship { get; set; }
		private Map gameMap { get; set; }
		private User user { get; set; }
		private Factories factories { get; set; }

		private void Awake()
		{
			factories = GetComponent<Factories>();
			user = GameData.LoadUser();
			gameMap = factories.maps.GetMap();

			InitShip();
			InitWorld();
			InitMap();
		}
		private void Start()
		{
			InitUIEvents();
		}
		private void InitUIEvents()
		{
			m_interface.onPause += gameMap.Pause;
			m_interface.onControllPlayer += ship.MoveTo;
			m_interface.onBeginControllPlayer += world.SetSlowMode;
			m_interface.onFirstTouch += gameMap.Play;
			world.onPlayerDeath += OnGameOver;
		}
		private void InitShip()
		{
			ship = factories.ships.Get(user.ship);
			ship.world = world;
			ship.mind.Init(world);
		}
		private void InitWorld()
		{
			world.factories = factories;
			world.ship = ship;
		}
		private void InitMap()
		{
			gameMap.world = world;
		}
		private void OnGameOver()
		{
			m_interface.GameOver();
			gameMap.isSleep = true;
		}
	}
}
