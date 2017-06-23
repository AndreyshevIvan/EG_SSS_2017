using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public abstract class Ammo : MonoBehaviour, IDemageBody
	{
		public float demage { get; set; }
		public Vector3 position { set { transform.position = value; } }

		public abstract void Start();
		public virtual void OnDemageTaked() { }

		protected Rigidbody m_body;
		protected BoundingBox m_mapBox;

		protected virtual void OnAwake() { }
		protected void Awake()
		{
			m_body = GetComponent<Rigidbody>();
			m_mapBox = GameData.mapBox;
			OnAwake();
		}
		protected virtual void DestroyMe()
		{
			Destroy(gameObject);
		}
	}
}
