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
		protected override void InitProperties()
		{
			health = maxHealth = 100;
			touchDemage = 100;
			starsCount = 2;
			points = 157;

			//healthBar = world.factories.bars.enemyHealth;

			world.SubscribeToMove(this);

			m_turretGun = GetComponent<EnemyTurretGun>();
			m_turretGun.Init(world);
			m_turretGun.speed = 6;

			guns.Add(m_turretGun);
		}

		private EnemyTurretGun m_turretGun;
	}
}
