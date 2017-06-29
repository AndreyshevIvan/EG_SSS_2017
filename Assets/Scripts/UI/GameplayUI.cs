using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MyGame
{
	public class GameplayUI : MonoBehaviour, IBarsFactory, IPlayerBar
	{
		public PointDelegate onControllPlayer;
		public BoolEventDelegate onBeginControllPlayer;
		public BoolEventDelegate onPause;
		public EventDelegate onFirstTouch;
		public EventDelegate onRestart;

		public Image m_slowmoCurtain;
		public Transform m_barsParent;
		public HealthBar m_shipHealthBar;
		public HealthBar m_enemyHealthBar;
		public PointsBar m_points;

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

		private bool m_isSlowMode = true;

		private bool isFirstTouchCreated { get; set; }
		private bool isControllAvailable { get; set; }

		private const float MAX_CURTAIN_TRANSPARENCY = 160;

		private void Awake()
		{
			OnStartNewGame();
			onBeginControllPlayer += UpdateCurtain;
		}
		private void Update()
		{
			HandleMouse();
		}
		private void HandleMouse()
		{
			if (!Input.GetMouseButton(0))
			{
				onBeginControllPlayer(isFirstTouchCreated);
				return;
			}

			onBeginControllPlayer(false);
			OnCreateFirstTouch();
			SetPosition(Input.mousePosition);
		}
		private void SetPosition(Vector3 screenPosition)
		{
			screenPosition.z = Camera.main.transform.position.y;
			screenPosition = Camera.main.ScreenToWorldPoint(screenPosition);
			onControllPlayer(screenPosition);
		}
		private void UpdateCurtain(bool isModeOn)
		{
			if (m_isSlowMode == isModeOn)
			{
				return;
			}

			float target = (isModeOn) ? MAX_CURTAIN_TRANSPARENCY : 0;
			m_slowmoCurtain.CrossFadeAlpha(target / 255, SLOWMO_CHANGE_TIME, true);
			m_isSlowMode = isModeOn;
		}
		private void OnStartNewGame()
		{
			isFirstTouchCreated = false;
		}
		private void OnCreateFirstTouch()
		{
			if (isFirstTouchCreated)
			{
				return;
			}

			onFirstTouch();
			isFirstTouchCreated = true;
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
