using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.World
{
	public abstract class Ammo : Body
	{
		public int demage { set { touchDemage = value; } }

		public abstract void StartAmmo();
		public sealed override void OnExitFromWorld()
		{
			world.EraseAmmo(this);
		}

		internal sealed override void OnErase()
		{
		}
	}
}
