using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public sealed class GameplayController : MonoBehaviour
	{
		public MapPhysics world;
		public GameplayUI m_interface;

		private Map gameMap { get; set; }
		private User user { get; set; }
		private Factories factories { get; set; }

		private void Awake()
		{
			factories = GetComponent<Factories>();
			user = GameData.LoadUser();
			gameMap = factories.maps.GetMap();
			world.factories = factories;
		}
		private void Start()
		{
			gameMap.world = world;
			gameMap.Init(factories.ships.Get(user.ship));
			InitUIEvents();
		}
		private void InitUIEvents()
		{
			m_interface.onPause += gameMap.Pause;
			m_interface.onRestart += gameMap.Restart;
			m_interface.onControllPlayer += gameMap.ship.MoveTo;
			m_interface.onBeginControllPlayer += world.SetSlowMode;
			m_interface.onFirstTouch += gameMap.Play;
			world.onPlayerDeath += OnGameOver;
		}
		private void OnGameOver()
		{
			Debug.Log("Player dead");
		}
	}
}
