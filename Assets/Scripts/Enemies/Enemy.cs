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
		public UnitType type { get; set; }

		protected bool isTimerWork { get; set; }
		protected float coldown { get; set; }

		protected sealed override void OnInitEnd()
		{
			isTimerWork = false;
			exitAllowed = true;
			openAllowed = true;
			distmantleAllowed = true;

			InitProperties();

			if (healthBar)
			{
				healthBar.SetValue(healthPart);
				healthBar.isFadable = true;
			}
			if (roadController) roadController.OnEndReached.AddListener(T =>
			{
				world.player.LossEnemy();
				Exit();
			});
		}
		protected sealed override void PlayingUpdate()
		{
			TryShoot();
		}
		protected override void OnPlaying()
		{
			if (roadController) roadController.Play();
		}
		protected override void OnPause()
		{
			if (roadController) roadController.Pause();
		}
		protected sealed override void OnExitFromWorld()
		{
			base.OnExitFromWorld();
			world.player.LossEnemy();
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
