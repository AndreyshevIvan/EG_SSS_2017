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
		public bool isMapStart { get; private set; }
		public bool isMapStay { get; private set; }
		public bool isPaused { get; private set; }
		public bool isGameEnd { get; private set; }
		public bool isWin { get { return isGameEnd && m_world.ship.isLive; } }
		public bool isPlaying { get { return !isPaused && isMapStart && !isGameEnd; } }

		[SerializeField]
		private GameWorld m_world;
		[SerializeField]
		private GameplayUI m_interface;
		[SerializeField]
		private ScenesController m_scenesController;
		[SerializeField]
		private Factories m_factory;

		private Ship ship { get; set; }
		private Map map { get; set; }
		private User user { get; set; }

		private const int FRAME_RATE = 40;

		private void Awake()
		{
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = FRAME_RATE;

			isMapStart = false;
			isPaused = false;
			isGameEnd = false;
			isMapStay = true;
		}
		private void Start()
		{
			InitUser();
			InitFactory();
			InitShip();
			InitMap();
			InitWorld();
			InitInterface();
		}
		private void InitUser()
		{
			//user = GameData.LoadUser();
		}
		private void InitFactory()
		{
			m_factory.Init(m_world.container, m_interface);
			m_world.factory = m_factory;
		}
		private void InitShip()
		{
			ship = m_factory.GetShip(ShipType.VOYAGER);
			ship.InitWorld(m_world);
		}
		private void InitMap()
		{
			map = m_factory.GetMap();
			map.InitGameplay(this);
		}
		private void InitWorld()
		{
			m_world.ship = ship;
			m_world.playerBar = m_interface;
			m_world.map = map;
			m_world.InitGameplay(this);
		}
		private void InitInterface()
		{
			m_interface.InitGameplay(this);

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

			Debug.Log(ship.isLive + " " + map.isReached + " at " + Time.realtimeSinceStartup);


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
