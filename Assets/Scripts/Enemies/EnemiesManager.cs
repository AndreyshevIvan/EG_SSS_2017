using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class EnemiesManager : MonoBehaviour
	{
		private EnemiesFactory m_factory;
		private float m_timer = 0;

		private void Awake()
		{
			m_factory = GetComponent<EnemiesFactory>();
		}
		private void FixedUpdate()
		{
			m_timer += Time.deltaTime;

			if (m_timer > 5)
			{
				m_factory.GetEasy();
				m_timer = 0;
			}
		}
	}
}