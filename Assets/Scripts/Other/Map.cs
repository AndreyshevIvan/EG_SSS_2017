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
		public Transform groundObjects { get { return m_groundObjects; } }
		public Transform skyObjects { get { return m_skyObjects; } }
		public float offset { get; private set; }
		public bool isReached { get; private set; }
		public bool isMoveing { get; private set; }

		public void InitGameplay(IGameplay gameplay)
		{
			this.gameplay = gameplay;
			offset = 0;
		}
		public void OnGameplayChange()
		{
			if (gameplay.isMapStay)
			{
				m_windParticles.Pause();
				return;
			}

			m_windParticles.Play();
		}
		public void Play()
		{
			tempSkySpawns = new List<FlySpawn>(m_flySpawns);
			SpawnGroundUnits();
		}
		public void Pause(bool isPause)
		{
		}

		[SerializeField]
		private ParticleSystem m_windParticles;
		[SerializeField]
		private Transform m_groundObjects;
		[SerializeField]
		private Transform m_skyObjects;
		[SerializeField]
		private Transform m_ground;
		[SerializeField]
		private List<FlySpawn> m_flySpawns;
		[SerializeField]
		private List<GroundSpawn> m_groundSpawns;

		private IGameplay gameplay { get; set; }
		private List<FlySpawn> tempSkySpawns { get; set; }

		private void Awake()
		{
			isReached = false;
		}
		private void FixedUpdate()
		{
			if (!gameplay.isPlaying)
			{
				return;
			}

			MoveGround();
			SpawnFlyInits();
		}
		private void MoveGround()
		{

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
