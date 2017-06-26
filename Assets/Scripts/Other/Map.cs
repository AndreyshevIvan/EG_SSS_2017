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

		public List<Spawn> m_spawns;

		public bool isPlay { get; set; }
		public EnemiesFactory enemies { get; set; }
		public ShipModel shipModel
		{
			get { return world.shipBody as ShipModel; }
			set { world.shipBody = value; }
		}
		public MapPhysics world { get; set; }
		public RoadsFactory roads { get; set; }

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

		private void Awake()
		{
		}
		private void Start()
		{
			world.ground = m_ground;
		}
		private void FixedUpdate()
		{
			if (!isPlay)
			{
				return;
			}

			Spawn spawn = m_spawns.Find(x => x.offset <= world.offset);
			if (spawn != null)
			{
				DoSpawn(spawn);
				m_spawns.Remove(spawn);
			}
		}
		private void DoSpawn(Spawn spawn)
		{
			CurvySpline road = roads.Get(spawn.road);
			for (int i = 0; i < spawn.count; i++)
			{
				Enemy enemy = Instantiate(spawn.enemy);
				enemy.splineController.Spline = road;
				float spawnPosition = MapPhysics.SPAWN_OFFSET * i / road.Length;
				enemy.splineController.InitialPosition = spawnPosition;
				enemy.splineController.Speed = spawn.speed;
				world.AddEnemy(enemy);
			}
		}
	}
}
