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

		protected Rigidbody m_body;

		protected virtual void OnAwake() { }

		private void Awake()
		{
			m_body = GetComponent<Rigidbody>();
			OnAwake();
		}
	}
}
