using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public abstract class Gun : Property
	{
		public int demage { get; set; }

		protected abstract void Shoot();

		new protected void FixedUpdate()
		{
			base.FixedUpdate();
			DoShoot();
		}

		private void DoShoot()
		{
			if (!isTimerReady)
			{
				return;
			}

			Shoot();
			ResetTimer();
		}
	}
}