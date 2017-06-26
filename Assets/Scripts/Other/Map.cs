using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluffyUnderware.Curvy;

namespace MyGame
{
	public sealed class Map : MonoBehaviour
	{
		public Transform m_ground;

		public List<SpawnTimer> m_spawnTimer;

		public EnemiesFactory enemies { get; set; }
		public ShipModel shipModel
		{
			get { return world.shipBody as ShipModel; }
			set { world.shipBody = value; }
		}
		public MapPhysics world { get; set; }

		public void Init(ShipModel model, ShipMind mind)
		{
			shipModel = model;
			shipMind = mind;
			model.transform.SetParent(transform);
		}

		private Vector3 shipPosition
		{
			get { return world.shipPosition; }
			set { world.shipPosition = value; }
		}
		private ShipMind shipMind
		{
			get { return world.shipMind; }
			set { world.shipMind = value; }
		}

		private float m_timer = 0;
		private float m_coldown = 1;

		private void Awake()
		{
		}
		private void Start()
		{
			world.ground = m_ground;
		}
		private void FixedUpdate()
		{
			if (Utils.IsTimerReady(m_timer, m_coldown))
			{
				Enemy enemy = null;
				int rand = UnityEngine.Random.Range(0, 2);

				if (rand == 1) enemy = enemies.middle;
				else enemy = enemies.easy;

				enemy.splineController.Spline = world.GetSpline();
				enemy.splineController.Speed = 5;
				world.AddEnemy(enemy);
				m_timer = 0;
				m_coldown = UnityEngine.Random.Range(1, 2);
			}

			Utils.UpdateTimer(ref m_timer, m_coldown);
		}
	}
}
