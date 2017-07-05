using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluffyUnderware.Curvy;
using MyGame.Hero;
using MyGame.Enemies;
using System;

namespace MyGame
{
	[System.Serializable]
	public class TempPlayer
	{
		public TempPlayer(IPlayerBar bar)
		{
			m_points = 0;
			playerBar = bar;
		}

		public EventDelegate onDemaged;
		public EventDelegate onLossEnemy;

		public bool isDemaged { get; private set; }
		public bool isLossEnemy { get; private set; }

		public void AddPoints(int pointsCount)
		{
			m_points = Mathf.Clamp(pointsCount + m_points, MIN_POINTS, MAX_POINTS);
			playerBar.points = m_points;
		}
		public void Demaged()
		{
			if (isDemaged)
			{
				return;
			}

			if (onDemaged != null) onDemaged();
			onDemaged = null;
			isDemaged = true;
		}
		public void LossEnemy()
		{
			if (isLossEnemy)
			{
				return;
			}

			if (onLossEnemy != null) onLossEnemy();
			onLossEnemy = null;
			isLossEnemy = true;
		}

		private int m_points;

		private const int MIN_POINTS = 0;
		private const int MAX_POINTS = 999999999;

		private IPlayerBar playerBar;
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
		public static Pair<TKey, TValue> Create(TKey key, TValue value)
		{
			Pair<TKey, TValue> newPair = new Pair<TKey, TValue>();
			newPair.key = key;
			newPair.value = value;
			return newPair;
		}

		[SerializeField]
		public TKey key;
		[SerializeField]
		public TValue value;
	}
}
namespace MyGame.Factory
{
	[System.Serializable]
	public class MapPair : Pair<MapType, Map> { }
	[System.Serializable]
	public class EnemiesPair : Pair<UnitType, Enemy> { }
	[System.Serializable]
	public class BonusPair : Pair<BonusType, Bonus> { }
	[System.Serializable]
	public class SplinePair : Pair<RoadType, CurvySpline> { }
	[System.Serializable]
	public class ShipPair : Pair<ShipType, Ship> { }
	[System.Serializable]
	public class BarsPair : Pair<BarType, UIBar> { }
	[System.Serializable]
	public class AmmoPair : Pair<AmmoType, Body> { }
}
