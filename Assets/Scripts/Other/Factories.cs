using System.Collections.Generic;
using UnityEngine;
using FluffyUnderware.Curvy;
using MyGame.Hero;
using MyGame.Enemies;
using MyGame;
using Malee;

namespace GameFactory
{
	public class Factories : MonoBehaviour, IFactory
	{
		public void Init(WorldContainer world, UIContainer gameInterface)
		{
			m_world = world;
			m_interface = gameInterface;
		}

		public Map GetMap(MapType key)
		{
			Map map = m_maps.Find(pair => pair.key.Equals(key)).value;
			return Component.Instantiate(map);
		}

		public Enemy GetEnemy(UnitType type)
		{
			Enemy origin = m_enemies.Find(pair => pair.key == type).value;
			Enemy enemy = Instantiate(origin, INVISIBLE_SPAWN, Quaternion.identity);
			m_world.Add(enemy);
			enemy.type = type;
			return enemy;
		}

		public CurvySpline GetRoad(RoadType type)
		{
			return m_roads.Find(pair => pair.key == type).value;
		}

		public Ship GetShip(ShipType type)
		{
			Ship original = m_ships.Find(pair => pair.key == type).value;
			Ship ship = Instantiate(original, INVISIBLE_SPAWN, Quaternion.identity);
			return ship;
		}

		public Bonus GetBonus(BonusType type)
		{
			Bonus bonus = Instantiate(m_bonuses. Find(pair => pair.key == type).value);
			bonus.type = type;
			m_world.Add(bonus);
			return bonus;
		}

		public UIBar GetBar(BarType type)
		{
			UIBar bar = Instantiate(m_bars.Find(pair => pair.key == type).value);
			m_interface.Add(bar);
			return bar;
		}

		public Body GetAmmo(AmmoType type)
		{
			Body ammo = Instantiate(m_ammo.Find(pair => pair.key == type).value);
			m_world.Add(ammo);
			return ammo;
		}

		[SerializeField]
		private List<MapPair> m_maps;
		[SerializeField]
		private List<EnemiesPair> m_enemies;
		[SerializeField]
		private List<SplinePair> m_roads;
		[SerializeField]
		private List<ShipPair> m_ships;
		[SerializeField][Reorderable]
		private BonusesFactoryList m_bonuses;
		[SerializeField]
		private List<BarsPair> m_bars;
		[SerializeField]
		private List<AmmoPair> m_ammo;

		private WorldContainer m_world;
		private UIContainer m_interface;

		private Vector3 INVISIBLE_SPAWN = new Vector3(1000, 1000, 1000);
	}

	public interface IFactory
	{
		void Init(WorldContainer world, UIContainer gameInterface);

		Map GetMap(MapType type);
		Enemy GetEnemy(UnitType type);
		Ship GetShip(ShipType type);
		CurvySpline GetRoad(RoadType type);
		Bonus GetBonus(BonusType type);
		UIBar GetBar(BarType type);
		Body GetAmmo(AmmoType type);
	}
}
