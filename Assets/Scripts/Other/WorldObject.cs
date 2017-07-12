using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FluffyUnderware.Curvy.Controllers;
using MyGame.GameUtils;
using MyGame.Factory;
using FluffyUnderware.Curvy;

namespace MyGame
{
	using System.Collections;
	using BonusCount = Pair<BonusType, int>;

	public abstract class WorldObject : MonoBehaviour, IWorldEntity
	{
		public Vector3 position
		{
			get { return transform.position; }
			set { transform.position = value; }
		}
		public SplineController roadController { get; protected set; }
		public ParticleSystem explosion { get { return m_explosion; } }

		public bool exitAllowed { get; protected set; }
		public bool openAllowed { get; protected set; }
		public bool distmantleAllowed { get; protected set; }
		public bool inGameBox { get { return m_box.Contain(position); } }

		public bool isWorldSet { get { return world != null; } }
		public int points { get; protected set; }
		public List<Pair<BonusType, int>> bonuses { get; protected set; }

		public void Init(IGameWorld newWorld)
		{
			if (world != null && world != newWorld)
			{
				return;
			}

			world = newWorld;
			gameplay = (IGameplay)newWorld;
			m_box = world.box;
			OnInitEnd();
			GameplayChange();
		}
		public void GameplayChange()
		{
			OnChangeGameplay();
			currentEvent = () => { };

			if (gameplay.isPaused)
			{
				OnPauseEvents();
			}
			else if (gameplay.isPlaying)
			{
				OnPlayingEvents();
				currentEvent = onPlaying;
				return;
			}
			else if (gameplay.isGameEnd)
			{
				OnEndGameplayEvents();
				currentEvent = onGameEnd;
				return;
			}
		}

		public void AddToRoad(CurvySpline road, float position)
		{
			StartCoroutine(SetSpline(road, position));
		}
		public void MoveToGround()
		{
			transform.SetParent(world.ground);
		}
		public void MoveToSky()
		{
			transform.SetParent(world.sky);
		}
		public void ExitFromWorld()
		{
			OnExitFromWorld();
		}

		protected BoundingBox m_box;

		protected IGameplay gameplay { get; private set; }
		protected IGameWorld world { get; private set; }
		protected IFactory factory { get { return world.factory; } }
		protected Rigidbody physicsBody { get; private set; }
		protected List<ParticleSystem> particles { get; set; }
		protected List<GameObject> toDestroy { get; private set; }

		protected void Awake()
		{
			physicsBody = GetComponent<Rigidbody>();
			roadController = GetComponent<SplineController>();
			particles = Utils.GetAllComponents<ParticleSystem>(this);
			bonuses = new List<BonusCount>();
			toDestroy = new List<GameObject>();

			exitAllowed = true;
			openAllowed = false;
			distmantleAllowed = false;

			onPlaying += PlayingUpdate;
			onPlaying += UpdateBars;
			onPlaying += SmartPlayingUpdate;

			onGameEnd += AfterMatchUpdate;
			onGameEnd += UpdateBars;
			onGameEnd += SmartPlayingUpdate;

			OnAwakeEnd();
		}
		protected virtual void OnAwakeEnd() { }
		protected virtual void OnInitEnd() { }

		protected virtual void OnChangeGameplay() { }

		private void OnPlayingEvents()
		{
			OnPlaying();
		}
		protected virtual void OnPlaying() { }

		private void OnEndGameplayEvents()
		{
			OnEndGameplay();
		}
		protected virtual void OnEndGameplay() { }

		private void OnPauseEvents()
		{
			OnPause();
		}
		protected virtual void OnPause() { }

		protected void OnTriggerEnter(Collider other)
		{
			OnTrigger(other);
		}
		protected virtual void OnTrigger(Collider other) { }

		protected void FixedUpdate()
		{
			if (world == null)
			{
				return;
			}

			if (m_extraUpdate != null) m_extraUpdate();
			if (currentEvent != null) currentEvent();
		}
		protected virtual void PlayingUpdate() { }
		protected virtual void SmartPlayingUpdate() { }
		protected virtual void AfterMatchUpdate() { }
		protected virtual void UpdateBars() { }

		protected void AddExtraListener(EventDelegate listener)
		{
			m_extraUpdate += listener;
		}
		protected void EraseExtraListener(EventDelegate listener)
		{
			if (m_extraUpdate != null) m_extraUpdate -= listener;
		}

		protected virtual void OnExitFromWorld() { }
		protected void UpdatePositionOnField()
		{
			position = new Vector3(
				Mathf.Clamp(position.x, m_box.xMin, m_box.xMax),
				GameWorld.FLY_HEIGHT,
				position.z//Mathf.Clamp(position.z, m_box.zMin, m_box.zMax)
			);
		}

		protected void Exit()
		{
			openAllowed = false;
			distmantleAllowed = false;
			world.Remove(this);
		}
		protected void Cleanup()
		{
			Utils.DestroyAll(particles);
			points = 0;
		}
		protected void OnDestroy()
		{
			toDestroy.ForEach(element => { if (element) Destroy(element); });
		}

		[SerializeField]
		private ParticleSystem m_explosion;
		private bool m_isStartExit = false;
		private EventDelegate m_extraUpdate;

		private EventDelegate currentEvent { get; set; }
		private EventDelegate onPlaying { get; set; }
		private EventDelegate onGameEnd { get; set; }

		private IEnumerator SetSpline(CurvySpline spline, float position)
		{
			roadController.Spline = spline;
			yield return new WaitForFixedUpdate();
			roadController.Position = position;
		}
	}

	public interface IWorldEntity
	{
		void Init(IGameWorld gameWorld);
		void GameplayChange();
	}
}
