﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public sealed class EasyEnemy : Enemy
	{
		protected override void InitProperties()
		{
			health = maxHealth = 10;
			touchDemage = 10;
			starsCount = 5;
			speed = 7;
			points = 57;
		}

		private float speed { get; set; }
	}
}
