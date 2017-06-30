using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MyGame.World;

namespace MyGame
{
	public sealed class MiddleEnemy : Enemy
	{
		public RocketLauncher m_rocketGun;

		protected override void InitProperties()
		{
			health = maxHealth = 50;
			touchDemage = 100;
			starsCount = 7;
			speed = 2;
			points = 1527;
		}
		protected override void InitGuns()
		{
			m_rocketGun.isTimerWork = true;
			m_rocketGun.speed = 10;
			m_rocketGun.factor = 1;
			m_rocketGun.Init(0, world, world.ship);

			guns.Add(m_rocketGun);
		}

		private float speed { get; set; }
	}
}
