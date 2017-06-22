using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame
{
	public interface IMapPhysics
	{
		ShipMind ship { get; }

		void AddPlayerBullet(Ammo bullet);
		void AddEnemyBullet(Ammo bullet);
	}
}
