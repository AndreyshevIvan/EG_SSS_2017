using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;
using MyGame.Hero;
using MyGame.GameUtils;

namespace MyGame
{
	public partial class GameplayUI : MonoBehaviour, IPlayerBar, UIContainer
	{
		public PointDelegate moveShip;
		public BoolEventDelegate onChangeMode;
		public BoolEventDelegate onPause;
		public EventDelegate startTouchEvents;
		public EventDelegate onRestart;

		public int points { set { m_points.SetValue(value); } }
		public int modifications { set { m_modsBar.SetValue(value); } }

		public const float ENDING_FADE_DURATION = 0.4f;
		public const float SLOWMO_OPEN_DUR = 0.3f;

		public void Init(IGameplay gameplay)
		{
			this.gameplay = gameplay;
		}
		public void GameplayChange()
		{
			currentBehaviour = () => { };

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
				if (gameplay.isWin) currentBehaviour = ControllShip;
				Utils.DoAfterTime(this, Ship.ENDING_CONTROLL_DURATION, () =>
				{
					currentBehaviour = () => { };
					OnGameEnd();
				});
			}
		}

		public void StartGame()
		{
			if (startTouchEvents != null) startTouchEvents();
			m_shipArea.GetComponent<Button>().interactable = false;
			m_shipArea.GetComponent<Animator>().SetTrigger(AREA_EXIT_TRIGGER);
			m_animator.SetTrigger(CLOSE_LEVEL_INFO);
		}
		public void Controll(bool isControll)
		{
			if (!gameplay.isPlaying)
			{
				return;
			}

			if (isControll)
			{
				lastTouch = UITouch.CONTROLL;
				SetSlowMode(false);
				return;
			}

			lastTouch = UITouch.WAIT;
			SetSlowMode(true);
		}
		public void Pause(bool isPause)
		{
			onPause(isPause);
			SetActive(m_pauseInterface, isPause);
		}
		public void Bomb()
		{
			if (player.Bomb())
			{
				lastTouch = UITouch.SPELL;
				SetSlowMode(false);
			}
		}
		public void Laser()
		{
			if (player.Laser())
			{
				lastTouch = UITouch.SPELL;
				SetSlowMode(false);
			}
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
		private Image m_curtain;
		[SerializeField]
		private Transform m_controll;
		[SerializeField]
		private Transform m_slowButtons;
		[SerializeField]
		private Image m_shipArea;
		[SerializeField]
		private Transform m_barsParent;
		[SerializeField]
		private PointsBar m_points;
		[SerializeField]
		private ModificationBar m_modsBar;
		[SerializeField]
		private RectTransform m_pauseButton;
		[SerializeField]
		private Component m_pauseInterface;
		[SerializeField]
		private Component m_levelTitle;
		[SerializeField]
		private SpellBar m_bombBar;
		[SerializeField]
		private SpellBar m_laserBar;
		private Animator m_animator;
		private Camera m_camera;
		private List<Graphic> m_slowButtonsGraphic;

		private IGameplay gameplay { get; set; }
		private UITouch lastTouch { get; set; }
		private BoundingBox box { get; set; }

		private const float TOUCH_OFFSET_Y = 0.04f;
		private const float CAMERA_MAX_OFFSET = 2;
		private const float CAMERA_SPEED = 7;
		private const float CAMERA_ANGLE_FACTOR = 0.076f;
		private const float PAUSE_BUTTON_SIZE_FACTOR = 0.08f;
		private const float AREA_SIZE_FACTOR = 0.35f;
		private const float AREA_POS_FACTOR = 0.02f;

		private const float MAX_CURTAIN_TRANSPARENCY = 0.8f;
		private const float BARS_FADING_DURATION = 1.2f;

		private const string AREA_EXIT_TRIGGER = "AreaExit";
		private const string OPEN_LEVEL_INFO = "OpenLevelInfo";
		private const string CLOSE_LEVEL_INFO = "CloseLevelInfo";
		private const string CLOSE_BARS = "CloseBars";

		private void Awake()
		{
			m_camera = Camera.main;
			Input.multiTouchEnabled = false;
			m_animator = GetComponent<Animator>();
			m_slowButtonsGraphic = Utils.GetAllComponents<Graphic>(m_slowButtons);
			lastTouch = UITouch.WAIT;
			box = GameData.box;

			InitUIElements();
			InitBehaviours();
			SetSlowMode(false);
		}
		private void InitUIElements()
		{
			m_curtain.CrossFadeAlpha(0, 0, true);

			Utils.SetSize(m_pauseButton, Screen.width * PAUSE_BUTTON_SIZE_FACTOR);

			float areaSize = AREA_SIZE_FACTOR * Screen.width;
			Utils.SetSize(m_shipArea.GetComponent<RectTransform>(), areaSize);
		}
		private void InitBehaviours()
		{
			prePlayingBehaviour += UpdatePreStartInterface;

			playingBehaviour += ControllShip;
			playingBehaviour += UpdateGunBars;
		}

		private void FixedUpdate()
		{
			if (currentBehaviour != null) currentBehaviour();
		}
		private void UpdatePreStartInterface()
		{
			Vector3 areaPosition = m_camera.WorldToScreenPoint(player.shipPosition);
			areaPosition.y += Screen.height * AREA_POS_FACTOR;
			m_shipArea.transform.position = areaPosition;
		}
		private void ControllShip()
		{
			if (lastTouch != UITouch.CONTROLL || !Input.GetMouseButton(0))
			{
				return;
			}

			Vector3 screenPosition = Input.mousePosition;
			screenPosition.y += TOUCH_OFFSET_Y * Screen.height;
			screenPosition.z = m_camera.transform.position.y;
			screenPosition = m_camera.ScreenToWorldPoint(screenPosition);
			screenPosition.x += screenPosition.x * -CAMERA_ANGLE_FACTOR;
			screenPosition.z += screenPosition.z * -CAMERA_ANGLE_FACTOR;
			if (moveShip != null) moveShip(screenPosition);
			UpdateCameraPosition();
		}
		private void UpdateCameraPosition()
		{
			float playerXPart = player.shipPosition.x / box.xMax;
			float camMove = playerXPart * playerXPart * CAMERA_MAX_OFFSET;
			camMove = (player.shipPosition.x < 0) ? -camMove : camMove;
			Vector3 newCameraPos = m_camera.transform.position;
			newCameraPos.x = Mathf.MoveTowards(newCameraPos.x, camMove, CAMERA_SPEED * Time.fixedDeltaTime);
			m_camera.transform.position = newCameraPos;
		}
		private void UpdateGunBars()
		{
			m_bombBar.SetValue((int)(player.bombProcess * 100));
			m_laserBar.SetValue((int)(player.laserProcess * 100));
		}

		public void SetSlowMode(bool isModeOn)
		{
			if (onChangeMode != null) onChangeMode(isModeOn);

			float curtainAlpha = (isModeOn) ? MAX_CURTAIN_TRANSPARENCY : 0;
			m_curtain.CrossFadeAlpha(curtainAlpha, SLOWMO_OPEN_DUR, true);

			float buttonsAlpha = (isModeOn) ? MAX_CURTAIN_TRANSPARENCY : 0;
			Utils.FadeList(m_slowButtonsGraphic, buttonsAlpha, SLOWMO_OPEN_DUR);

			if (isModeOn)
			{
				m_slowButtons.transform.SetParent(m_controll);
				return;
			}

			m_slowButtons.transform.SetParent(m_curtain.transform);
		}

		private void OnPrePlaying()
		{
			CloseAll();
			SetActive(m_shipArea, true);
			SetActive(m_levelTitle, true);

			m_points.Fade(0, 0);
			m_modsBar.Fade(0, 0);
			m_animator.Play(OPEN_LEVEL_INFO);
		}
		private void OnPlaying()
		{
			CloseAll();
			SetActive(m_pauseButton, true);
			SetActive(m_points, true);
			SetActive(m_modsBar, true);
			SetActive(m_bombBar, true);
			SetActive(m_laserBar, true);
			SetActive(m_curtain, true);

			m_curtain.CrossFadeAlpha(0, 0, true);
			m_points.Fade(1, BARS_FADING_DURATION);
			m_modsBar.Fade(1, BARS_FADING_DURATION);
			Utils.FadeList(m_slowButtonsGraphic, 0);
		}
		private void OnGameEnd()
		{
			CloseAll();
			SetActive(m_results, true);

			SetSlowMode(false);
			Utils.FadeElement(m_results.transform, 0, 0);
			m_points.Fade(0, BARS_FADING_DURATION);
			m_modsBar.Fade(0, BARS_FADING_DURATION);
		}
		private void CloseAll()
		{
			SetActive(m_curtain, false);
			SetActive(m_results, false);
			SetActive(m_pauseButton, false);
			SetActive(m_pauseInterface, false);
			SetActive(m_shipArea, false);
			SetActive(m_points, false);
			SetActive(m_modsBar, false);
			SetActive(m_levelTitle, false);
			SetActive(m_bombBar, false);
			SetActive(m_laserBar, false);
		}
		private void SetActive(Component element, bool isOpen)
		{
			element.gameObject.SetActive(isOpen);
		}

		private EventDelegate currentBehaviour;
		private EventDelegate prePlayingBehaviour;
		private EventDelegate playingBehaviour;
		private EventDelegate pauseBehaviour;
		private EventDelegate winBehaviour;

		private enum UITouch
		{
			SPELL,
			WAIT,
			CONTROLL,
		}
	}

