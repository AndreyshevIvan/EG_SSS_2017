using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class EnemiesManager : MonoBehaviour
	{
		public MapPhysics m_mapPhysics;

		public Vector3 demoPositionFirst;
		public Vector3 demoPositionSecond;
		public Vector3 demoPositionThird;

		private EnemiesFactory m_factory;

		private void Awake()
		{
			m_factory = GetComponent<EnemiesFactory>();

			Enemy easy = m_factory.easy;
			Enemy middle = m_factory.middle;
			Enemy hard = m_factory.hard;

			easy.transform.position = demoPositionFirst;
			middle.transform.position = demoPositionSecond;
			hard.transform.position = demoPositionThird;

			m_mapPhysics.AddEnemy(easy.gameObject);
			m_mapPhysics.AddEnemy(middle.gameObject);
			m_mapPhysics.AddEnemy(hard.gameObject);
		}
		private void FixedUpdate()
		{
		}
	}
}