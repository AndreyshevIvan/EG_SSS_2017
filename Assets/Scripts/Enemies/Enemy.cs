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
				healthBar.SetValue(health);
				toDestroy.Add(healthBar.gameObject);
			}
			if (roadController) roadController.OnEndReached.AddListener(T =>
			{
				if (!gameplay.isGameEnd) world.player.LossEnemy();
				Exit();
			});
		}
		protected abstract void InitProperties();

		protected sealed override void PlayingUpdate()
		{
			if (m_tactic != null) m_tactic();
			TryShoot();
		}
		protected abstract void Shoot();
		protected void ExtraReady()
		{
			m_timer = coldown;
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
			if (healthBar) healthBar.Fade(1, PlayerHealthBar.HP_BAR_FADE_DUR);
		}
		protected sealed override void OnDeath()
		{
			world.player.KillEnemy(type);

			if (Utils.IsHappen(HEALTH_PROBABLILITY))
			{
				bonuses.Add(BonusCount.Create(BonusType.HEALTH, 1));
			}
			if (world.player.isAllowedModify && Utils.IsHappen(AMMO_PROBABILITY))
			{
				bonuses.Add(BonusCount.Create(BonusType.AMMO_UP, 1));
			}
		}

		protected void AddTactic(EventDelegate tactic)
		{
			m_tactic += tactic;
		}
		protected void RemoveTactic(EventDelegate tactic)
		{
			if (m_tactic != null) m_tactic -= tactic;
		}

		private float m_timer = 0;
		private EventDelegate m_tactic;

		private float HEALTH_PROBABLILITY = 0.18f;
		private float AMMO_PROBABILITY = 0.2f;

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
