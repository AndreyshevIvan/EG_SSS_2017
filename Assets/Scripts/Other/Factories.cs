using UnityEngine;
using FluffyUnderware.Curvy;
using UnityEditor;
using MyGame.Hero;

namespace MyGame.Factory
{
	public class Factories : MonoBehaviour, IFactory
	{
		public void Init(WorldContainer world, UIContainer gameInterface)
		{
			m_world = world;
			m_interface = gameInterface;
		}

		public Map GetMap()
		{
			Map newMap = m_maps.GetMap();
			return newMap;
		}
		public Enemy GetEnemy(EnemyType type)
		{
			Enemy newEnemy = m_enemies.Get(type);
			m_world.Add(newEnemy);
			return newEnemy;
		}
		public Ship GetShip(ShipType type)
		{
			Ship newShip = m_ships.Get(type);
			//newShip.mind.type = type;
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
			m_world.Add(newBonus);
			return newBonus;
		}
		public UIBar GetBar(BarType type)
		{
			UIBar newBar = m_bars.Get(type);
			m_interface.Add(newBar);
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

		private WorldContainer m_world;
		private UIContainer m_interface;
	}

	public interface IFactory
	{
		void Init(WorldContainer world, UIContainer gameInterface);

		Map GetMap();
		Enemy GetEnemy(EnemyType type);
		Ship GetShip(ShipType type);
		CurvySpline GetRoad(RoadType type);
		Bonus GetBonus(BonusType type);
		UIBar GetBar(BarType type);
	}
}
