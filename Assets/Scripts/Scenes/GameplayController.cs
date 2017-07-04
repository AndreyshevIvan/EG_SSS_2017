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
		public bool isPaused { get; private set; }
		public bool isMapStay { get { return map.isMoveing; } }
		public bool isGameEnd { get { return !ship.isLive || map.isReached; } }
		public bool isWin { get { return isGameEnd && m_world.ship.isLive; } }
		public bool isPlaying { get { return !isPaused && isMapStart && !isGameEnd; } }

		private Ship ship { get; set; }
		private Map map { get; set; }
		private User user { get; set; }

		[SerializeField]
		private GameWorld m_world;
		[SerializeField]
		private GameplayUI m_interface;
		[SerializeField]
		private ScenesController m_scenesController;
		[SerializeField]
		private Factories m_factory;
		private ShipProperties m_shipProperties = new ShipProperties();

		private bool m_isMapStart;
		private bool m_isMapStay;
		private bool m_isPaused;
		private bool m_isGameEnd;
		private bool m_isWin;
		private bool m_isPlaying;

		private const int FRAME_RATE = 40;

		private void Awake()
		{
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = FRAME_RATE;

			isMapStart = false;
			isPaused = false;
		}
		private void Start()
		{
			m_world.gameplay = this;

			InitFactory();
			InitMap();
			InitShip();
			InitWorld();
			InitInterface();

			UpdateChanges();
		}
		private void InitFactory()
		{
			m_factory.Init(m_world.container, m_interface);
			m_world.factory = m_factory;
		}
		private void InitMap()
		{
			map = m_factory.GetMap();
			map.gameplay = this;
			map.factory = m_factory;
		}
		private void InitShip()
		{
			ship = m_factory.GetShip(ShipType.STANDART);
			ship.properties = m_shipProperties;
		}
		private void InitWorld()
		{
			m_world.map = map;
			m_world.ship = ship;
			m_world.playerBar = m_interface;
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
			if (!IsAnyChange())
			{
				return;
			}

			UpdateChanges();

			if (!isGameEnd)
			{
				return;
			}

			if (isWin)
			{
				OnMapReached();
				return;
			}

			OnGameOver();
		}

		private void OnMapStart()
		{
			isMapStart = true;
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
			m_scenesController.SetScene("DemoScene");
		}

		private bool IsAnyChange()
		{
			return (
				m_isMapStart != isMapStart ||
				m_isMapStay != isMapStay ||
				m_isPaused != isPaused ||
				m_isGameEnd != isGameEnd ||
				m_isWin != isWin ||
				m_isPlaying != isPlaying);
		}
		private void UpdateChanges()
		{
			m_isMapStart = isMapStart;
			m_isMapStay = isMapStay;
			m_isPaused = isPaused;
			m_isGameEnd = isGameEnd;
			m_isWin = isWin;
			m_isPlaying = isPlaying;

			m_world.OnGameplayChange();
			m_interface.OnGameplayChange();
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
