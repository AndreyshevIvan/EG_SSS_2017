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

		public MapPhysics world { get; set; }
		public bool isSleep { get; set; }

		public void Play()
		{
			isSleep = false;
			tempSkySpawns = new List<FlySpawn>(m_flySpawns);
			SpawnGroundUnits();
		}
		public void Pause(bool isPause)
		{
		}

		private Ship ship { get { return world.ship; } }
		private Factories factories { get { return world.factories; } }
		private List<FlySpawn> tempSkySpawns { get; set; }

		private void Start()
		{
			ship.transform.SetParent(transform);
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
		}
		private void SpawnFlyInits()
		{
			FlySpawn spawn = tempSkySpawns.Find(x => x.offset <= world.offset);
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
			tempSkySpawns.Remove(spawn);
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
