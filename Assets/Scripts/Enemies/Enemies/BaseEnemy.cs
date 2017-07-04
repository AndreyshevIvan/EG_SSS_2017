using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame.Enemies
{
	public sealed class BaseEnemy : Enemy
	{
		protected override void InitProperties()
		{
			health = maxHealth = 10;
			touchDemage = 10;
			starsCount = 5;
			points = 57;
		}
		protected override void Shoot()
		{
		}
	}
}
