using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluffyUnderware.Curvy;
using MyGame.Hero;
using MyGame.Enemies;
using System;

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
		public UnitType enemy;
		public float offset;
		public float speed;
		public int count;
	}
	[System.Serializable]
	public class GroundSpawn
	{
		public Vector3 position;
		public UnitType enemy;
	}

	[System.Serializable]
	public class Pair<TKey, TValue>
	{
		[SerializeField]
		public TKey key;
		[SerializeField]
		public TValue value;
	}

	[System.Serializable]
	public class EnemiesPair : Pair<UnitType, Enemy> {}
	[System.Serializable]
	public class BonusPair : Pair<BonusType, Bonus> {}
	[System.Serializable]
	public class SplinePair : Pair<RoadType, CurvySpline> {}
	[System.Serializable]
	public class ShipPair : Pair<ShipType, Ship> {}
	[System.Serializable]
	public class BarsPair : Pair<BarType, UIBar> {}
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
		public Enemy Get(UnitType type)
		{
			Enemy enemy = m_list.Find(pair => pair.key == type).value;
			return Component.Instantiate(enemy);
		}

		[SerializeField]
		private List<EnemiesPair> m_list;
	}

	[System.Serializable]
	public struct RoadsFactory
	{
		public CurvySpline Get(RoadType type)
		{
			return m_list.Find(pair => pair.key == type).value;
		}

		[SerializeField]
		private List<SplinePair> m_list;
	}

	[System.Serializable]
	public struct BonusesFactory
	{
		public Bonus Get(BonusType type)
		{
			Bonus bonus = m_list.Find(pair => pair.key == type).value;
			return Component.Instantiate(bonus);
		}

		[SerializeField]
		private List<BonusPair> m_list;
	}

	[System.Serializable]
	public struct ShipsFactory
	{
		public Ship Get(ShipType type)
		{
			Ship ship = m_list.Find(pair => pair.key == type).value;
			return Component.Instantiate(ship);
		}

		[SerializeField]
		private List<ShipPair> m_list;
	}

	[System.Serializable]
	public struct BarsFactory
	{
		public UIBar Get(BarType type)
		{
			UIBar newBar = m_list.Find(pair => pair.key == type).value;
			return Component.Instantiate(newBar);
		}

		[SerializeField]
		private List<BarsPair> m_list;
	}
}
