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
		public float touchDemage { get; protected set; }
		public bool isLive { get { return isImmortal || health > 0; } }
		public bool isImmortal { get; protected set; }
		public int health { get; protected set; }
		public float healthPart { get { return health / maxHealth; } }
		public MapPhysics world { get; set; }
		public Vector3 position
		{
			get { return transform.position; }
			set { transform.position = value; }
		}
		public SplineController splineController { get; protected set; }

		public virtual void OnDemageTaked() { }
		public abstract void OnDeleteByWorld();

		protected float maxHealth { get; set; }
		protected Rigidbody physicsBody { get; set; }
		protected float addDemage { set { health -= (int)value; } }
		protected BoundingBox mapBox { get; set; }

		protected void Awake()
		{
			physicsBody = GetComponent<Rigidbody>();
			splineController = GetComponent<SplineController>();
			mapBox = GameData.mapBox;
			OnAwake();
		}
		protected virtual void OnAwake() { }
		protected virtual bool IsCanBeDemaged() { return !isImmortal; }
		protected virtual void DoBeforeDemaged() { }
		protected virtual void OnTrigger(Collider other) { }
		protected virtual void DoAfterDemaged() { }
		protected void OnTriggerEnter(Collider other)
		{
			OnTrigger(other);

			if (!IsCanBeDemaged())
			{
				return;
			}

			float demage = 0;
			if (Utils.GetDemage(ref demage, other))
			{
				DoBeforeDemaged();
				addDemage = demage;
				DoAfterDemaged();
			}
		}
	}
}
