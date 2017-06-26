using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public class HardEnemy : Enemy
	{
		protected override void DoBeforeDeath()
		{
			m_turretGun.isTimerWork = false;
		}
		protected override void UpdateTactic()
		{
			m_turretGun.isTimerWork = m_renderer.isVisible;
		}

		private EnemyTurretGun m_turretGun;
		private Renderer m_renderer;

		private void Start()
		{
			health = 100;
			touchDemage = 100;
			starsCount = 10;
			world.MoveWithMap(this);

			m_turretGun = GetComponent<EnemyTurretGun>();
			m_renderer = GetComponent<Renderer>();
			m_turretGun.Init(0, world);
			m_turretGun.speed = 6;
		}
	}
}
