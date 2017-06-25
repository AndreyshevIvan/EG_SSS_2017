using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public sealed class EasyEnemy : Enemy
	{
		protected override void DoBeforeDeath()
		{
		}

		private void Start()
		{
			health = 10;
			touchDemage = 100;
			starsCount = 0;
		}
		private void FixedUpdate()
		{
		}
	}
}
