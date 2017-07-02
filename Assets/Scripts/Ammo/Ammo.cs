using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public abstract class Ammo : Body
	{
		public int demage { set { touchDemage = value; } }

		protected sealed override void OnInitEnd()
		{
			isImmortal = true;
		}
		protected sealed override void OnExitFromWorld()
		{
		}
	}
}
