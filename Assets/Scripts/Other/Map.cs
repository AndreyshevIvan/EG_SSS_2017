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
		public Transform m_groundObjects;
		public Transform m_skyObjects;
		public Transform m_ground;
		public List<FlySpawn> m_flySpawns;
		public List<GroundSpawn> m_groundSpawns;

		public MapPhysics world { get; set; }
		public IGameplay gameplay { get; set; }
		public bool isReached { get; private set; }

		public void Play()
		{
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
			world.groundObjects = m_groundObjects;
			world.skyObjects = m_skyObjects;
			world.ground = m_ground;
			isReached = false;
		}
		private void FixedUpdate()
		{
			UpdateGameSleep();

			if (!gameplay.isPlaying)
			{
				return;
			}

			SpawnFlyInits();
		}
		private void UpdateGameSleep()
		{
			if (gameplay.isMapSleep)
			{
				m_windParticles.Pause();
				return;
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
				Enemy enemy = Instantiate(spawn.enemy, m_skyObjects);
				world.AddEnemy(enemy);
				enemy.roadController.Spline = road;
				float spawnPosition = MapPhysics.SPAWN_OFFSET * i / road.Length;
				enemy.roadController.InitialPosition = spawnPosition;
				enemy.roadController.Speed = spawn.speed;
			}
			tempSkySpawns.Remove(spawn);
		}
		private void SpawnGroundUnits()
		{
			m_groundSpawns.ForEach(spawn =>
			{
				Enemy enemy = Instantiate(spawn.enemy, m_groundObjects);
				world.AddEnemy(enemy);
				enemy.position = spawn.position;
			});
		}
	}
}
