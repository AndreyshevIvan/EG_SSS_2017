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

		public const float ENDING_FADE_TIME = 0.4f;
		public const float SLOWMO_CHANGE_TIME = 0.3f;

		public void Init(IGameWorld gameWorld)
		{
			gameplay = gameWorld as IGameplay;
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
		private Transform m_barsParent;
		[SerializeField]
		private PointsBar m_points;
		[SerializeField]
		private ModificationBar m_modsBar;
		[SerializeField]
		private Button m_pausButton;
		[SerializeField]
		private GameObject m_pauseInterface;

		private bool m_isControll = false;

		private IGameplay gameplay { get; set; }
		private RectTransform pauseButtonRT { get; set; }
		private bool isFirstTouchCreated { get; set; }
		private bool isControllPlayer
		{
			get
			{
				return gameplay.isPlaying && m_isControll;
			}
		}
		private Vector3 clickPosition { get; set; }

		private const float MAX_CURTAIN_TRANSPARENCY = 0.8f;
		private const float TOUCH_OFFSET_Y = 0.035f;
		private const float CAMERA_ANGLE_FACTOR = 0.076f;
		private const float PAUSE_BUTTON_SIZE_FACTOR = 0.06f;

		private void Awake()
		{
			pauseButtonRT = m_pausButton.GetComponent<RectTransform>();
			Utils.SetSize(pauseButtonRT, Screen.width * PAUSE_BUTTON_SIZE_FACTOR);

			m_slowmoCurtain.CrossFadeAlpha(0, 0, true);
			m_pauseInterface.SetActive(false);
			m_pausButton.targetGraphic.CrossFadeAlpha(0, 0, true);

			prePlayingBehaviour += WaitFirstTouch;

			playingBehaviour += ControllInterface;
			playingBehaviour += ControllShip;
			playingBehaviour += UpdateCurtain;


		}
		private void FixedUpdate()
		{
			currentBehaviour();
		}

		private void OnMouseEnter()
		{
			clickPosition = Input.mousePosition;
			Debug.Log(clickPosition);
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
		private void ControllInterface()
		{
			if (!Input.GetMouseButton(0))
			{
				return;
			}

			if (Input.GetMouseButtonUp(0))
			{
				if (!pauseButtonRT.rect.Contains(Input.mousePosition))
				{
					return;
				}

				Pause(true);
			}
		}
		private void UpdateCurtain()
		{
			bool isModeOn = isFirstTouchCreated && !isControllPlayer;
			if (uncontrollEvents != null) uncontrollEvents(isModeOn);
			float target = (isModeOn) ? MAX_CURTAIN_TRANSPARENCY : 0;
			m_slowmoCurtain.CrossFadeAlpha(target, SLOWMO_CHANGE_TIME, true);
		}
		private void WaitFirstTouch()
		{
			if (isFirstTouchCreated || !Input.GetMouseButton(0))
			{
				return;
			}

			isFirstTouchCreated = true;
			firstTouchEvents();
			// TODO: closing interface
		}

		private void Pause(bool isPause)
		{
			onPause(isPause);
			m_pauseInterface.SetActive(isPause);
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
