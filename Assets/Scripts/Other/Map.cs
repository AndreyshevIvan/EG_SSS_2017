using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public sealed class Map : MonoBehaviour
	{
		public MapPhysics m_mapPhysics;
		public Transform m_ground;

		public EnemiesFactory enemies { get; set; }
		public ShipModel shipModel
		{
			get { return world.shipBody as ShipModel; }
			set { world.shipBody = value; }
		}
		public MapPhysics world { get; private set; }

		public void Init(ShipModel model, ShipMind mind)
		{
			shipModel = model;
			shipMind = mind;
			model.transform.SetParent(transform);
			shipPosition = new Vector3(0, MapPhysics.FLY_HEIGHT, 0);
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

		private void Awake()
		{
			world = Instantiate(m_mapPhysics, transform);
			world.ground = m_ground;
			world.transform.position = Vector3.zero;
		}
		private void Start()
		{
			Enemy enemy = enemies.easy;
			enemy.transform.position = new Vector3(-5, MapPhysics.FLY_HEIGHT, 4);
			world.AddEnemy(enemy);
			enemy = enemies.middle;
			enemy.transform.position = new Vector3(0, MapPhysics.FLY_HEIGHT, 0);
			world.AddEnemy(enemy);
			enemy = enemies.hard;
			enemy.transform.position = new Vector3(5, MapPhysics.FLY_HEIGHT, 4);
			world.AddEnemy(enemy);
		}
		private void FixedUpdate()
		{
		}
	}
}
