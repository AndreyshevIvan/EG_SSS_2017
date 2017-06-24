using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public abstract class Ammo : Body
	{
		protected float demage { set { touchDemage = value; } }

		public abstract void Start();

		protected virtual void OnAwake() { }
		new protected void Awake()
		{
			base.Awake();

			mapBox = GameData.mapBox;
			OnAwake();
		}
	}
}
