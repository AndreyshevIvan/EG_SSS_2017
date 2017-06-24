using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public abstract class Ammo : MonoBehaviour, IDemageBody
	{
		public float touchDemage { get; set; }
		public Vector3 position { set { transform.position = value; } }

		public abstract void Start();
		public virtual void OnDemageTaked() { }

		protected Rigidbody body { get; set; }
		protected BoundingBox mapBox { get; set; }

		protected virtual void OnAwake() { }
		protected void Awake()
		{
			body = GetComponent<Rigidbody>();
			mapBox = GameData.mapBox;
			OnAwake();
		}
		protected virtual void DestroyMe()
		{
			Destroy(gameObject);
		}
	}
}
