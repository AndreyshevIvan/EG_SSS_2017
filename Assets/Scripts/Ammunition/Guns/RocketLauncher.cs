using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public sealed class RocketLauncher : Gun
	{
		public Rocket m_rocket;

		protected override void DoAfterInit()
		{
			coldown = 1.2f;
		}
		protected override void Shoot()
		{
			Rocket newRocket = Instantiate(m_rocket);
			newRocket.Init(target, 70, 100, 0.3f);
			newRocket.position = transform.position;
			newRocket.Start();
			gameMap.AddEnemyBullet(newRocket.gameObject);
		}
	}
}