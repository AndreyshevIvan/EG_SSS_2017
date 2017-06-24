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

		private void Start()
		{
			health = 50;
			touchDemage = 100;
			starsCount = 8;

			m_rocketGun.Init(0, gameMap, gameMap.shipBody);
		}
		private void FixedUpdate()
		{
		}
	}
}
