using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame.Enemies
{
	public class TargetTurret : Enemy
	{
		protected override void InitProperties()
		{
			health = maxHealth = 100;
			starsCount = 2;
			points = 157;
			isTimerWork = true;
			coldown = 1;
			healthBar = world.factory.GetBar(BarType.ENEMY_HEALTH);
		}
		protected override void Shoot()
		{
		}

		[SerializeField]
		private Bullet m_bullet;

		private const float SHOOT_DISTANCE = 20;
	}
}
