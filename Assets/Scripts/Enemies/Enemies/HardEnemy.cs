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

		private EnemyTurretGun m_turretGun;

		private void Start()
		{
			health = 100;
			touchDemage = 100;
			starsCount = 10;
			gameMap.MoveWithMap(this);

			m_turretGun = GetComponent<EnemyTurretGun>();
			m_turretGun.Init(0, gameMap);
		}
		private void FixedUpdate()
		{
			//Debug.Log("Enemy position: " + position);
		}
	}
}
