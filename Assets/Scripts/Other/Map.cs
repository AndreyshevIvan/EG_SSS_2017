using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluffyUnderware.Curvy;
using MyGame.Hero;

namespace MyGame
{
	public sealed class Map : MonoBehaviour
	{
		public ParticleSystem m_windParticles;
		public Transform m_ground;
		public Transform m_sky;
		public List<FlySpawn> m_flySpawns;
		public List<GroundSpawn> m_groundSpawns;

		public MapPhysics world { get; set; }
		public bool isGamePaused { get; set; }
		public bool isSleep { get; private set; }

		public void Play()
		{
			isSleep = false;
			tempSkySpawns = new List<FlySpawn>(m_flySpawns);
			SpawnGroundUnits();
		}
		public void Pause(bool isPause)
		{
		}
		public void Stop()
		{
			isSleep = true;
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
			world.isGamePaused = isGamePaused;
			if (isGamePaused)
			{
				return;
			}

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

			if (isSleep)
			{
				m_windParticles.Pause();
			}

			m_windParticles.Play();
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
