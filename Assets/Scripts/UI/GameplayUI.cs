using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;
using MyGame.Hero;

namespace MyGame
{
	public class GameplayUI : MonoBehaviour, IPlayerBar, UIContainer, IWorldEntity
	{
		public PointDelegate moveShip;
		public BoolEventDelegate uncontrollEvents;
		public BoolEventDelegate onPause;
		public EventDelegate firstTouchEvents;
		public EventDelegate onRestart;

		public int points { set { m_points.SetValue(value); } }
		public int modifications { set { m_modsBar.SetValue(value); } }
		public bool isFirstTouchCreated { get; private set; }

		public const float ENDING_FADE_TIME = 0.4f;
		public const float SLOWMO_CHANGE_TIME = 0.3f;

		public void Init(IGameWorld gameWorld)
		{
			world = gameWorld;
			gameplay = world as IGameplay;
		}
		public void GameplayChange()
		{
			currentBehaviour = () => { };
			isFirstTouchCreated = false;

			if (!gameplay.isMapStart)
			{
				OnPrePlaying();
				currentBehaviour = prePlayingBehaviour;
			}
			else if (gameplay.isPlaying)
			{
				OnPlaying();
				currentBehaviour = playingBehaviour;
			}
			else if (gameplay.isPaused)
			{
				currentBehaviour = pauseBehaviour;
			}
			else if (gameplay.isGameEnd)
			{
				currentBehaviour = ControllShip;
				Utils.DoAfterTime(this, Ship.ENDING_CONTROLL_DURATION, () =>
				{
					currentBehaviour = () => { };
					OnGameEnd();
				});
			}
		}

		public void StartGame()
		{
			if (firstTouchEvents != null) firstTouchEvents();
			m_shipArea.GetComponent<Button>().interactable = false;
			m_shipArea.GetComponent<Animator>().SetTrigger(AREA_EXIT_TRIGGER);
		}
		public void Pause(bool isPause)
		{
			onPause(isPause);
			m_pauseInterface.SetActive(isPause);
		}
		public void ViewResults(User oldUser, User newUser, Player player, bool isWin)
		{
			m_endingTitle.text = StrManager.Get((uint)((isWin) ? 6 : 7));

			this.oldUser = oldUser;
			this.newUser = newUser;
			this.player = player;

			if (m_animator) m_animator.Play(OPEN_RESULTS);
		}

		public void Add(UIBar bar)
		{
			bar.transform.SetParent(m_barsParent);
			bar.controller = this;
		}
		public void Erase(UIBar bar)
		{
		}
		public void Cleanup()
		{
			List<Component> toDelete = new List<Component>();
			toDelete.AddRange(Utils.GetChilds<Component>(m_barsParent));
			toDelete.ForEach(element => Destroy(element.gameObject));
		}

		[SerializeField]
		private Image m_slowmoCurtain;
		[SerializeField]
		private Image m_shipArea;
		[SerializeField]
		private Transform m_barsParent;
		[SerializeField]
		private PointsBar m_points;
		[SerializeField]
		private ModificationBar m_modsBar;
		[SerializeField]
		private Button m_pauseButton;
		[SerializeField]
		private GameObject m_pauseInterface;
		[SerializeField]
		private Text m_endingTitle;
		[SerializeField]
		private GameObject m_results;
		private Animator m_animator;
		private Camera m_camera;
		private bool m_isControll = false;

		private IGameWorld world { get; set; }
		private IGameplay gameplay { get; set; }
		private bool isControllPlayer
		{
			get
			{
				return gameplay.isPlaying && m_isControll;
			}
		}
		private bool isModeOn
		{
			get { return isFirstTouchCreated && !isControllPlayer; }
		}

		private User oldUser { get; set; }
		private User newUser { get; set; }
		private Player player { get; set; }

		private const float TOUCH_OFFSET_Y = 0.035f;
		private const float CAMERA_ANGLE_FACTOR = 0.076f;
		private const float PAUSE_BUTTON_SIZE_FACTOR = 0.07f;
		private const float AREA_SIZE_FACTOR = 0.28f;
		private const float AREA_POS_FACTOR = 0.02f;

		private const float MAX_CURTAIN_TRANSPARENCY = 0.8f;
		private const float RESULTS_FADE_TIME = 0.4f;

		private const string AREA_EXIT_TRIGGER = "AreaExit";
		private const string OPEN_LEVEL_INFO = "OpenLevelInfo";
		private const string CLOSE_LEVEL_INFO = "CloseLevelInfo";
		private const string OPEN_RESULTS = "OpenResults";
		private const string CLOSE_BARS = "CloseBars";

