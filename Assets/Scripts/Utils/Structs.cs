using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluffyUnderware.Curvy;
using MyGame.World;
using MyGame.Hero;

namespace MyGame
{
	public struct TempPlayer
	{
		public int points { get; set; }
		public int stars { get; set; }
	}

	[System.Serializable]
	public struct BoundingBox
	{
		public BoundingBox(float xMin, float xMax, float zMin, float zMax)
		{
			this.xMin = xMin;
			this.xMax = xMax;
			this.zMin = zMin;
			this.zMax = zMax;
		}

		public float xMin;
		public float xMax;
		public float zMin;
		public float zMax;
	}

	[System.Serializable]
	public class FlySpawn
	{
		public RoadType road;
		public Enemy enemy;
		public float offset;
		public float speed;
		public int count;
	}
	[System.Serializable]
	public class GroundSpawn
	{
		public Vector3 position;
		public Enemy enemy;
	}
}
namespace MyGame.Factory
{
	[System.Serializable]
	public struct MapsFactory
	{
		public Map m_firstMap;

		public Map GetMap()
		{
			Map newMap = Component.Instantiate(m_firstMap);
			newMap.Init(gameWorld);
			return newMap;
		}

		internal GameWorld gameWorld;
	}

	[System.Serializable]
	public struct EnemiesFactory
	{
		public Enemy m_easyEnemy;
		public Enemy m_middleEnemy;
		public Enemy m_hardEnemy;

		public Enemy Get(EnemyType type)
		{
			Enemy enemy = null;

			switch (type)
			{
				case EnemyType.EASY:
					enemy = m_easyEnemy;
					break;

				case EnemyType.NORMAL:
					enemy = m_middleEnemy;
					break;

				case EnemyType.HARD:
					enemy = m_hardEnemy;
					break;
			}

			Enemy newEnemy = Component.Instantiate(enemy);
			newEnemy.Init(gameWorld);
			return newEnemy;
		}

		internal GameWorld gameWorld;
	}

	[System.Serializable]
	public struct RoadsFactory
	{
		public CurvySpline big;
		public CurvySpline left;
		public CurvySpline right;
		public CurvySpline diff;
		public CurvySpline player;

		public CurvySpline Get(RoadType type)
		{
			switch (type)
			{
				case RoadType.BIG:
					return big;

				case RoadType.LEFT:
					return left;

				case RoadType.RIGHT:
					return right;

				case RoadType.DIFF:
					return diff;

				case RoadType.PLAYER:
					return player;
			}

			return big;
		}
	}

	[System.Serializable]
	public struct BonusesFactory
	{
		public Bonus star;
		public Bonus health;
		public Bonus ammo;

		public Bonus Get(BonusType type)
		{
			Bonus bonus = null;

			switch (type)
			{
				case BonusType.STAR:
					bonus = star;
					break;

				case BonusType.HEALTH:
					bonus = health;
					break;

				case BonusType.AMMO_UP:
					bonus = ammo;
					break;
			}

			Bonus newBonus = Component.Instantiate(bonus);
			newBonus.Init(gameWorld);
			return newBonus;
		}

		internal GameWorld gameWorld;
	}

	[System.Serializable]
	public struct ShipsFactory
	{
		public Ship m_modelFirst;
		public Ship m_modelSecond;
		public Ship m_modelThird;

		public Ship Get(ShipType type)
		{
			Ship newShip = null;

			switch (type)
			{
				case ShipType.VOYAGER:
					newShip = m_modelFirst;
					break;

				case ShipType.DESTENY:
					newShip = m_modelSecond;
					break;

				case ShipType.SPLASH:
					newShip = m_modelThird;
					break;
			}

			Ship ship = Component.Instantiate(newShip);
			ship.Init(gameWorld);
			ship.mind.type = type;
			return ship;
		}

		internal GameWorld gameWorld;
	}
}