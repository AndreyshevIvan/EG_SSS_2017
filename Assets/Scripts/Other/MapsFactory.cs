using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public class MapsFactory : MonoBehaviour
	{
		public Map m_firstMap;

		public Map GetMap()
		{
			return Instantiate(m_firstMap);
		}
	}
}
