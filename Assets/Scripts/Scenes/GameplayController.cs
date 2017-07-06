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

		private bool m_isMapStart;
		private bool m_isPaused;
		private bool m_isGameEnd;
		private bool m_isWin;
		private bool m_isPlaying;
		private bool m_isStop;

		private const int FRAME_RATE = 60;

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
			if (!CheckUpdateChanges() || !isGameEnd)
			{
				return;
			}

			StartCoroutine(OnMapReached(isWin));
		}

		private void OnMapStart()
		{
			isMapStart = true;
			map.Play();
			m_interface.OnMapStart();
			Destroy(ship.roadController);
		}
		private IEnumerator OnMapReached(bool isWin)
		{
			if (!isWin)
			{
				m_world.KillPlayer();
			}

			SaveUserData();
			yield return new WaitForSeconds(ENDING_WAITING_TIME);
			isStop = true;
			m_interface.Ending();
			yield return new WaitForSeconds(GameplayUI.ENDING_FADE_TIME);
			m_resultsUI.gameObject.SetActive(true);
			m_resultsUI.Open(null, null, m_world.player, isWin);
		}
		private void SaveUserData()
		{
			//User user = GameData.LoadUser();
			//User oldUser = (User)user.Clone();
			//user.AddNew(m_world.player);
			//User newUser = (User)user.Clone();
			//GameData.SaveUser(user);
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
