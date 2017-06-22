using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame
{
	public sealed class EasyEnemy : Enemy
	{
		private void Awake()
		{
			m_health = 100;
		}
	}
}
