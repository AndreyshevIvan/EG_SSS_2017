using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public abstract class Body : MonoBehaviour, ILivingBody, IDemageBody
	{
		public float touchDemage { get; protected set; }
		public bool isLive { get { return isImmortal || health > 0; } }
		public bool isImmortal { get; protected set; }
		public int health { get; protected set; }
		public float healthPart { get { return health / maxHealth; } }
		public MapPhysics gameMap { get; set; }
		public Vector3 position
		{
			get { return transform.position; }
			set { transform.position = value; }
		}

		public virtual void OnDemageTaked() { }

		protected float maxHealth { get; set; }
		protected BoundingBox mapBox { get; set; }
		protected Rigidbody physicsBody { get; set; }
		protected float addDemage { set { health -= (int)value; } }

		protected void Awake()
		{
			physicsBody = GetComponent<Rigidbody>();
			mapBox = GameData.mapBox;
		}
		protected virtual bool IsCanBeDemaged() { return !isImmortal; }
		protected virtual void DoBeforeDemaged() { }
		protected virtual void OnTrigger(Collider other) { }
		protected virtual void DoAfterDemaged() { }
		protected void OnTriggerEnter(Collider other)
		{
			OnTrigger(other);

			if (IsCanBeDemaged())
			{
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
}
