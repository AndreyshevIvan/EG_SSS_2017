using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class Gun : MonoBehaviour
	{
		public Ammo m_simpleAmmo;

		public void Init(byte level)
		{
			m_simpleAmmo.Init(level);
		}

		private void FixedUpdate()
		{

		}
	}
}