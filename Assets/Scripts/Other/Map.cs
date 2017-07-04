using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluffyUnderware.Curvy;
using MyGame.Hero;
using MyGame.Factory;
using MyGame.Enemies;

namespace MyGame
{
	public sealed class Map : MonoBehaviour
	{
		public IGameplay gameplay { get; set; }
		public IFactory factory { get; set; }
		public Transform groundObjects { get { return m_groundObjects; } }
		public Transform skyObjects { get { return m_skyObjects; } }
		public float offset { get; private set; }
		public bool isReached { get; private set; }
		public bool isMoveing { get; private set; }

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
		private float m_maxOffset;
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

		private List<FlySpawn> tempSkySpawns { get; set; }

		private const float MOVE_SPEED = 1.7f;

		private void Awake()
		{
			isReached = false;
			offset = 0;
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
			if (offset >= m_maxOffset)
			{
				return;
			}

			float movement = MOVE_SPEED * Time.fixedDeltaTime;
			m_ground.transform.Translate(new Vector3(0, 0, -movement));
			offset += movement;
		}
		private void SpawnFlyInits()
		{
			FlySpawn spawn = tempSkySpawns.Find(x => x.offset <= offset);
			if (spawn == null)
			{
				return;
			}

			CurvySpline road = factory.GetRoad(spawn.road);
			for (int i = 0; i < spawn.count; i++)
			{
				Enemy enemy = factory.GetEnemy(spawn.enemy);
				enemy.transform.SetParent(m_skyObjects);
				enemy.roadController.Spline = road;
				float spawnPosition = GameWorld.SPAWN_OFFSET * i / road.Length;
				enemy.roadController.InitialPosition = spawnPosition;
				enemy.roadController.Speed = spawn.speed;
			}
			tempSkySpawns.Remove(spawn);
		}
		private void SpawnGroundUnits()
		{
			m_groundSpawns.ForEach(spawn => {
				Enemy enemy = factory.GetEnemy(spawn.enemy);
				enemy.transform.SetParent(m_groundObjects);
				enemy.position = spawn.position;
			});
		}
	}
}
