using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public abstract class Enemy : WorldObject
	{
		public Bonus bonus { get; set; }
		public int points { get; set; }
		public byte starsCount { get; protected set; }

		public sealed override void OnWorldChange()
		{
			guns.ForEach(gun => {
				if (gun) gun.isTimerWork = !gameplay.isPlaying;
			});
		}
		public sealed override void OnExitFromWorld()
		{
			world.EraseEnemy(this);
		}

		protected List<Gun> guns { get; set; }

		protected sealed override void OnAwakeEnd()
		{
			guns = new List<Gun>();
			if (roadController) roadController.OnEndReached.AddListener((T) => {
				Cleanup();
				world.EraseEnemy(this);
			});
		}
		protected sealed override void OnInitEnd()
		{
			InitProperties();

			if (healthBar)
			{
				healthBar.SetValue(healthPart);
				healthBar.isFadable = true;
			}
		}
		protected sealed override void DoAfterDemaged()
		{
			if (isLive) return;
			world.EraseEnemy(this);
		}
		protected abstract void InitProperties();
	}
}
