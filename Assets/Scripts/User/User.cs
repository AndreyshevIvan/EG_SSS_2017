using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	[System.Serializable]
	public class User
	{
		public string userName
		{
			get { return m_name; }
			set { m_name = value; }
		}
		public ShipType ship
		{
			get { return m_ship; }
		}

		private string m_name = "IvanAndreyshev";

		private uint m_experience = 300;
		private ushort m_level = 16;
		private ushort m_prestige = 0;

		private ShipType m_ship;
		private List<ShipType> m_ships;
		private List<ShipType> m_allowedShips;
		private List<CardType> m_achievements;
	}
}
