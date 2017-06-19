using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class ShipsModelsManager : MonoBehaviour
	{
		public GameObject m_modelFirst;
		public GameObject m_modelSecond;
		public GameObject m_modelThird;

		public GameObject Get(ShipType type)
		{
			switch (type)
			{
				case ShipType.FIRST:
					return m_modelFirst;
				case ShipType.SECOND:
					return m_modelSecond;
				case ShipType.THIRD:
					return m_modelThird;
			}

			return m_modelFirst;
		}
	}
}
