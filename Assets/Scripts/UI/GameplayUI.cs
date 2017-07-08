using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;

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
				currentBehaviour = prePlayingBehaviour;
			}
			else if (gameplay.isPlaying)
			{
				currentBehaviour = playingBehaviour;
			}
			else if (gameplay.isPaused)
			{
				currentBehaviour = pauseBehaviour;
			}
		}
		public void OnMapStart()
		{
		}
		public void Ending()
		{
			m_slowmoCurtain.CrossFadeAlpha(1, ENDING_FADE_TIME, true);
		}
		public void Pause(bool isPause)
		{
			onPause(isPause);
			m_pauseInterface.SetActive(isPause);
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
		private EventTrigger m_pauseTrigger;

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

		private const float MAX_CURTAIN_TRANSPARENCY = 0.8f;
		private const float TOUCH_OFFSET_Y = 0.035f;
		private const float CAMERA_ANGLE_FACTOR = 0.076f;
		private const float PAUSE_BUTTON_SIZE_FACTOR = 0.07f;
		private const float AREA_SIZE_FACTOR = 0.28f;
		private const float AREA_POS_FACTOR = 0.02f;

		private void Awake()
		{
			m_pauseTrigger = m_pauseButton.GetComponent<EventTrigger>();

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

			m_shipArea.gameObject.SetActive(true);
			float areaSize = AREA_SIZE_FACTOR * Screen.width;
			Utils.SetSize(m_shipArea.GetComponent<RectTransform>(), areaSize);
		}
		private void InitBehaviours()
		{
			prePlayingBehaviour += WaitStartTouch;
			prePlayingBehaviour += UpdatePreStartInterface;

			playingBehaviour += ControllShip;
			playingBehaviour += UpdateSlowmoElements;
		}

		private void FixedUpdate()
		{
			if (currentBehaviour != null) currentBehaviour();
		}

		private void WaitStartTouch()
		{
			if (isFirstTouchCreated || !Input.GetMouseButton(0))
			{
				return;
			}

			isFirstTouchCreated = true;
			if (firstTouchEvents != null) firstTouchEvents();

			Animation areaAnim = m_shipArea.GetComponent<Animation>();
			if (areaAnim && areaAnim.clip)
			{
				areaAnim.Play();
				m_shipArea.CrossFadeAlpha(0, areaAnim.clip.length, true);
			}
		}
		private void UpdatePreStartInterface()
		{
			Vector3 areaPosition = Camera.main.WorldToScreenPoint(world.shipPosition);
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
			screenPosition.z = Camera.main.transform.position.y;
			screenPosition = Camera.main.ScreenToWorldPoint(screenPosition);
			screenPosition.x += screenPosition.x * -CAMERA_ANGLE_FACTOR;
			screenPosition.z += screenPosition.z * -CAMERA_ANGLE_FACTOR;
			if (moveShip != null) moveShip(screenPosition);
		}
		private void UpdateSlowmoElements()
		{
			if (uncontrollEvents != null) uncontrollEvents(isModeOn);

			float target = (isModeOn) ? MAX_CURTAIN_TRANSPARENCY : 0;
			m_slowmoCurtain.CrossFadeAlpha(target, SLOWMO_CHANGE_TIME, true);

			target = (isModeOn) ? 1 : 0;
			m_pauseTrigger.enabled = isModeOn;
			if (isModeOn) m_pauseButton.gameObject.SetActive(true);
			m_pauseButton.targetGraphic.CrossFadeAlpha(target, SLOWMO_CHANGE_TIME, true);
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
