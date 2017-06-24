using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public class HardEnemy : Enemy
	{
		private void Start()
		{
			health = 100;
			touchDemage = 100;
			starsCount = 12;
		}
		private void FixedUpdate()
		{
		}
	}
}
