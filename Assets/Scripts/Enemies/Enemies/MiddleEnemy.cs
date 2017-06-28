using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public sealed class MiddleEnemy : Enemy
	{
		public RocketLauncher m_rocketGun;

		protected override void DoBeforeDeath()
		{
			m_rocketGun.isTimerWork = false;
		}
		protected override void NotSleepUpdate()
		{
			m_rocketGun.isTimerWork = !isSleep;
		}

		private float speed { get; set; }

		private void Start()
		{
			health = 50;
			touchDemage = 100;
			starsCount = 7;
			speed = 2;
			m_rocketGun.isTimerWork = true;
			m_rocketGun.speed = 10;
			m_rocketGun.factor = 1;
			m_rocketGun.Init(0, world, world.ship);
		}
	}
}
