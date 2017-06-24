using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class ShipsFactory : MonoBehaviour
	{
		public GameObject m_modelFirst;
		public GameObject m_modelSecond;
		public GameObject m_modelThird;
		public Transform m_modelsParent;

		public GameObject Spawn(ShipType type)
		{
			GameObject ship = null;

			switch (type)
			{
				case ShipType.VOYAGER:
					ship = m_modelFirst;
					break;

				case ShipType.DESTENY:
					ship = m_modelSecond;
					break;

				case ShipType.SPLASH:
					ship = m_modelThird;
					break;
			}

			return Instantiate(ship, m_modelsParent);
		}
	}
}
