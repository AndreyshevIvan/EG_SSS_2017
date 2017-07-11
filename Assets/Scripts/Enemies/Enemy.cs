using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MyGame.GameUtils;

namespace MyGame.Enemies
{
	using BonusCount = Pair<BonusType, int>;

	public abstract class Enemy : Body
	{
		public UnitType type { get; set; }

		protected bool isTimerWork { get; set; }
		protected float coldown { get; set; }

		protected sealed override void OnInitEnd()
		{
			isTimerWork = false;
			exitAllowed = false;
			openAllowed = true;
			distmantleAllowed = true;

			InitProperties();

			if (healthBar)
			{
				healthBar.SetValue(healthPercents);
				toDestroy.Add(healthBar.gameObject);
			}
			if (roadController) roadController.OnEndReached.AddListener(T =>
			{
				if (!gameplay.isGameEnd) world.player.LossEnemy();
				Exit();
			});
		}
		protected sealed override void PlayingUpdate()
		{
			UpdateTaktic();
			TryShoot();
		}
		protected virtual void UpdateTaktic() { }
		protected sealed override void OnDeath()
		{
			world.player.KillEnemy(type);

			if (Utils.IsHappen(0.1f))
			{
				bonuses.Add(BonusCount.Create(BonusType.HEALTH, 1));
			}
			else if (world.player.isAllowedModify && Utils.IsHappen(0.4f))
			{
				bonuses.Add(BonusCount.Create(BonusType.AMMO_UP, 1));
			}
		}
		protected sealed override void OnPlaying()
		{
			if (roadController) roadController.Play();
		}
		protected sealed override void OnPause()
		{
			if (roadController) roadController.Pause();
		}
		protected sealed override void DoAfterDemaged()
		{
			if (healthBar) healthBar.Fade(1, HealthBar.HP_BAR_FADE_DUR);
		}
		protected abstract void InitProperties();
		protected abstract void Shoot();

		private float m_timer = 0;

		private void TryShoot()
		{
			if (isTimerWork && Utils.UpdateTimer(ref m_timer, coldown))
			{
				Shoot();
				m_timer = 0;
			}
		}
	}
}
