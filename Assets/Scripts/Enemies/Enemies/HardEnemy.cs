using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MyGame.World;

namespace MyGame
{
	public class HardEnemy : Enemy
	{
		protected override void NotSleepUpdate()
		{
			m_turretGun.isTimerWork = m_renderer.isVisible && !isSleep;
		}
		protected override void OnInitEnd()
		{
			health = maxHealth = 100;
			touchDemage = 100;
			starsCount = 2;
			world.SubscribeToMove(this);

			m_turretGun = GetComponent<EnemyTurretGun>();
			m_renderer = GetComponent<Renderer>();
			m_turretGun.Init(0, world);
			m_turretGun.speed = 6;
			points = 157;
		}
		protected override void DisableGuns()
		{
			m_turretGun.isTimerWork = false;
		}

		private EnemyTurretGun m_turretGun;
		private Renderer m_renderer;
	}
}
