using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluffyUnderware.Curvy;
using MyGame.Hero;
using MyGame.Enemies;
using System;
using Malee;
using MyGame;

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
		public bool Contain(Vector3 point)
		{
			return
				point.x >= xMin &&
				point.x <= xMax &&
				point.z <= zMax &&
				point.z >= zMin;
		}

		public float xMin;
		public float xMax;
		public float zMin;
		public float zMax;
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

	[System.Serializable]
	public class ReordarableList<T> : ReorderableArray<T>
	{
		public T Find(Predicate<T> match)
		{
			for (int i = 0; i < Length; i++)
			{
				if (match(this[i])) return this[i];
			}

			return default(T);
		}
		public List<T> FindAll(Predicate<T> match)
		{
			List<T> result = new List<T>();
			for (int i = 0; i < Length; i++)
			{
				if (match(this[i])) result.Add(this[i]);
			}
			return result;
		}
	}
}

namespace GameFactory
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

	[System.Serializable]
	public class BonusesFactoryList : ReordarableList<BonusPair>
	{
	}
}

namespace GameSpawns
{
	[System.Serializable]
	public class FlySpawn
	{
		public RoadType road;
		public UnitType enemy;
		public float time;
		public float speed;
		public int count;

		public override string ToString()
		{
			string result = road.ToString() + " ";
			result += enemy.ToString() + " ";
			result += time.ToString() + " ";
			result += speed.ToString() + " ";
			result += count.ToString();
			return result;
		}
	}
	[System.Serializable]
	public class GroundSpawn
	{
		public Vector3 position;
		public UnitType enemy;

		public override string ToString()
		{
			string result = position.ToString() + " ";
			result += enemy.ToString();
			return result;
		}
	}

	[System.Serializable]
	public class SkySpawnList : ReordarableList<FlySpawn>
	{
	}

	[System.Serializable]
	public class GroundSpawnList : ReordarableList<GroundSpawn>
	{
	}
}
