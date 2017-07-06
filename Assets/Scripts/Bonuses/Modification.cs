using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame
{
	public class Modification : Bonus
	{
		protected override void OnRealize()
		{
			world.player.Modify();
		}
	}
}
