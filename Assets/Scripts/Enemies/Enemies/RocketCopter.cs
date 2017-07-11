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
			health = maxHealth = 32 + (int)(world.time / 10);
			touchDemage = 50;
			points = 100;
			healthBar = world.factory.GetBar(BarType.ENEMY_HEALTH);
			bonuses.Add(Pair<BonusType, int>.Create(BonusType.STAR, 5));
		}
		protected override void Shoot()
		{
		}
	}
}
