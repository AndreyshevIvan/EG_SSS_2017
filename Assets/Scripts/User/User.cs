using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class User : MonoBehaviour
	{
		public string userName
		{
			get { return m_name; }
			set { m_name = value; }
		}

		private string m_name = "IvanAndreyshev";

		private uint m_experience = 300;
		private ushort m_level = 16;
		private ushort m_prestige = 0;

		List<string> m_ships;
		List<string> m_allowedShips;
		List<string> m_achievements;
	}
}
