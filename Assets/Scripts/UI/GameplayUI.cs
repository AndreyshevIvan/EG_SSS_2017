using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MyGame
{
	public class GameplayUI : MonoBehaviour, IBarsFactory, IPlayerBar
	{
		public PointDelegate moveShip;
		public BoolEventDelegate uncontrollEvents;
		public BoolEventDelegate onPause;
		public EventDelegate firstTouchEvents;
		public EventDelegate onRestart;

		public Image m_slowmoCurtain;
		public Transform m_barsParent;
		public HealthBar m_shipHealthBar;
		public HealthBar m_enemyHealthBar;
		public PointsBar m_points;

		public IGameplay gameplay { get; set; }
		public UIBar shipHealth
		{
			get
			{
				UIBar bar = Instantiate(m_shipHealthBar, m_barsParent);
				bar.transform.position = Vector3.one * float.MaxValue;
				return bar;
			}
		}
		public UIBar enemyHealth
		{
			get
			{
				UIBar bar = Instantiate(m_enemyHealthBar, m_barsParent);
				bar.transform.position = Vector3.one * float.MaxValue;
				return bar;
			}
		}
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

		private const float MAX_CURTAIN_TRANSPARENCY = 160;

		private void Awake()
		{
			firstTouchEvents += () => { isFirstTouchCreated = true; };
			OnStartNewGame();
		}
		private void Update()
		{
			UpdateCurtain();
		}
		private void FixedUpdate()
		{
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
			m_slowmoCurtain.CrossFadeAlpha(target / 255, SLOWMO_CHANGE_TIME, true);
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

	public interface IBarsFactory
	{
		UIBar shipHealth { get; }
		UIBar enemyHealth { get; }
	}

	public interface IPlayerBar
	{
		int points { get; set; }
		int modifications { get; set; }
		int progress { get; set; }
	}
}
