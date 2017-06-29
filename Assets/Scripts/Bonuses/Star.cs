using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MyGame.World;

namespace MyGame
{
	public sealed class Star : Bonus
	{
		protected override void OnRealize()
		{
		}
		protected override void OnAwakeEnd()
		{
			isMagnetic = true;
			price = 1;
		}

		private byte price { get; set; }
		private float moveDelta { get; set; }
	}
}
