using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public abstract class Ammo : MonoBehaviour, IDemageBody
	{
		public float demage { get { return m_demage; } }

		public abstract void Init(byte level);
		public void DoShoot(Transform spawn)
		{
			if (!isReady || !IsWeaponReady())
			{
				return;
			}

			Fire(spawn);
			m_timer = 0;
		}
		public virtual void UpdateWeapon()
		{
			Utils.UpdateTimer(ref m_timer, m_coldown);
		}

		protected float m_coldown = 1;
		protected float m_demage;

		protected bool isReady { get { return Utils.IsColdownReady(m_timer, m_coldown); } }

		protected abstract void Fire(Transform spawn);
		protected abstract void Update();
		protected abstract bool IsWeaponReady();

		private float m_timer = 0;

		private void FixedUpdate()
		{
			Update();
			Utils.UpdateTimer(ref m_timer, m_coldown);
		}
	}
}