		private void Awake()
		{
			m_camera = Camera.main;
			Input.multiTouchEnabled = false;
			m_animator = GetComponent<Animator>();

			InitUIElements();
			InitBehaviours();
		}
		private void InitUIElements()
		{
			m_slowmoCurtain.gameObject.SetActive(true);
			m_slowmoCurtain.CrossFadeAlpha(0, 0, true);
			m_pauseInterface.SetActive(false);

			m_pauseButton.targetGraphic.CrossFadeAlpha(0, 0, true);

			RectTransform pauseRect = m_pauseButton.GetComponent<RectTransform>();
			Utils.SetSize(pauseRect, Screen.width * PAUSE_BUTTON_SIZE_FACTOR);

			float areaSize = AREA_SIZE_FACTOR * Screen.width;
			Utils.SetSize(m_shipArea.GetComponent<RectTransform>(), areaSize);
		}
		private void InitBehaviours()
		{
			prePlayingBehaviour += UpdatePreStartInterface;

			playingBehaviour += ControllShip;
			playingBehaviour += UpdateSlowmoElements;
		}

		private void FixedUpdate()
		{
			if (currentBehaviour != null) currentBehaviour();
		}
		private void UpdatePreStartInterface()
		{
			Vector3 areaPosition = m_camera.WorldToScreenPoint(world.shipPosition);
			areaPosition.y += Screen.height * AREA_POS_FACTOR;
			m_shipArea.transform.position = areaPosition;
		}
		private void ControllShip()
		{
			m_isControll = Input.GetMouseButton(0);

			if (!m_isControll)
			{
				return;
			}

			isFirstTouchCreated = true;
			Vector3 screenPosition = Input.mousePosition;
			screenPosition.y += TOUCH_OFFSET_Y * Screen.height;
			screenPosition.z = m_camera.transform.position.y;
			screenPosition = m_camera.ScreenToWorldPoint(screenPosition);
			screenPosition.x += screenPosition.x * -CAMERA_ANGLE_FACTOR;
			screenPosition.z += screenPosition.z * -CAMERA_ANGLE_FACTOR;
			if (moveShip != null) moveShip(screenPosition);
		}
		private void UpdateSlowmoElements()
		{
			if (gameplay.isGameEnd)
			{
				m_pauseButton.gameObject.SetActive(false);
				return;
			}

			if (uncontrollEvents != null) uncontrollEvents(isModeOn);

			float target = (isModeOn) ? MAX_CURTAIN_TRANSPARENCY : 0;
			m_slowmoCurtain.CrossFadeAlpha(target, SLOWMO_CHANGE_TIME, true);

			target = (isModeOn) ? 1 : 0;
			if (isModeOn) m_pauseButton.gameObject.SetActive(true);
			m_pauseButton.targetGraphic.CrossFadeAlpha(target, SLOWMO_CHANGE_TIME, true);
		}

		private void OnPrePlaying()
		{
			m_pauseButton.gameObject.SetActive(false);
			m_animator.Play(OPEN_LEVEL_INFO);
		}
		private void OnPlaying()
		{
			m_shipArea.gameObject.SetActive(false);
			m_pauseButton.gameObject.SetActive(true);
			m_pauseButton.targetGraphic.CrossFadeAlpha(0, 0, true);
			m_animator.SetTrigger(CLOSE_LEVEL_INFO);
		}
		private void OnGameEnd()
		{
			m_pauseButton.gameObject.SetActive(false);
			m_results.SetActive(true);
			List<Graphic> graphic = Utils.GetAllComponents<Graphic>(m_results.transform);
			graphic.ForEach(element => element.CrossFadeAlpha(0, 0, true));
			Utils.DoAfterTime(this, GameplayController.ENDING_WAITING_TIME, () =>
			{
				m_points.FadeClose(ENDING_FADE_TIME);
				m_modsBar.FadeClose(ENDING_FADE_TIME);
			});
		}

		private EventDelegate currentBehaviour;
		private EventDelegate prePlayingBehaviour;
		private EventDelegate playingBehaviour;
		private EventDelegate pauseBehaviour;
		private EventDelegate winBehaviour;
	}

	public delegate void PointDelegate(Vector3 touchPositiion);
	public delegate void BoolEventDelegate(bool isStartOrEnd);
	public delegate void EventDelegate();

	public interface IPlayerBar
	{
		int points { set; }
		int modifications { set; }
	}

	public interface UIContainer
	{
		void Add(UIBar bar);
		void Erase(UIBar bar);
	}
}
