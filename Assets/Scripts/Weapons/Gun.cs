using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public abstract class Gun : ShipProperty
	{
		public void Shoot()
		{
			if (!isTimerReady)
			{
				return;
			}

			OnShoot();
			ResetTimer();
		}

		protected abstract void OnShoot();
	}
}