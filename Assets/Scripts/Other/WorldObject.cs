using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FluffyUnderware.Curvy.Controllers;

namespace MyGame
{
	public abstract class WorldObject : MonoBehaviour, IWorldEntity
	{
		public Vector3 position
		{
			get { return transform.position; }
			set { transform.position = value; }
		}
		public SplineController roadController { get; protected set; }
		public ParticleSystem explosion { get { return m_explosion; } }
		public int erasePoints { get; set; }
		public bool isWorldSet { get { return world != null; } }
		public bool isExitAllowed { get; protected set; }

		public void Init(IGameWorld gameWorld)
		{
			if (world != null && world != gameWorld)
			{
				return;
			}

			world = gameWorld;
			gameplay = gameWorld as IGameplay;
			OnInitEnd();
		}
		public void OnGameplayChange()
		{
			if (!gameplay.isPlaying)
			{
				currentEvent = () => { };
				return;
			}

			if (gameplay.isMapStay)
			{
				currentEvent = onMapStay;
				return;
			}

			currentEvent = onMapMove;
		}

		protected IGameplay gameplay { get; private set; }
		protected IGameWorld world { get; set; }
		protected BoundingBox mapBox { get; set; }
		protected Rigidbody physicsBody { get; set; }
		protected List<ParticleSystem> particles { get; set; }

		protected void Awake()
		{
			physicsBody = GetComponent<Rigidbody>();
			roadController = GetComponent<SplineController>();
			particles = Utils.GetAllComponents<ParticleSystem>(this);
			mapBox = GameData.mapBox;
			isExitAllowed = true;

			onPlaying += PlayingUpdate;
			onPlaying += UpdateBars;

			onMapStay += onPlaying;
			onMapStay += StayingUpdate;

			onMapMove += onPlaying;
			onMapMove += MoveingUpdate;

			OnAwakeEnd();
		}
		protected virtual void OnAwakeEnd() { }
		protected virtual void OnInitEnd() { }

		protected void OnTriggerEnter(Collider other)
		{
			OnTrigger(other);
		}
		protected virtual void OnTrigger(Collider other) { }

		protected void OnTriggerExit(Collider other)
		{
			if (other.gameObject.layer == GameWorld.WORLD_BOX_LAYER &&
				isExitAllowed)
			{
				world.Remove(this, false);
			}
		}

		protected void FixedUpdate()
		{
			if (world == null)
			{
				return;
			}

			if (currentEvent != null) currentEvent();
		}
		protected virtual void PlayingUpdate() { }
		protected virtual void StayingUpdate() { }
		protected virtual void MoveingUpdate() { }
		protected virtual void UpdateBars() { }

		protected virtual void OnExitFromWorld() { }
		protected void Cleanup()
		{
			Utils.DestroyAll(particles);
			erasePoints = 0;
		}

		[SerializeField]
		private ParticleSystem m_explosion;
		private bool m_isStartExit = false;

		private EventDelegate currentEvent { get; set; }
		private EventDelegate onPlaying { get; set; }
		private EventDelegate onMapStay { get; set; }
		private EventDelegate onMapMove { get; set; }
}

	public interface IWorldEntity
	{
		void Init(IGameWorld gameWorld);
		void OnGameplayChange();

		bool isWorldSet { get; }
		bool isExitAllowed { get; }
	}
}
