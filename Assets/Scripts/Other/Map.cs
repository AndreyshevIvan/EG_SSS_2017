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
		public Transform m_sky;

		public List<FlySpawn> m_flySpawns;
		public List<GroundSpawn> m_groundSpawns;

		public Ship ship
		{
			get { return world.shipBody as Ship; }
			set { world.shipBody = value; }
		}
		public MapPhysics world { get; set; }
		public Factories factories { get; set; }
		public bool isSleep { get; set; }

		public void Init(Ship newShip)
		{
			ship = newShip;
			ship.transform.SetParent(transform);
			ship.mind.Init(world);
			world.shipMind = ship.mind;
			factories = world.factories;
		}
		public void Play()
		{
			isSleep = false;
			
			SpawnGroundUnits();
		}
		public void Restart()
		{
		}
		public void Pause(bool isPause)
		{
		}

		private void Start()
		{
			world.ground = m_ground;
			world.sky = m_sky;
			isSleep = true;
		}
		private void FixedUpdate()
		{
			UpdateGameSleep();

			if (isSleep)
			{
				return;
			}

			SpawnFlyInits();
		}
		private void UpdateGameSleep()
		{
			world.isSleep = isSleep;
			ship.mind.isSleep = isSleep;
		}
		private void SpawnFlyInits()
		{
			FlySpawn spawn = m_flySpawns.Find(x => x.offset <= world.offset);
			if (spawn == null)
			{
				return;
			}

			CurvySpline road = factories.roads.Get(spawn.road);
			for (int i = 0; i < spawn.count; i++)
			{
				Enemy enemy = Instantiate(spawn.enemy, m_sky);
				world.AddEnemy(enemy);
				enemy.splineController.Spline = road;
				float spawnPosition = MapPhysics.SPAWN_OFFSET * i / road.Length;
				enemy.splineController.InitialPosition = spawnPosition;
				enemy.splineController.Speed = spawn.speed;
			}
			m_flySpawns.Remove(spawn);
		}
		private void SpawnGroundUnits()
		{
			m_groundSpawns.ForEach(spawn =>
			{
				Enemy enemy = Instantiate(spawn.enemy, m_ground);
				world.AddEnemy(enemy);
				enemy.position = spawn.position;
			});
		}
	}
}
