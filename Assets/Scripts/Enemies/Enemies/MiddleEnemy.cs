using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public sealed class MiddleEnemy : Enemy
	{
		private void Start()
		{
			health = 50;
			touchDemage = 100;
			starsCount = 8;
		}
		private void FixedUpdate()
		{
		}
	}
}
