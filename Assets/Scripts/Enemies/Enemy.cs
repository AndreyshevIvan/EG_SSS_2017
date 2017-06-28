using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public abstract class Enemy : Body
	{
		public byte starsCount { get; protected set; }
		public Bonus bonus { get; set; }

		public void DisableEnemy()
		{
			world.EraseEnemy(this);
		}
		public sealed override void OnDeleteByWorld()
		{
			world.EraseEnemy(this);
		}

		protected override void OnAwakeEnd()
		{
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

		private byte stars { get; set; }
		private byte points { get; set; }
	}
}
