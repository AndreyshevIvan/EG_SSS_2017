using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class ShipsFactory : MonoBehaviour
	{
		public ShipMind m_modelFirst;
		public ShipMind m_modelSecond;
		public ShipMind m_modelThird;

		public ShipModel m_body;

		public void Spawn(ShipType type, Map gameMap)
		{
			ShipModel body = Instantiate(m_body);
			ShipMind newMind = null;

			switch (type)
			{
				case ShipType.VOYAGER:
					newMind = m_modelFirst;
					break;

				case ShipType.DESTENY:
					newMind = m_modelSecond;
					break;

				case ShipType.SPLASH:
					newMind = m_modelThird;
					break;
			}

			ShipMind mind = Instantiate(newMind, body.transform);
			mind.Init(GameData.LoadShip(type), gameMap.world);
			gameMap.Init(body, mind);
		}
	}
}
