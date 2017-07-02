using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public sealed class MiddleEnemy : Enemy
	{
		protected override void InitProperties()
		{
			health = maxHealth = 50;
			touchDemage = 100;
			starsCount = 7;
			points = 1527;
		}
		protected override void Shoot()
		{
		}
	}
}
