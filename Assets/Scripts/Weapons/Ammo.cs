using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public abstract class Ammo : MonoBehaviour, IDemageBody, IModifiable
	{
		public float demage { get { return m_demage; } }
		public byte level { get { return m_level; } }
		public byte maxLevel { get { return GameData.maxModLevel; } }
		public byte minLevel { get { return GameData.minModLevel; } }

		public abstract void Start();
		public void SetLevel(byte newLevel)
		{
			m_level = Utils.Clamp(newLevel, minLevel, maxLevel);
			OnUpdateLevel();
		}
		public void Modify()
		{
			OnModify();
		}

		protected float m_demage;
		protected Rigidbody m_body;
		protected byte m_level;

		protected Vector3 velocity { set { m_body.velocity = value; } }

		protected abstract void OnAwake();
		protected abstract void OnModify();
		protected abstract void OnUpdateLevel();
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
