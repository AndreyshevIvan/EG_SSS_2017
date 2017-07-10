using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyGame.Hero;
using MyGame.Factory;
using FluffyUnderware.Curvy;
using MyGame.GameUtils;

namespace MyGame
{
	public sealed class GameplayController : MonoBehaviour, IGameplay
	{
		public bool isMapStart { get; private set; }
		public bool isPaused { get; private set; }
		public bool isGameEnd
		{
			get
			{
				return !ship.isLive || (map.isReached && m_world.isAllEnemiesKilled);
			}
		}
		public bool isWin
		{
			get
			{
				return isGameEnd && m_world.ship.isLive;
			}
		}
		public bool isPlaying
		{
			get
			{
				return !isPaused && isMapStart && !isGameEnd;
			}
		}

		public void Continue()
		{
			SceneManager.LoadScene("Main");
		}

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
		private ShipProperties m_shipProperties = new ShipProperties();
		private Updater m_updater;

		private bool m_isMapStart;
		private bool m_isPaused;
		private bool m_isGameEnd;
		private bool m_isWin;
		private bool m_isPlaying;

		private float m_prePauseTimeScale;

		private const float SHIP_PRE_START_SPEED = 4;
		private const float SHIP_START_SPEED = 6;
		public const float ENDING_WAITING_TIME = 3;

		private void Awake()
		{
			m_updater = GetComponent<Updater>();
			QualitySettings.vSyncCount = 0;

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

			m_interface.moveShip += MoveShip;

			m_interface.onChangeMode += isTrue => m_world.SetSlowMode(isTrue);

			m_interface.startTouchEvents += () =>
			{
				ship.roadController.Spline = null;
				m_updater.Add(MoveShipToStartRoad, SetStartRoad, UpdType.FIXED);
			};

			m_interface.onBomb = () => { Debug.Log("Bomb"); return true; };
			m_interface.onLaser = () => { Debug.Log("Laser"); return true; };
		}

		private void FixedUpdate()
		{
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
		private void OnMapReached()
		{
			if (!isWin)
			{
				m_world.KillPlayer();
			}

			SaveUserData();

			Utils.DoAfterTime(this, ENDING_WAITING_TIME, () =>
			{
				m_world.player.isWin = isWin;
				m_interface.ViewResults(null, null);
			});
		}
		private void Pause(bool isPause)
		{
			isPaused = isPause;

			if (isPause)
			{
				m_prePauseTimeScale = Time.timeScale;
			}

			Time.timeScale = (isPause) ? 0 : m_prePauseTimeScale;
		} 

		private bool CheckUpdateChanges()
		{
			bool isChange = (
				m_isMapStart != isMapStart ||
				m_isPaused != isPaused ||
				m_isGameEnd != isGameEnd ||
				m_isWin != isWin ||
				m_isPlaying != isPlaying);

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

			m_world.GameplayChange();
			m_interface.GameplayChange();
		}
		private void SaveUserData()
		{
			//User user = GameData.LoadUser();
			//User oldUser = (User)user.Clone();
			//user.AddNew(m_world.player);
			//User newUser = (User)user.Clone();
			//GameData.SaveUser(user);
		}

		private void MoveShip(Vector3 targetPosition)
		{
			if (ship) ship.MoveTo(targetPosition);
		}
		private bool MoveShipToStartRoad()
		{
			CurvySpline spline = m_factory.GetRoad(RoadType.PLAYER);
			Vector3 target = spline.Segments[0].transform.position;
			float movement = SHIP_START_SPEED * Time.fixedDeltaTime;
			ship.position = Vector3.MoveTowards(ship.position, target, movement);
			return ship.position == target;
		}
	}

	public interface IGameplay
	{
		bool isMapStart { get; }
		bool isPaused { get; }
		bool isGameEnd { get; }
		bool isWin { get; }
		bool isPlaying { get; }
	}
}
