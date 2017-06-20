using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public abstract class Weapon : MonoBehaviour, IDemageBody
	{
		public float demage { get { return m_demage; } }

		public abstract void Init(byte level);
		public abstract void Fire(Transform spawn);
		public virtual void UpdateWeapon()
		{
			Utils.UpdateTimer(ref m_timer, m_coldown);
		}

		protected float m_coldown = 1;
		protected float m_demage;

		protected bool isReady { get { return Utils.IsColdownReady(m_timer, m_coldown); } }

		protected abstract void Update();

		private float m_timer = 0;

		private void FixedUpdate()
		{
			Update();
			Utils.UpdateTimer(ref m_timer, m_coldown);
		}
	}
}
