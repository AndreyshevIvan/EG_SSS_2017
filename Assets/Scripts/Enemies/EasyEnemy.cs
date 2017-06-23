using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public sealed class EasyEnemy : Enemy
	{
		private void Start()
		{
			m_health = 100;
			transform.position = new Vector3(0, 1, 30);
			m_physicsBody.velocity = new Vector3(0, 0, -5);
		}
		private void FixedUpdate()
		{

		}
	}
}
