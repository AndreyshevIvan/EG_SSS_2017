using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame.Enemies
{
	public sealed class RocketCopter : Enemy
	{
		protected override void InitProperties()
		{
			health = maxHealth = 3;
			touchDemage = 50;
			points = 100;
			healthBar = world.factory.GetBar(BarType.ENEMY_HEALTH);
			bonuses.Add(Pair<BonusType, int>.Create(BonusType.STAR, 5));

			isTimerWork = true;
			coldown = 2.5f;

			m_rocketData.target = world.ship.transform;
			m_rocketData.diactivateTime = 1.5f;
			m_rocketData.speed = 5;
			m_rocketData.demage = 20;
			m_rocketData.rotationSpeed = 1;
		}
		protected override void Shoot()
		{
			Rocket rocket = factory.GetAmmo<Rocket>(AmmoType.COPTER_ROCKET);
			rocket.SetTarget(m_rocketData, position);
		}

		private RocketData m_rocketData = new RocketData();
	}
}
