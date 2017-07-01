using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FluffyUnderware.Curvy.Controllers;

namespace MyGame
{
	public abstract class WorldObject : MonoBehaviour, IWorldObject
	{
		public int health { get; protected set; }
		public float healthPart { get { return (float)health / (float)maxHealth; } }
		public bool isLive { get { return isImmortal || health > 0; } }
		public bool isImmortal { get; protected set; }
		public int touchDemage { get; protected set; }
		public Vector3 position
		{
			get { return transform.position; }
			set { transform.position = value; }
		}
		public SplineController roadController { get; protected set; }
		public ParticleSystem explosion { get; set; }
		public List<Bonus> bonuses { get; set; }

		public void Init(IGameWorld gameWorld)
		{
			gameplay = gameWorld as IGameplay;
			world = gameWorld;
			OnInitEnd();
		}
		public virtual void Heal(int healCount)
		{
			health = health + healCount;
			if (health > maxHealth)
			{
				health = maxHealth;
			}
		}

		public virtual void OnWorldChange() { }
		public virtual void OnDemageTaked() { }
		public virtual void OnErase() { }
		public virtual void OnExitFromWorld() { }
		public void Cleanup()
		{
			Utils.DestroyAll(particles);
			Utils.DestroyAll(bonuses);
		}

		protected IGameplay gameplay { get; set; }
		protected IGameWorld world { get; set; }
		protected HealthBar healthBar { get; set; }
		protected BoundingBox mapBox { get; set; }
		protected Rigidbody physicsBody { get; set; }
		protected int maxHealth { get; set; }
		protected int addDemage
		{
			set { health = (health - value < 0) ? 0 : health - value; }
		}
		protected List<ParticleSystem> particles { get; set; }

		protected void Awake()
		{
			physicsBody = GetComponent<Rigidbody>();
			roadController = GetComponent<SplineController>();
			particles = Utils.GetAllComponents<ParticleSystem>(this);
			mapBox = GameData.mapBox;
			OnAwakeEnd();
		}
		protected virtual void OnAwakeEnd() { }
		protected virtual void OnInitEnd() { }

		protected void OnTriggerEnter(Collider other)
		{
			OnTrigger(other);

			if (!IsCanBeDemaged())
			{
				return;
			}

			int demage = 0;
			if (Utils.GetDemage(ref demage, other))
			{
				DoBeforeDemaged();
				addDemage = demage;
				if (healthBar) healthBar.SetValue(healthPart);
				DoAfterDemaged();
			}
		}
		protected virtual bool IsCanBeDemaged() { return !isImmortal; }
		protected virtual void DoBeforeDemaged() { }
		protected virtual void OnTrigger(Collider other) { }
		protected virtual void DoAfterDemaged() { }

		protected void FixedUpdate()
		{
			if (gameplay.isPaused)
			{
				return;
			}

			PermanentUpdate();

			if (!gameplay.isPlaying)
			{
				return;
			}

			PlayingUpdate();

			if (gameplay.isMapStay)
			{
				StayingUpdate();
			}

			MoveingUpdate();
		}
		protected virtual void PermanentUpdate() { }
		protected virtual void PlayingUpdate() { }
		protected virtual void StayingUpdate() { }
		protected virtual void MoveingUpdate() { }
	}

	public interface IWorldObject : IGameplayObject
	{
		ParticleSystem explosion { get; }
		List<Bonus> bonuses { get; }

		void OnErase();
		void OnExitFromWorld();
	}

	public interface IGameplayObject
	{
		void Init(IGameWorld gameWorld);
		void OnWorldChange();
	}
}
