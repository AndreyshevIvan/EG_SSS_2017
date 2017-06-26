using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public abstract class Ammo : Body
	{
		protected float demage { set { touchDemage = value; } }

		public abstract void Start();

		protected sealed override void OnTouchDeleter()
		{
			world.EraseAmmo(this);
		}
		new protected void Awake()
		{
			base.Awake();
			OnAwake();
		}
	}
}
