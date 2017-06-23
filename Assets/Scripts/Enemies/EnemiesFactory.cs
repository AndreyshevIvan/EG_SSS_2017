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
		public MiddleEnemy m_middleEnemy;
		public HardEnemy m_hardEnemy;

		public EasyEnemy easy { get { return Instantiate(m_easyEnemy); } }
		public MiddleEnemy middle { get { return Instantiate(m_middleEnemy); } }
		public HardEnemy hard { get { return Instantiate(m_hardEnemy); } }
	}
}
