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
		public bool isFull { get { return health == maxHealth; } }
		public int touchDemage { get; protected set; }

		public virtual void Heal(int healCount)
		{
			if (healCount == 0)
			{
				return;
			}

			health = health + healCount;
			health = Mathf.Clamp(health, 0, maxHealth);
			if (healthBar) healthBar.SetValue(healthPercents);
			OnHealEnd();
		}

		protected bool m_isEraseOnDeath = true;

		protected UIBar healthBar { get; set; }
		protected int maxHealth { get; set; }

		protected sealed override void OnTrigger(Collider other)
		{
			if (!IsCanBeDemaged())
			{
				return;
			}

			Body otherBody = Utils.GetOther<Body>(other);
			if (!otherBody) return;

			DoBeforeDemaged();
			Heal(-1 * otherBody.touchDemage);
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

		protected virtual void OnHealEnd() { }

		protected sealed override void UpdateBars()
		{
			if (healthBar) healthBar.position = position;
		}
	}
}
