using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame
{
	public class Hearth : Bonus
	{
		protected override void OnRealize()
		{
			world.ship.Heal(HEALTH_IN_HEARTH);
		}

		private const byte HEALTH_IN_HEARTH = 50;
	}
}