	public partial class GameplayUI : MonoBehaviour, IPlayerBar, UIContainer
	{
		public void ViewResults(User oldUser, User newUser)
		{
			CalcData();
			InitResultsInterface();
		}

		[SerializeField]
		private Text m_endingTitle;
		[SerializeField]
		private Text m_levelCompleteTxt;
		[SerializeField]
		private Text m_clearVictoryTxt;
		[SerializeField]
		private Text m_allKillsTxt;
		[SerializeField]
		private Text m_starsTxt;
		[SerializeField]
		private Text m_starsValue;
		[SerializeField]
		private Text m_pointsValue;

		[SerializeField]
		private Color m_completeColor;
		[SerializeField]
		private Image m_levelCompleteIco;
		[SerializeField]
		private Image m_clearVictoryIco;
		[SerializeField]
		private Image m_allKillsIco;
		[SerializeField]
		private Image m_pointsLine;

		[SerializeField]
		private Component m_results;
		[SerializeField]
		private Button m_continue;

		private Player player { get { return gameplay.player; } }
		private User oldUser { get; set; }
		private User newUser { get; set; }

		private const float ACHIEVEMENTS_SIZE_FACTOR = 0.05f;
		private const float RESULTS_FADE_TIME = 0.2f;

		private const string OPEN_RESULTS = "OpenResults";

