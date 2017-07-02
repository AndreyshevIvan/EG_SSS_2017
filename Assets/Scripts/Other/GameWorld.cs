using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FluffyUnderware.Curvy;
using MyGame.Hero;
using MyGame.Factory;

namespace MyGame
{
	public class GameWorld : MonoBehaviour, IGameWorld, IGameplay
	{
		public IPlayerBar playerBar { get; set; }
		public IFactory factory { get; set; }
		public Map map { get; set; }
		public Ship ship { get; set; }
		public WorldContainer container { get; private set; }

		public IGameplay gameplay { get; set; }
		public bool isMapStart { get { return gameplay.isMapStart; } }
		public bool isPaused { get { return gameplay.isPaused; } }
		public bool isMapStay { get { return gameplay.isMapStay; } }
		public bool isGameEnd { get { return gameplay.isGameEnd; } }
		public bool isWin { get { return gameplay.isWin; } }
		public bool isPlaying { get { return gameplay.isPlaying; } }

		public const float FLY_HEIGHT = 4;
		public const float SPAWN_OFFSET = 1.2f;
		public const int WORLD_BOX_LAYER = 31;
		public const int MODIFICATION_COUNT = 7;

		public void OnGameplayChange()
		{
			ship.OnGameplayChange();
			container.NotifyObjects();
		}
		public void Add<T>(T obj) where T : IWorldEntity
		{
			if (obj.isWorldSet)
			{
				return;
			}

			container.Add(obj);
		}
		public void Remove<T>(T obj, bool isOpenBeforeDelete) where T : IWorldEntity
		{
			container.Remove(obj, isOpenBeforeDelete);
		}

		public Vector3 GetNearestEnemy(Vector3 point)
		{
			return Vector3.zero;
		}
		public void SetSlowMode(bool isModeOn)
		{
			float targetScale = (isModeOn) ? SLOW_TIMESCALE : 1;

			if (isModeOn != m_lastModeType)
			{
				Time.fixedDeltaTime = (isModeOn) ? SLOWMO_DT : NORMAL_DT;
				m_deltaScale = Mathf.Abs(targetScale - Time.timeScale);
				m_lastModeType = isModeOn;
			}

			float step = Time.fixedDeltaTime / GameplayUI.SLOWMO_CHANGE_TIME * m_deltaScale;
			Time.timeScale = Mathf.MoveTowards(Time.timeScale, targetScale, step);
		}
		public void SubscribeToMove(WorldObject body)
		{
			body.transform.SetParent(map.groundObjects);
		}
		public void MoveToShip(WorldObject body, bool useShipMagnetic = true)
		{
			float distance = Vector3.Distance(body.position, ship.position);
			if (distance > ship.mind.magnetDistance)
			{
				return;
			}

			float factor = (useShipMagnetic) ? ship.mind.magnetic : 1;
			float distanceFactor = ship.mind.magnetDistance / distance;
			float movement = factor * distanceFactor * MAGNETIC_SPEED * Time.fixedDeltaTime;
			body.position = Vector3.MoveTowards(
				body.position,
				ship.position,
				movement);
		}
		public void KillPlayer()
		{
			Dismantle(ship);
			Destroy(ship);
		}
		public void CreateExplosion(ParticleSystem explosion, Vector3 position)
		{
			if (explosion == null)
			{
				return;
			}

			ParticleSystem explosionObject = Instantiate(explosion);
			explosionObject.transform.position = position;
			explosionObject.transform.SetParent(map.skyObjects);
		}

		public void OnTriggerExit(Collider other)
		{
			WorldObject body = other.GetComponent<WorldObject>();
			if (body == null) return;

			container.Remove(body, false);
			Destroy(body.gameObject);
		}

		public void Cleanup()
		{
		}

		private BoundingBox m_gameBox;
		private bool m_lastModeType = false;
		private float m_deltaScale = 1 - SLOW_TIMESCALE;

		private const float MAGNETIC_SPEED = 2;
		private const float MAP_MOVE_SPEED = 1.7f;
		private const float SLOW_TIMESCALE = 0.2f;
		private const float DISMANTLE_FORCE = 300;
		private const float NORMAL_DT = 0.02f;
		private const float SLOWMO_DT = 0.01f;
		private const int GARBAGE_LAYER = 12;

