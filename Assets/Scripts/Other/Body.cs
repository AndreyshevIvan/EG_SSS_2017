using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MyGame.GameUtils;

namespace MyGame
{
	public class Body : WorldObject
	{
		public int health { get; protected set; }
		public float healthPart { get { return (float)health / (float)maxHealth; } }
		public int healthPercents { get { return (int)Mathf.Round(100 * healthPart); } }
		public bool isLive { get { return isImmortal || health > 0; } }
		public bool isImmortal { get; protected set; }
		public int touchDemage { get; protected set; }

		public virtual void Heal(int healCount)
		{
			health = health + healCount;
			if (health > maxHealth)
			{
				health = maxHealth;
			}
		}

		protected bool m_isEraseOnDeath = true;

		protected UIBar healthBar { get; set; }
		protected int maxHealth { get; set; }
		protected int addDemage
		{
			set { health = (health - value < 0) ? 0 : health - value; }
		}

		protected sealed override void OnTrigger(Collider other)
		{
			if (!IsCanBeDemaged())
			{
				return;
			}

			Body otherBody = Utils.GetOther<Body>(other);
			if (!otherBody) return;

			DoBeforeDemaged();
			addDemage = otherBody.touchDemage;
			otherBody.OnDemageTaked();
			DoAfterDemaged();

			if (!isLive && m_isEraseOnDeath)
			{
				OnDeath();
				world.Remove(this);
			}

			if (healthBar) healthBar.SetValue(healthPercents);
		}
		protected virtual bool IsCanBeDemaged() { return !isImmortal; }
		protected virtual void DoBeforeDemaged() { }
		protected virtual void DoAfterDemaged() { }
		protected virtual void OnDemageTaked() { }
		protected virtual void OnDeath() { }

		protected sealed override void UpdateBars()
		{
			if (healthBar) healthBar.position = position;
		}
	}
}
