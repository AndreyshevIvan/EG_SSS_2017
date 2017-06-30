using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyGame.World;
using MyGame.Hero;

namespace MyGame
{
	public sealed class GameplayController : MonoBehaviour, IGameplay
	{
		public MapPhysics m_world;
		public GameplayUI m_interface;

		private Ship ship { get; set; }
		private Map map { get; set; }
		private User user { get; set; }
		private Factories factories { get; set; }

		public bool isMapStart { get; private set; }
		public bool isMapSleep { get; private set; }
		public bool isPaused { get; private set; }
		public bool isGameEnd { get; private set; }
		public bool isWin { get { return isGameEnd && m_world.ship.isLive; } }
		public bool isPlaying { get { return !isPaused && isMapStart && !isGameEnd; } }

		private void Awake()
		{
			isMapStart = false;
			isPaused = false;
			isGameEnd = false;
			isMapSleep = true;

			InitFactories();
			InitUser();
			InitShip();
			InitWorld();
			InitMap();
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
			m_world.gameplay = this;
		}
		private void InitMap()
		{
			map = factories.maps.GetMap();
			map.world = m_world;
			map.gameplay = this;
		}

		private void Start()
		{
			InitInterface();
		}
		private void InitInterface()
		{
			m_interface.gameplay = this;

			m_interface.onPause += map.Pause;

			m_interface.moveShip += ship.MoveTo;

			m_interface.uncontrollEvents += isTrue => m_world.SetSlowMode(isTrue);

			m_interface.firstTouchEvents += () => {
				ship.roadController.Play();
			};
			ship.roadController.OnEndReached.AddListener(T => {
				isMapStart = true;
				OnMapStart();
			});
		}

		private void FixedUpdate()
		{
			isGameEnd = !m_world.isPlayerLive || map.isReached;

			if (isGameEnd)
			{
				if (isWin)
				{
					OnMapReached();
					return;
				}

				OnGameOver();
			}
		}

		private void OnMapStart()
		{
			isMapStart = true;
			isMapSleep = false;
			map.Play();
			m_interface.OnMapStart();
			Destroy(ship.roadController);
		}
		private void OnMapReached()
		{
		}
		private void OnGameOver()
		{
			m_world.KillPlayer();
			m_interface.GameOver();
			isMapSleep = true;
		}
	}

	public interface IGameplay
	{
		bool isMapStart { get; }
		bool isPaused { get; }
		bool isMapSleep { get; }
		bool isGameEnd { get; }
		bool isWin { get; }
		bool isPlaying { get; }
	}
}