		private void CalcData()
		{
		}
		private void InitResultsInterface()
		{
			InitStrings();
			InitAchievements();

			Utils.FadeElement(m_results.transform, 1, RESULTS_FADE_TIME);

			if (m_animator) m_animator.Play(OPEN_RESULTS);
		}
		private void InitStrings()
		{
			m_endingTitle.text = StrManager.Get(6);
			m_levelCompleteTxt.text = StrManager.Get(8);
			m_clearVictoryTxt.text = StrManager.Get(9);
			m_allKillsTxt.text = StrManager.Get(10);
			m_starsTxt.text = StrManager.Get(7);

			int fontSize = Utils.GetFromSreen(ACHIEVEMENTS_SIZE_FACTOR);
			m_levelCompleteTxt.fontSize = fontSize;
			m_clearVictoryTxt.fontSize = fontSize;
			m_allKillsTxt.fontSize = fontSize;
			m_starsTxt.fontSize = fontSize;
			m_starsValue.fontSize = fontSize;
			m_pointsValue.fontSize = fontSize;
		}
		private void InitAchievements()
		{
			m_starsValue.text = player.stars.ToString();
			m_pointsValue.text = player.points.ToString();

			if (!player.isWin)
			{
				return;
			}

			m_levelCompleteIco.color = m_completeColor;
			if (!player.isDemaged) m_clearVictoryIco.color = m_completeColor;
			if (!player.isLossEnemy) m_allKillsIco.color = m_completeColor;
		}
	}

	public partial class GameplayUI : MonoBehaviour, IPlayerBar, UIContainer
	{
		public static Color GetShipBulletColor(float modsPart)
		{
			float part = 1 - modsPart;
			Color color = new Color(255, 255 * part, 255 * part);
			return color;
		}
	}

	public delegate void PointDelegate(Vector3 touchPositiion);
	public delegate void BoolEventDelegate(bool isStartOrEnd);
	public delegate void EventDelegate();
	public delegate bool ResultEvent();

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
