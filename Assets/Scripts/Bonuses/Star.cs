using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public sealed class Star : Bonus
	{
		public enum StarPrice
		{
			X1 = 1,
			X3 = 3,
			X5 = 5,
		}

		protected override void OnRealize()
		{
			world.player.AddStars((int)price);
		}
		protected override void OnStart()
		{
			isMagnetic = true;
			price = StarPrice.X1;
		}

		private StarPrice price { get; set; }
		private float moveDelta { get; set; }
	}
}
