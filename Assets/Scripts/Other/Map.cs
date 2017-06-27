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

		public List<FlySpawn> m_flySpawns;
		public List<GroundSpawn> m_groundSpawns;

		public bool isPlay { get; set; }
		public EnemiesFactory enemies { get; set; }
		public ShipModel shipModel
		{
			get { return world.shipBody as ShipModel; }
			set { world.shipBody = value; }
		}
		public MapPhysics world { get; set; }
		public RoadsFactory roads { get; set; }

		public void Init(ShipModel model)
		{
			shipModel = model;
			shipMind = model.mind;
			model.transform.SetParent(transform);
		}
		public void Play()
		{
		}
		public void Restart()
		{
		}
		public void Pause(bool isPause)
		{
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
			SpawnGroundUnits();
		}
		private void FixedUpdate()
		{
			if (!isPlay)
			{
				return;
			}

			FlySpawn spawn = m_flySpawns.Find(x => x.offset <= world.offset);
			if (spawn != null)
			{
				SpawnFlyInits(spawn);
				m_flySpawns.Remove(spawn);
			}
		}
		private void SpawnFlyInits(FlySpawn spawn)
		{
			CurvySpline road = roads.Get(spawn.road);
			for (int i = 0; i < spawn.count; i++)
			{
				Enemy enemy = Instantiate(spawn.enemy);
				world.AddEnemy(enemy);
				enemy.splineController.Spline = road;
				float spawnPosition = MapPhysics.SPAWN_OFFSET * i / road.Length;
				enemy.splineController.InitialPosition = spawnPosition;
				enemy.splineController.Speed = spawn.speed;
			}
		}
		private void SpawnGroundUnits()
		{
			foreach (GroundSpawn spawn in m_groundSpawns)
			{
				Enemy enemy = Instantiate(spawn.enemy);
				world.AddEnemy(enemy);
				enemy.position = spawn.position;
			}
		}
	}
}
