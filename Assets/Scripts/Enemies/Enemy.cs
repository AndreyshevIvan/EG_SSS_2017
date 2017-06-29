using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame.World
{
	public abstract class Enemy : Body
	{
		public Bonus bonus { get; set; }
		public int points { get; set; }
		public byte starsCount { get; protected set; }

		public sealed override void OnExitFromWorld()
		{
			world.EraseEnemy(this);
		}

		protected void Start()
		{
			healthBar = world.factories.bars.enemyHealth;
			healthBar.SetValue(healthPart);
		}
		protected sealed override void DoAfterDemaged()
		{
			if (!isLive)
			{
				world.EraseEnemyByKill(this);
			}

			healthBar.SetValue(healthPart);
		}
		protected abstract void DisableGuns();

		internal sealed override void OnErase()
		{
			DisableGuns();
		}
	}
}
