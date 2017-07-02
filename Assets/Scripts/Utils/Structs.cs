using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluffyUnderware.Curvy;
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
		public Map GetMap()
		{
			return Component.Instantiate(m_firstMap);
		}

		[SerializeField]
		private Map m_firstMap;
	}

	[System.Serializable]
	public struct EnemiesFactory
	{
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

			return Component.Instantiate(enemy);
		}

		[SerializeField]
		private Enemy m_easyEnemy;
		[SerializeField]
		private Enemy m_middleEnemy;
		[SerializeField]
		private Enemy m_hardEnemy;
	}

	[System.Serializable]
	public struct RoadsFactory
	{
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

		[SerializeField]
		private CurvySpline big;
		[SerializeField]
		private CurvySpline left;
		[SerializeField]
		private CurvySpline right;
		[SerializeField]
		private CurvySpline diff;
		[SerializeField]
		private CurvySpline player;
	}

	[System.Serializable]
	public struct BonusesFactory
	{
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

			return Component.Instantiate(bonus);
		}

		[SerializeField]
		private Bonus star;
		[SerializeField]
		private Bonus health;
		[SerializeField]
		private Bonus ammo;
	}

	[System.Serializable]
	public struct ShipsFactory
	{
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
			//ship.mind.type = type;
			return ship;
		}

		[SerializeField]
		private Ship m_modelFirst;
		[SerializeField]
		private Ship m_modelSecond;
		[SerializeField]
		private Ship m_modelThird;
	}

	[System.Serializable]
	public struct BarsFactory
	{
		public UIBar Get(BarType type)
		{
			UIBar newBar = null;

			switch (type)
			{
				case BarType.ENEMY_HEALTH:
					newBar = enemyHealth;
					break;

				case BarType.PLAYER_HEALTH:
					newBar = playerHealth;
					break;
			}

			return Component.Instantiate(newBar);
		}

		[SerializeField]
		private UIBar playerHealth;
		[SerializeField]
		private UIBar enemyHealth;
	}
}