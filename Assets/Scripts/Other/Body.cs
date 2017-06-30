using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FluffyUnderware.Curvy.Controllers;

namespace MyGame
{
	public abstract class Body : MonoBehaviour
	{
		public ParticleSystem m_deathExplosion;
		public List<ParticleSystem> m_destroyParticles;

		public int health { get; protected set; }
		public float healthPart { get { return (float)health / (float)maxHealth; } }
		public bool isLive { get { return isImmortal || health > 0; } }
		public bool isImmortal { get; protected set; }
		public int touchDemage { get; protected set; }
		public Vector3 position
		{
			protected get { return transform.position; }
			set { transform.position = value; }
		}
		public SplineController roadController { get; protected set; }
		public ParticleSystem deathExplosion { get { return m_deathExplosion; } }
		public UIBar healthBar { get; protected set; }

		public void Init(MapPhysics newWorld)
		{
			world = newWorld;
			OnInitEnd();
		}
		public virtual void OnDemageTaked() { }
		public virtual void Heal(int healCount)
		{
			health = health + healCount;
			if (health > maxHealth)
			{
				health = maxHealth;
			}
		}

		protected IMapPhysics world { get; set; }
		protected RoadsFactory roads { get { return world.factories.roads; } }
		protected BoundingBox mapBox { get; set; }
		protected Rigidbody physicsBody { get; set; }
		protected int maxHealth { get; set; }
		protected int addDemage
		{
			set
			{
				health = (health - value < 0) ? 0 : health - value;
			}
		}
		protected bool isUseWorldSleep { get; set; }

		protected void Awake()
		{
			physicsBody = GetComponent<Rigidbody>();
			roadController = GetComponent<SplineController>();
			mapBox = GameData.mapBox;
			isUseWorldSleep = true;
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
				DoAfterDemaged();
			}
		}
		protected virtual bool IsCanBeDemaged() { return !isImmortal; }
		protected virtual void DoBeforeDemaged() { }
		protected virtual void OnTrigger(Collider other) { }
		protected virtual void DoAfterDemaged() { }

		protected void FixedUpdate()
		{
			if (healthBar) healthBar.position = position;

			if (!world.gameplay.isPlaying)
			{
				return;
			}

			NotSleepUpdate();

			if (isUseWorldSleep && world.gameplay.isMapSleep)
			{
				if (physicsBody) physicsBody.Sleep();
				return;
			}

			if (physicsBody) physicsBody.WakeUp();
			WakeupUpdate();
		}
		protected virtual void WakeupUpdate() { }
		protected virtual void NotSleepUpdate() { }

		protected void OnDestroy()
		{
			OnDestroyBody();
			if (healthBar != null) Destroy(healthBar.gameObject);
			m_destroyParticles.ForEach(particles => Destroy(particles));
		}
		protected virtual void OnDestroyBody() { }

		protected void ExitFromWorld()
		{
			Destroy(gameObject);
		}

		internal abstract void OnErase();
		internal abstract void OnExitFromWorld();
	}
}
