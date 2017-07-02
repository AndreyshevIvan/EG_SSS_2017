using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluffyUnderware.Curvy;
using MyGame.Hero;
using MyGame.Factory;

namespace MyGame
{
	public sealed class Map : MonoBehaviour, IGameplayObject
	{
		public ParticleSystem m_windParticles;
		public Transform m_groundObjects;
		public Transform m_skyObjects;
		public Transform m_ground;
		public List<FlySpawn> m_flySpawns;
		public List<GroundSpawn> m_groundSpawns;

		public bool isReached { get; private set; }

		public void InitGameplay(IGameplay gameplay)
		{
			this.gameplay = gameplay;
		}

		public void OnGameplayChange()
		{
		}
		public void Play()
		{
			tempSkySpawns = new List<FlySpawn>(m_flySpawns);
			SpawnGroundUnits();
		}
		public void Pause(bool isPause)
		{
		}

		private IGameplay gameplay { get; set; }
		private List<FlySpawn> tempSkySpawns { get; set; }

		private void Awake()
		{
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
			if (gameplay.isMapStay)
			{
				m_windParticles.Pause();
				return;
			}

			m_windParticles.Play();
		}
		private void SpawnFlyInits()
		{
			/*
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
				float spawnPosition = GameWorld.SPAWN_OFFSET * i / road.Length;
				enemy.roadController.InitialPosition = spawnPosition;
				enemy.roadController.Speed = spawn.speed;
			}
			tempSkySpawns.Remove(spawn);*/
		}
		private void SpawnGroundUnits()
		{
			m_groundSpawns.ForEach(spawn => {
				Enemy enemy = Instantiate(spawn.enemy, m_groundObjects);
				//world.AddEnemy(enemy);
				enemy.position = spawn.position;
			});
		}
	}
}
