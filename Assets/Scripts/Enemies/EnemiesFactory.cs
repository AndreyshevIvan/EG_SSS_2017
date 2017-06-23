using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public class EnemiesFactory : MonoBehaviour
	{
		public EasyEnemy m_easyEnemy;

		public EasyEnemy GetEasy()
		{
			return Instantiate(m_easyEnemy);
		}
	}
}
