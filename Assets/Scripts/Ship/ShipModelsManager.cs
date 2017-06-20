using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class ShipModelsManager : MonoBehaviour
	{
		public GameObject m_modelFirst;
		public GameObject m_modelSecond;
		public GameObject m_modelThird;

		public GameObject Get(ShipType type)
		{
			switch (type)
			{
				case ShipType.VOYAGER:
					return m_modelFirst;
				case ShipType.DESTENY:
					return m_modelSecond;
				case ShipType.SPLASH:
					return m_modelThird;
			}

			return m_modelFirst;
		}
	}
}
