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
			health = maxHealth = 1;
			touchDemage = 10;
			points = 40;
			bonuses.Add(Pair<BonusType, int>.Create(BonusType.STAR, 3));
		}
		protected override void Shoot()
		{
			roadController.Position = 10;
		}
		protected override void SmartPlayingUpdate()
		{
			//Debug.Log(roadController.Position);
		}
	}
}
