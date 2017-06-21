using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public abstract class Ammo : ShipProperty, IDemageBody
	{
		public float demage { get { return m_demage; } }

		public abstract void Start();

		protected float m_demage;
		protected Rigidbody m_body;

		protected Vector3 velocity { set { m_body.velocity = value; } }

		protected abstract void OnAwake();
		protected abstract void OnUpdate();

		private void Awake()
		{
			m_body = GetComponent<Rigidbody>();
		}
		private void FixedUpdate()
		{
			OnUpdate();
		}
	}
}
