using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

namespace MyGame
{
	public class GameplayUI : MonoBehaviour, IPlayerBar, UIContainer
	{
		public PointDelegate moveShip;
		public BoolEventDelegate uncontrollEvents;
		public BoolEventDelegate onPause;
		public EventDelegate firstTouchEvents;
		public EventDelegate onRestart;

		public IGameplay gameplay { get; set; }
		public int points
		{
			get { return (int)m_points.value; }
			set { m_points.SetValue(value); }
		}
		public int modifications
		{
			get;
			set;
		}
		public int progress
		{
			get;
			set;
		}

		public const float SLOWMO_CHANGE_TIME = 0.3f;

		public void OnGameplayChange()
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
		public void GameOver()
		{
		}
		public void Restart()
		{
			SceneManager.LoadScene("DemoScene");
		}
		public void Cleanup()
		{
			List<Component> toDelete = new List<Component>();
			toDelete.AddRange(Utils.GetChilds<Component>(m_barsParent));
			toDelete.ForEach(element => Destroy(element.gameObject));
		}
		public void Add(UIBar bar)
		{
		}

		[SerializeField]
		public Image m_slowmoCurtain;
		[SerializeField]
		public Transform m_barsParent;
		[SerializeField]
		public PointsBar m_points;
		private bool m_isPlayerControll = false;

		private bool isSlowMode
		{
			get
			{
				return
					gameplay.isMapStart &&
					isFirstTouchCreated &&
					!m_isPlayerControll &&
					!gameplay.isGameEnd &&
					isSecondTouchCreated;
			}
		}
		private bool isFirstTouchCreated { get; set; }
		private bool isSecondTouchCreated { get; set; }

		private const float MAX_CURTAIN_TRANSPARENCY = 0.8f;

		private void Awake()
		{
			firstTouchEvents += () => { isFirstTouchCreated = true; };
			OnStartNewGame();
		}
		private void FixedUpdate()
		{
			UpdateCurtain();
			ControllInterface();

			if (gameplay.isMapStart)
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
			screenPosition.z = Camera.main.transform.position.y;
			screenPosition = Camera.main.ScreenToWorldPoint(screenPosition);
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
			uncontrollEvents(isSlowMode);
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
		int points { get; set; }
		int modifications { get; set; }
		int progress { get; set; }
	}

	public interface UIContainer
	{
		void Add(UIBar bar);
	}
}
