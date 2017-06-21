using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public abstract class Gun : ShipProperty
	{
		public Ammo m_ammo;

		public void Shoot()
		{
			if (!Utils.IsColdownReady(m_timer, m_coldown))
			{
				return;
			}

			OnShoot();
			m_timer = 0;
		}
		public sealed override void Modify()
		{
			m_ammo.Modify();
			OnModificateGun();
		}

		protected float m_modifyColdownStep;
		protected float m_minColdown;
		protected sealed override void OnChangeLevel()
		{
			m_ammo.SetLevel(m_level);
			OnChangeGunLevel();
		}
		protected abstract void OnShoot();
		protected abstract void OnChangeGunLevel();
		protected abstract void OnModificateGun();

		private void FixedUpdate()
		{
			Utils.UpdateTimer(ref m_timer, m_coldown);
		}
	}
}