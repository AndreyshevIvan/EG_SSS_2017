using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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

			healthBar = world.factory.GetBar(BarType.ENEMY_HEALTH);
		}
		protected override void Shoot()
		{
		}
	}
}
