using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyGame.Hero;
using MyGame.Factory;

namespace MyGame
{
	public sealed class GameplayController : MonoBehaviour, IGameplay
	{
		[SerializeField]
		private GameWorld m_world;
		[SerializeField]
		private GameplayUI m_interface;
		[SerializeField]
		private ScenesController m_scenesController;

		private Ship ship { get; set; }
		private Map map { get; set; }
		private User user { get; set; }
		private Factories factory { get; set; }

		public bool isMapStart { get; private set; }
		public bool isMapStay { get; private set; }
		public bool isPaused { get; private set; }
		public bool isGameEnd { get; private set; }
		public bool isWin { get { return isGameEnd && m_world.ship.isLive; } }
		public bool isPlaying { get { return !isPaused && isMapStart && !isGameEnd; } }

		private const int FRAME_RATE = 40;

		private void Awake()
		{
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = FRAME_RATE;

			isMapStart = false;
			isPaused = false;
			isGameEnd = false;
			isMapStay = true;

			InitFactories();
			InitUser();
			InitShip();
			InitMap();
			InitWorld();
		}
		private void InitFactories()
		{
			factory = GetComponent<Factories>();
			factory.Init(m_world, m_interface);
		}
		private void InitUser()
		{
			//user = GameData.LoadUser();
		}
		private void InitShip()
		{
			ship = factory.GetShip(ShipType.VOYAGER);
		}
		private void InitMap()
		{
			//map = factories.maps.GetMap();
		}
		private void InitWorld()
		{
			m_world.factory = factory;
			m_world.ship = ship;
			//m_world.playerBar = m_interface;
			m_world.gameplay = this;
			m_world.map = map;
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
			isGameEnd = !ship.isLive || map.isReached;

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
			isMapStay = false;
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
			isMapStay = true;
			m_scenesController.SetScene("demoscene");
		}
	}

	public interface IGameplay
	{
		bool isMapStart { get; }
		bool isPaused { get; }
		bool isMapStay { get; }
		bool isGameEnd { get; }
		bool isWin { get; }
		bool isPlaying { get; }
	}
}
