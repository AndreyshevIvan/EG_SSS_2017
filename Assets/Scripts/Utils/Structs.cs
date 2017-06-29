using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluffyUnderware.Curvy;
using MyGame.World;

namespace MyGame
{
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

	[System.Serializable]
	public struct MapsFactory
	{
		public Map m_firstMap;

		public Map GetMap()
		{
			return Component.Instantiate(m_firstMap);
		}
	}

	[System.Serializable]
	public struct EnemiesFactory
	{
		public EasyEnemy m_easyEnemy;
		public MiddleEnemy m_middleEnemy;
		public HardEnemy m_hardEnemy;

		public EasyEnemy easy { get { return Component.Instantiate(m_easyEnemy); } }
		public MiddleEnemy middle { get { return Component.Instantiate(m_middleEnemy); } }
		public HardEnemy hard { get { return Component.Instantiate(m_hardEnemy); } }
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
			ship.mind.type = type;
			return ship;
		}
	}

	[System.Serializable]
	public struct RoadsFactory
	{
		public CurvySpline big;
		public CurvySpline left;
		public CurvySpline right;
		public CurvySpline diff;

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
			switch (type)
			{
				case BonusType.STAR:
					return star;

				case BonusType.HEALTH:
					return health;

				case BonusType.AMMO_UP:
					return ammo;
			}

			return star;
		}
	}

	public struct TempPlayer
	{
		public int points { get; set; }
		public int stars { get; set; }
	}
}