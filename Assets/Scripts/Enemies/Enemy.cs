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

		protected override void DoAfterDemaged()
		{
			if (!isLive)
			{
				DoBeforeDeath();
				gameMap.EraseEnemy(this);
			}
		}
		protected abstract void DoBeforeDeath();

		private byte stars { get; set; }
		private byte points { get; set; }
	}
}
