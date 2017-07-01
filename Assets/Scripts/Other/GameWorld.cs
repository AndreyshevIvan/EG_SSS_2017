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
	public partial class GameWorld : MonoBehaviour, IGameWorld, IGameplay, IGameplayObject
	{
		public IFactory factory { get; set; }
		public Map map { get; set; }
		public Ship ship { get; set; }
		public ShipMind shipMind { get { return ship.mind; } }

		public IGameplay gameplay { protected get; set; }
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

		public void Init(IGameWorld gameWorld)
		{
		}
		public void OnWorldChange()
		{
		}

		public void AddEnemy(Enemy enemy)
		{
			if (!enemy) return;
			Add(m_enemies, enemy);
		}
		public void AddAmmo(Ammo ammo)
		{
			if (!ammo) return;

			ammo.transform.SetParent(map.m_skyObjects);
			Add(m_ammo, ammo);
		}
		public void AddBonus(Bonus bonus)
		{
			if (!bonus) return;
			Add(m_bonuses, bonus);
		}

		public void EraseEnemy(Enemy enemy)
		{
			Erase(m_enemies, enemy);
		}
		public void EraseAmmo(Ammo ammo)
		{
			Erase(m_ammo, ammo);
		}
		public void EraseBonus(Bonus bonus)
		{
			Erase(m_bonuses, bonus);
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
			body.transform.SetParent(map.m_groundObjects);
		}
		public void MoveToShip(WorldObject body, bool useShipMagnetic = true)
		{
			float distance = Vector3.Distance(body.position, ship.position);
			if (distance > shipMind.magnetDistance)
			{
				return;
			}

			float factor = (useShipMagnetic) ? shipMind.magnetic : 1;
			float distanceFactor = shipMind.magnetDistance / distance;
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
			explosionObject.transform.SetParent(map.m_skyObjects);
		}

		public void OnTriggerExit(Collider other)
		{
			WorldObject body = other.GetComponent<WorldObject>();
			if (!body) return;

			body.OnExitFromWorld();
			body.Cleanup();
			Destroy(body.gameObject);
		}

		public void Cleanup()
		{
			List<Component> toDelete = new List<Component>();
			toDelete.AddRange(Utils.GetChilds<Component>(map.m_skyObjects));
			toDelete.AddRange(Utils.GetChilds<Component>(map.m_groundObjects));
			toDelete.ForEach(element => Destroy(element.gameObject));
		}

		private List<WorldObject> m_toEarse = new List<WorldObject>();
		private List<Enemy> m_enemies = new List<Enemy>();
		private List<Bonus> m_bonuses = new List<Bonus>();
		private List<Ammo> m_ammo = new List<Ammo>();

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

		private void Add<T>(List<T> list, T newObject) where T : WorldObject
		{
			if (list.Find(obj => obj == newObject))
			{
				return;
			}

			newObject.Init(this);
			list.Add(newObject);
		}
		private void Erase<T>(List<T> list, T eraseObject) where T : WorldObject
		{
			if (m_toEarse.Find(obj => obj == eraseObject) ||
				!list.Find(obj => obj == eraseObject))
			{
				return;
			}

			m_toEarse.Add(eraseObject);
			eraseObject.OnErase();
			m_toEarse.Remove(eraseObject);
			list.Remove(eraseObject);
			Destroy(eraseObject.gameObject);
		}
	}

	public interface IGameWorld : IWorldContainer
	{
		IFactory factory { get; }
		Ship ship { get; }
		ShipMind shipMind { get; }

		Vector3 GetNearestEnemy(Vector3 point);

		void CreateExplosion(ParticleSystem explosion, Vector3 position);
		void SubscribeToMove(WorldObject body);
		void MoveToShip(WorldObject body, bool useShipMagnetic = true);
	}

	public interface IWorldContainer
	{
		void AddEnemy(Enemy enemy);
		void AddAmmo(Ammo ammo);
		void AddBonus(Bonus bonus);

		void EraseEnemy(Enemy enemy);
		void EraseAmmo(Ammo ammo);
		void EraseBonus(Bonus bonus);
	}

	delegate float MyDelegate(float value);
}