		private void Awake()
		{
			m_gameBox = GameData.mapBox;
			container = new WorldContainer(this);
		}
		private void Dismantle(WorldObject dismantleObject)
		{
			/*
			if (!dismantleObject)
			{
				return;
			}

			CreateExplosion(dismantleObject.explosion, dismantleObject.position);
			List<Rigidbody> bodies = Utils.GetChilds<Rigidbody>(dismantleObject);
			bodies.ForEach(body =>
			{
				body.transform.SetParent(groundObjects);
				body.useGravity = true;
				body.isKinematic = false;
				body.gameObject.layer = GARBAGE_LAYER;
				body.AddForce(Utils.RandomVect(-DISMANTLE_FORCE, DISMANTLE_FORCE));
				Renderer renderer = body.GetComponentInChildren<Renderer>();
				if (renderer != null) renderer.material = m_garbageMaterial;
			});
			*/
		}
	}

	public interface IGameWorld
	{
		IFactory factory { get; }
		Ship ship { get; }

		void Add<T>(T obj) where T : IWorldEntity;
		void Remove<T>(T obj, bool isOpenBeforeDelete) where T : IWorldEntity;

		Vector3 GetNearestEnemy(Vector3 point);

		void CreateExplosion(ParticleSystem explosion, Vector3 position);
		void SubscribeToMove(WorldObject body);
		void MoveToShip(WorldObject body, bool useShipMagnetic = true);
	}

	public class WorldContainer
	{
		public WorldContainer(IGameWorld initWorld)
		{
			world = initWorld;
		}

		public void Add<T>(T obj) where T : IWorldEntity
		{
			if (obj is Enemy)
			{
				AddEnemy(obj as Enemy);
			}
			else if (obj is Bonus)
			{
				AddBonus(obj as Bonus);
			}
			else if (obj is Ammo)
			{
				AddAmmo(obj as Ammo);
			}
		}
		public void Remove<T>(T obj, bool isOpenBeforeDelete) where T : IWorldEntity
		{
			if (obj is Enemy)
			{
				EraseEnemy(obj as Enemy);
			}
			else if (obj is Bonus)
			{
				EraseBonus(obj as Bonus);
			}
			else if (obj is Ammo)
			{
				EraseAmmo(obj as Ammo);
			}
		}
		public void NotifyObjects()
		{
			m_enemies.ForEach(element => element.OnGameplayChange());
			m_bonuses.ForEach(element => element.OnGameplayChange());
			m_ammo.ForEach(element => element.OnGameplayChange());
		}

		private List<Enemy> m_enemies = new List<Enemy>();
		private List<Bonus> m_bonuses = new List<Bonus>();
		private List<Ammo> m_ammo = new List<Ammo>();
		private List<Property> m_properties = new List<Property>();

		private void AddEnemy(Enemy enemy)
		{
			if (!enemy) return;
			AddObject(m_enemies, enemy);
		}
		private void AddAmmo(Ammo ammo)
		{
			if (!ammo) return;
			AddObject(m_ammo, ammo);
		}
		private void AddBonus(Bonus bonus)
		{
			if (!bonus) return;
			AddObject(m_bonuses, bonus);
		}

		private void EraseEnemy(Enemy enemy)
		{
			EraseObject(m_enemies, enemy);
		}
		private void EraseAmmo(Ammo ammo)
		{
			EraseObject(m_ammo, ammo);
		}
		private void EraseBonus(Bonus bonus)
		{
			EraseObject(m_bonuses, bonus);
		}

		private void AddObject<T>(List<T> list, T newObject) where T : IWorldEntity
		{
			if (list.Find(obj => obj.Equals(newObject)) != null)
			{
				return;
			}

			newObject.Init(world);
			list.Add(newObject);
		}
		private void EraseObject<T>(List<T> list, T eraseObject) where T : IWorldEntity
		{
			list.Remove(eraseObject);
			Component component = eraseObject as Component;
			if (component) Component.Destroy(component.gameObject);
		}

		private IGameWorld world { get; set; }
	}
}