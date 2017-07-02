using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public class Body : WorldObject
	{
		public int health { get; protected set; }
		public float healthPart { get { return (float)health / (float)maxHealth; } }
		public bool isLive { get { return isImmortal || health > 0; } }
		public bool isImmortal { get; protected set; }
		public int touchDemage { get; protected set; }
		public List<Bonus> bonuses { get; set; }

		public virtual void Heal(int healCount)
		{
			health = health + healCount;
			if (health > maxHealth)
			{
				health = maxHealth;
			}
		}

		protected UIBar healthBar { get; set; }
		protected int maxHealth { get; set; }
		protected bool isEraseOnDeath { get; set; }
		protected int addDemage
		{
			set { health = (health - value < 0) ? 0 : health - value; }
		}

		new protected void Awake()
		{
			bonuses = new List<Bonus>();
			isEraseOnDeath = true;
			base.Awake();
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

			if (!isLive && isEraseOnDeath)
			{
				world.CreateExplosion(explosion, position);
				world.Remove(this, true);
				return;
			}

			if (healthBar) healthBar.SetValue(healthPart);
		}
		protected virtual bool IsCanBeDemaged() { return !isImmortal; }
		protected virtual void DoBeforeDemaged() { }
		protected virtual void DoAfterDemaged() { }
		protected virtual void OnDemageTaked() { }
	}
}
