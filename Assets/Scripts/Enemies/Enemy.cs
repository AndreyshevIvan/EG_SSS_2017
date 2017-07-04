using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame.Enemies
{
	public abstract class Enemy : Body
	{
		public byte starsCount { get; protected set; }
		protected bool isTimerWork { get; set; }
		protected float coldown { get; set; }

		protected sealed override void OnInitEnd()
		{
			InitProperties();

			if (healthBar)
			{
				healthBar.SetValue(healthPart);
				healthBar.isFadable = true;
			}

			if (roadController) roadController.OnEndReached.AddListener((T) =>
			{
				world.Remove(this, false);
			});
		}
		protected sealed override void PlayingUpdate()
		{
			TryShoot();
		}
		protected abstract void InitProperties();
		protected abstract void Shoot();

		private float m_timer = 0;

		private void TryShoot()
		{
			if (isTimerWork && Utils.UpdateTimer(ref m_timer, coldown))
			{
				Shoot();
			}
		}
	}
}
