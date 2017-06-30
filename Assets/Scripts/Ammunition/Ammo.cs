using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public abstract class Ammo : Body
	{
		public int demage { set { touchDemage = value; } }

		public abstract void StartAmmo();

		protected sealed override void OnAwakeEnd()
		{
			isUseWorldSleep = false;
		}

		internal sealed override void OnExitFromWorld()
		{
			world.EraseAmmo(this);
		}
		internal sealed override void OnErase()
		{
		}
	}
}
