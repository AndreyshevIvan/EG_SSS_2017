using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyGame.World;

namespace MyGame
{
	public class Modification : Bonus
	{
		protected override void OnRealize()
		{
			world.ship.mind.Modificate(0);
		}
	}
}
