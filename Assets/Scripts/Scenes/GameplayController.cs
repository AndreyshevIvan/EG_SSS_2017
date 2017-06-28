using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyGame
{
	public sealed class GameplayController : MonoBehaviour
	{
		public MapPhysics m_world;
		public GameplayUI m_interface;

		private Ship ship { get; set; }
		private Map gameMap { get; set; }
		private User user { get; set; }
		private Factories factories { get; set; }

		private void Awake()
		{
			InitFactories();
			InitUser();
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
			m_interface.onBeginControllPlayer += m_world.SetSlowMode;
			m_interface.onFirstTouch += gameMap.Play;
			m_world.onPlayerDeath += OnGameOver;
		}
		private void InitFactories()
		{
			factories = GetComponent<Factories>();
			factories.bars = m_interface;
		}
		private void InitUser()
		{
			user = GameData.LoadUser();
		}
		private void InitShip()
		{
			ship = factories.ships.Get(user.ship);
			ship.Init(m_world);
		}
		private void InitWorld()
		{
			m_world.factories = factories;
			m_world.ship = ship;
			m_world.playerBar = m_interface;
		}
		private void InitMap()
		{
			gameMap = factories.maps.GetMap();
			gameMap.world = m_world;
		}
		private void OnGameOver()
		{
			m_interface.GameOver();
			gameMap.isSleep = true;
		}
	}
}
