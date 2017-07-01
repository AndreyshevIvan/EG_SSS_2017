﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public sealed class RocketLauncher : Gun
	{
		public Rocket m_rocket;
		public float speed { get; set; }
		public float factor { get; set; }

		protected override void DoAfterInit()
		{
			coldown = 1.5f;
		}
		protected override void Shoot()
		{
			Rocket newRocket = Instantiate(m_rocket);
			newRocket.position = transform.position;
			//world.AddAmmo(newRocket);
		}
	}
}