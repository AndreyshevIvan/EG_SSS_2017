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
		public float time { get; private set; }
		public bool isReached
		{
			get
			{
				return m_isMapEnd &&
				tempSkySpawns.Count == 0;
			}
		}
		public bool isMoveing { get; private set; }

		public void OnGameplayChange()
		{
		}
		public void Play()
		{
			tempSkySpawns = new List<FlySpawn>(m_flySpawns);
			SpawnAllGroundUnits();
		}

		[SerializeField]
		private float m_endTime;
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

		private bool m_isMapEnd = false;
		private Enemy m_enemyToDebug;

		private const float MOVE_SPEED = 1.6f;

		private void Awake()
		{
			time = 0;
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
			if (time >= m_endTime)
			{
				m_windParticles.Pause();
				m_isMapEnd = true;
				return;
			}

			float movement = MOVE_SPEED * Time.fixedDeltaTime;
			m_ground.transform.Translate(new Vector3(0, 0, -movement));
			time += Time.fixedDeltaTime;
		}
		private void SpawnFlyInits()
		{
			FlySpawn spawn = tempSkySpawns.Find(x => x.time <= time);
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
				float spawnPosition = GetSplineOffset(spawn.enemy) * i / road.Length;
				enemy.roadController.InitialPosition = spawnPosition;
				enemy.roadController.Speed = spawn.speed;
				m_enemyToDebug = enemy;
			}
			tempSkySpawns.Remove(spawn);
		}
		private void SpawnAllGroundUnits()
		{
			m_groundSpawns.ForEach(spawn => {
				Enemy enemy = factory.GetEnemy(spawn.enemy);
				enemy.transform.SetParent(m_groundObjects);
				enemy.position = spawn.position;
			});
		}
		private float GetSplineOffset(UnitType type)
		{
			switch (type)
			{
				case UnitType.BASE_ENEMY:
					return 1.2f;
				case UnitType.ROCKET_COPTER:
					return 2;
			}

			return 1;
		}
	}
}
