using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyGame.World;
using MyGame.Hero;

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

			m_interface.onBeginControllPlayer += isTrue => {
				m_world.SetSlowMode(isTrue, false);
			};

			m_interface.onFirstTouch += ship.splineController.Play;
			ship.splineController.OnEndReached.AddListener(T => OnMapStart());

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

		private void OnMapStart()
		{
			gameMap.Play();
			m_interface.OnPrepareEnd();
			Destroy(ship.splineController);
		}
		private void OnGameOver()
		{
			m_interface.GameOver();
			gameMap.Stop();
		}
	}
}
