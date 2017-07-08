using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyGame.Hero;
using MyGame.Factory;
using FluffyUnderware.Curvy;

namespace MyGame
{
	public sealed class GameplayController : MonoBehaviour, IGameplay
	{
		public bool isMapStart { get; private set; }
		public bool isPaused { get; private set; }
		public bool isStop { get; private set; }
		public bool isGameEnd { get { return !ship.isLive || (map.isReached && m_world.isAllEnemiesKilled); } }
		public bool isWin { get { return isGameEnd && m_world.ship.isLive; } }
		public bool isPlaying { get { return !isPaused && isMapStart && !isGameEnd; } }

		public void Continue()
		{
			SceneManager.LoadScene("Main");
		}

		public const float ENDING_WAITING_TIME = 3;

		private Ship ship { get; set; }
		private Map map { get; set; }

		[SerializeField]
		private GameWorld m_world;
		[SerializeField]
		private GameplayUI m_interface;
		[SerializeField]
		private ScenesController m_scenesController;
		[SerializeField]
		private Factories m_factory;
		[SerializeField]
		private Results m_resultsUI;
		private ShipProperties m_shipProperties = new ShipProperties();

		private EventDelegate m_update;

		private bool m_isMapStart;
		private bool m_isPaused;
		private bool m_isGameEnd;
		private bool m_isWin;
		private bool m_isPlaying;
		private bool m_isStop;

		private float m_prePauseTimeScale;
		private bool m_moveingComplete = false;

		private const int FRAME_RATE = 60;
		private const float SHIP_PRE_START_SPEED = 4;
		private const float SHIP_START_SPEED = 6;

		private void Awake()
		{
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = FRAME_RATE;
			m_resultsUI.gameObject.SetActive(false);

			isMapStart = false;
			isPaused = false;
			isStop = false;
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
			map = m_factory.GetMap(MapType.FIRST);
			map.gameplay = this;
			map.factory = m_factory;
		}
		private void InitShip()
		{
			ship = m_factory.GetShip(ShipType.STANDART);
			ship.properties = m_shipProperties;

			ship.roadController.Spline = m_factory.GetRoad(RoadType.PRE_START);
			ship.roadController.Clamping = CurvyClamping.Loop;
			ship.roadController.PlayAutomatically = true;
			ship.roadController.Speed = SHIP_PRE_START_SPEED;
		}
		private void InitWorld()
		{
			m_world.map = map;
			m_world.ship = ship;
			m_world.playerInterface = m_interface;
		}
		private void InitInterface()
		{
			m_interface.Init(m_world);

			m_interface.onPause += Pause;

			m_interface.moveShip += ship.MoveTo;

			m_interface.uncontrollEvents += isTrue => m_world.SetSlowMode(isTrue);

			m_interface.firstTouchEvents += () =>
			{
				ship.roadController.Spline = null;
				m_update += MoveShipToStartRoad;
			};
		}

		private void FixedUpdate()
		{
			if (m_update != null) m_update();

			if (!CheckUpdateChanges() || !isGameEnd)
			{
				return;
			}

			OnMapReached();
		}

		private void SetStartRoad()
		{
			ship.roadController.OnEndReached.AddListener(T =>
			{
				map.Play();
				isMapStart = true;
				Destroy(ship.roadController);
			});

			ship.roadController.Spline = m_factory.GetRoad(RoadType.PLAYER);
			ship.roadController.Clamping = CurvyClamping.Clamp;
			ship.roadController.Speed = SHIP_START_SPEED;
			ship.roadController.Position = 0;

		}
		private void MoveShipToStartRoad()
		{
			if (m_moveingComplete)
			{
				m_update -= MoveShipToStartRoad;
				SetStartRoad();
				return;
			}

			CurvySpline spline = m_factory.GetRoad(RoadType.PLAYER);
			Vector3 target = spline.Segments[0].transform.position;
			float movement = SHIP_START_SPEED * Time.fixedDeltaTime;
			ship.position = Vector3.MoveTowards(ship.position, target, movement);
			m_moveingComplete = ship.position == target;
		}
		private void OnMapReached()
		{
			if (!isWin)
			{
				m_world.KillPlayer();
			}

			SaveUserData();

			Utils.DoAfterTime(this, ENDING_WAITING_TIME, () =>
			{
				isStop = true;

				Utils.DoAfterTime(this, GameplayUI.ENDING_FADE_TIME, () =>
				{
					m_resultsUI.gameObject.SetActive(true);
					m_resultsUI.Open(null, null, m_world.player, isWin);
				});
			});
		}
		private void SaveUserData()
		{
			//User user = GameData.LoadUser();
			//User oldUser = (User)user.Clone();
			//user.AddNew(m_world.player);
			//User newUser = (User)user.Clone();
			//GameData.SaveUser(user);
		}

		private void Pause(bool isPause)
		{
			isPaused = isPause;

			if (isPause)
			{
				m_prePauseTimeScale = Time.timeScale;
			}

			Time.timeScale = (isPause) ? 1 : m_prePauseTimeScale;
		} 

		private bool CheckUpdateChanges()
		{
			bool isChange = (
				m_isMapStart != isMapStart ||
				m_isPaused != isPaused ||
				m_isGameEnd != isGameEnd ||
				m_isWin != isWin ||
				m_isPlaying != isPlaying) ||
				m_isStop != isStop;

			if (isChange) UpdateChanges();
			return isChange;
		}
		private void UpdateChanges()
		{
			m_isMapStart = isMapStart;
			m_isPaused = isPaused;
			m_isGameEnd = isGameEnd;
			m_isWin = isWin;
			m_isPlaying = isPlaying;
			m_isStop = isStop;

			m_world.GameplayChange();
			m_interface.GameplayChange();
		} 
	}

	public interface IGameplay
	{
		bool isMapStart { get; }
		bool isPaused { get; }
		bool isGameEnd { get; }
		bool isWin { get; }
		bool isPlaying { get; }
		bool isStop { get; }
	}
}
