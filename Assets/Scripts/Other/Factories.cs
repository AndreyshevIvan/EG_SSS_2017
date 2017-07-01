using UnityEngine;
using FluffyUnderware.Curvy;
using UnityEditor;
using MyGame.Hero;
using System;

namespace MyGame.Factory
{
	public class Factories : MonoBehaviour, IFactory
	{
		public void Init(IWorldContainer world, UIContainer gameInterface)
		{
			m_world = world;
			m_interface = gameInterface;

			Start();
		}

		public Map GetMap()
		{
			Map newMap = m_maps.GetMap();
			return newMap;
		}
		public Enemy GetEnemy(EnemyType type)
		{
			Enemy newEnemy = m_enemies.Get(type);
			m_world.AddEnemy(newEnemy);
			return newEnemy;
		}
		public Ship GetShip(ShipType type)
		{
			Ship newShip = m_ships.Get(type);
			return newShip;
		}
		public CurvySpline GetRoad(RoadType type)
		{
			CurvySpline newRoad = m_roads.Get(type);
			return newRoad;
		}
		public Bonus GetBonus(BonusType type)
		{
			Bonus newBonus = m_bonuses.Get(type);
			m_world.AddBonus(newBonus);
			return newBonus;
		}
		public UIBar GetBar(BarType type)
		{
			UIBar newBar = m_bars.Get(type);
			m_interface.AddBar(newBar);
			return newBar;
		}

		[SerializeField]
		private MapsFactory m_maps;
		[SerializeField]
		private EnemiesFactory m_enemies;
		[SerializeField]
		private ShipsFactory m_ships;
		[SerializeField]
		private RoadsFactory m_roads;
		[SerializeField]
		private BonusesFactory m_bonuses;
		[SerializeField]
		private BarsFactory m_bars;

		private IWorldContainer m_world;
		private UIContainer m_interface;

		private void Start()
		{
			if (m_world == null)
			{
				throw new Exception("Factory: world not init");
			}
			if (m_interface == null)
			{
				throw new Exception("Factory: interface not init");
			}
		}
	}

	public interface IFactory
	{
		void Init(IWorldContainer world, UIContainer gameInterface);

		Map GetMap();
		Enemy GetEnemy(EnemyType type);
		Ship GetShip(ShipType type);
		CurvySpline GetRoad(RoadType type);
		Bonus GetBonus(BonusType type);
		UIBar GetBar(BarType type);
	}
}
