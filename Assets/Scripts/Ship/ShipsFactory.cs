using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class ShipsFactory : MonoBehaviour
	{
		public ShipModel m_shipBody;
		public GameObject m_modelFirst;
		public GameObject m_modelSecond;
		public GameObject m_modelThird;

		public GameObject Spawn(ShipType type)
		{
			GameObject shipModelBody = Instantiate(m_shipBody).gameObject;
			GameObject newShip = null;

			switch (type)
			{
				case ShipType.VOYAGER:
					newShip = m_modelFirst;
					break;

				case ShipType.DESTENY:
					newShip = m_modelSecond;
					break;

				case ShipType.SPLASH:
					newShip = m_modelThird;
					break;
			}
			Instantiate(newShip, shipModelBody.transform);
			return shipModelBody;
		}
	}
}
