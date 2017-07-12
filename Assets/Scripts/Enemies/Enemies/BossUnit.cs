using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame.Enemies
{
	public class BossUnit : Enemy
	{
		protected override void InitProperties()
		{
			InitEvents();
		}
		protected override void Shoot()
		{
		}

		private void InitEvents()
		{

		}
	}
}
