using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Hero
{
	public class ShipMind : MonoBehaviour
	{
		public ShipType type { get; internal set; }
		public float magnetic { get; protected set; }
		public float magnetDistance { get; protected set; }

		public void Modificate(byte modNumber)
		{
		}

		private IGameWorld world { get; set; }

		internal void Init(IGameWorld newWorld)
		{
			world = newWorld;

			magnetic = 1;
			magnetDistance = 5;
		}
	}
}
