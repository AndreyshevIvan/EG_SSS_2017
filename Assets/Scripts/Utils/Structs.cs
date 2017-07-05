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
