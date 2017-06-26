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
		protected override void UpdateTactic()
		{
			position = position - Vector3.forward * speed * Time.deltaTime;
		}

		private float speed { get; set; }

		private void Start()
		{
			health = 10;
			touchDemage = 100;
			starsCount = 5;
			speed = 7;
		}
	}
}
