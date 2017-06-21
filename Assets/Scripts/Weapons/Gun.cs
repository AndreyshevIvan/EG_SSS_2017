using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public abstract class Gun : MonoBehaviour, IModifiable
	{
		public Ammo m_ammo;

		public byte level { get { return m_level; } }
		public byte maxLevel { get { return GameData.maxModLevel; } }
		public byte minLevel { get { return GameData.minModLevel; } }

		public void Shoot()
		{
			OnShoot();
		}
		public void SetLevel(byte level)
		{
			OnSetLevel(level);
			m_ammo.SetLevel(level);
		}
		public void Modify()
		{
			OnModify();
			m_ammo.Modify();
		}

		protected byte m_level;

		protected abstract void OnShoot();
		protected abstract void OnSetLevel(byte level);
		protected abstract void OnModify();
	}
}