using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

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
		}
		public void OnMapStart()
		{
		}
		public void Pause(bool isPause)
		{
			onPause(isPause);
			OpenPauseInterface(isPause);
		}
		public void Ending()
		{
			m_slowmoCurtain.CrossFadeAlpha(1, ENDING_FADE_TIME, true);
		}
		public void Cleanup()
		{
			List<Component> toDelete = new List<Component>();
			toDelete.AddRange(Utils.GetChilds<Component>(m_barsParent));
			toDelete.ForEach(element => Destroy(element.gameObject));
		}
		public void Add(UIBar bar)
		{
			bar.transform.SetParent(m_barsParent);
			bar.controller = this;
		}
		public void Erase(UIBar bar)
		{
		}

		[SerializeField]
		private Image m_slowmoCurtain;
		[SerializeField]
		private Transform m_barsParent;
		[SerializeField]
		private PointsBar m_points;
		[SerializeField]
		private ModificationBar m_modsBar;

		private bool m_isPlayerControll = false;

		private IGameplay gameplay { get; set; }
		private bool isSlowMode
		{
			get
			{
				return
					isFirstTouchCreated &&
					isSecondTouchCreated &&
					!m_isPlayerControll &&
					gameplay.isPlaying;
			}
		}
		private bool isFirstTouchCreated { get; set; }
		private bool isSecondTouchCreated { get; set; }

		private const float MAX_CURTAIN_TRANSPARENCY = 0.8f;
		private const float TOUCH_OFFSET_Y = 0.035f;
		private const float CAMERA_ANGLE_FACTOR = 0.076f;

		private void Awake()
		{
			firstTouchEvents += () => { isFirstTouchCreated = true; };
			OnStartNewGame();
		}

		private void FixedUpdate()
		{
			if (gameplay.isStop)
			{
				return;
			}

			ControllInterface();
			UpdateCurtain();

			if (gameplay.isPlaying)
			{
				ControllShip();
			}
		}
		private void ControllShip()
		{
			if (!Input.GetMouseButton(0))
			{
				m_isPlayerControll = false;
				return;
			}

			isSecondTouchCreated = m_isPlayerControll = true;
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

			if (!isFirstTouchCreated)
			{
				firstTouchEvents();
			}
		}
		private void UpdateCurtain()
		{
			if (uncontrollEvents != null) uncontrollEvents(isSlowMode);
			float target = (isSlowMode) ? MAX_CURTAIN_TRANSPARENCY : 0;
			m_slowmoCurtain.CrossFadeAlpha(target, SLOWMO_CHANGE_TIME, true);
		}

		private void OnStartNewGame()
		{
			isFirstTouchCreated = false;
			m_isPlayerControll = false;
			m_slowmoCurtain.CrossFadeAlpha(0, 0, true);
		}

		private void OpenPauseInterface(bool isPause)
		{
		}

		private bool IsStartPause()
		{
			return isFirstTouchCreated && false;
		}
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
