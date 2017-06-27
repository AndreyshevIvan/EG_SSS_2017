using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MyGame
{
	public class GameplayUI : MonoBehaviour
	{
		public PointDelegate onControllPlayer;
		public BoolEventDelegate onBeginControllPlayer;
		public BoolEventDelegate onPause;
		public EventDelegate onFirstTouch;
		public EventDelegate onRestart;

		public const float SLOW_TIME = 0.2f;

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

		private bool isFirstTouchCreated { get; set; }
		private bool isControllAvailable { get; set; }

		private void Awake()
		{
			OnStartNewGame();
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
}
