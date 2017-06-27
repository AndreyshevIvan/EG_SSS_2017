using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
	public class GameplayUI : MonoBehaviour
	{
		public PointDelegate onControllPlayer;
		public BoolEventDelegate onBeginControllPlayer;
		public BoolEventDelegate onPause;
		public EventDelegate onFirstTouch;
		public EventDelegate onRestart;

		public void Pause(bool isPause)
		{
			onPause(isPause);
			OpenPauseInterface(isPause);
		}
		public void Restart()
		{

		}

		private bool isFirstTouchCreated { get; set; }
		private bool isControllAvailable { get; set; }

		private void Awake()
		{
			OnStartNewGame();
		}
		private void FixedUpdate()
		{
			HandleMouse();
		}
		private void HandleMouse()
		{
			if (!Input.GetMouseButton(0))
			{
				return;
			}

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
			onFirstTouch += OnCreateFirstTouch;
		}
		private void OnCreateFirstTouch()
		{
			isFirstTouchCreated = true;
			onFirstTouch -= OnCreateFirstTouch;
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
