using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public abstract class Enemy : Body
	{
		public bool isSleep { get; set; }
		public byte starsCount { get; protected set; }

		public void DisableEnemy()
		{
			world.EraseEnemy(this);
		}

		protected override void OnAwake()
		{
			isSleep = true;
		}
		protected override void DoAfterDemaged()
		{
			if (!isLive)
			{
				DoBeforeDeath();
				world.EraseEnemyByKill(this);
			}
		}
		protected abstract void DoBeforeDeath();
		protected abstract void UpdateTactic();
		protected sealed override void OnTouchDeleter()
		{
			world.EraseEnemy(this);
		}
		protected void FixedUpdate()
		{
			if (!isSleep)
			{
				UpdateTactic();
			}
		}

		private byte stars { get; set; }
		private byte points { get; set; }
	}
}
