using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public abstract class Body : MonoBehaviour, ILivingBody, IDemageBody
	{
		public float demage { get { return m_touchDemage; } }
		public bool isLive { get { return isImmortal || m_health > 0; } }
		public bool isImmortal { get; protected set; }
		public int health { get { return m_health; } }
		public float healthPart { get { return m_health / m_maxhealth; } }

		protected int m_health;
		protected float m_maxhealth;
		protected float m_touchDemage;

		protected float addDemage { set { m_health -= (int)value; } }

		protected virtual bool IsCanBeDemaged() { return !isImmortal; }
		protected virtual void DoBeforeDemaged() { }
		protected virtual void DoAfterDemaged() { }
		protected virtual void OnTrigger(Collider other) { }

		private void OnTriggerEnter(Collider other)
		{
			OnTrigger(other);

			if (IsCanBeDemaged())
			{
				DoBeforeDemaged();
				addDemage = Utils.GetDemage(other);
				DoAfterDemaged();
			}
		}
	}
}
