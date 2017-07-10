using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FluffyUnderware.Curvy;
using MyGame.Hero;
using MyGame.Factory;
using MyGame.Enemies;
using MyGame.GameUtils;

namespace MyGame
{
	public class GameWorld : MonoBehaviour, IGameWorld, IGameplay, IWorldEntity
	{
		public IPlayerBar playerInterface { get; set; }
		public IFactory factory { get; set; }
		public Map map { get; set; }
		public Ship ship
		{
			get { return m_ship; }
			set
			{
				m_ship = value;
				Add(m_ship);
				Add(m_ship.mind);
			}
		}
		public WorldContainer container { get; private set; }
		public Player player { get { return m_player; } }
		public Vector3 shipPosition { get { return ship.position; } }

		public IGameplay gameplay { get; set; }
		public bool isMapStart { get { return gameplay.isMapStart; } }
		public bool isPaused { get { return gameplay.isPaused; } }
		public bool isGameEnd { get { return gameplay.isGameEnd; } }
		public bool isWin { get { return gameplay.isWin; } }
		public bool isPlaying { get { return gameplay.isPlaying; } }

		public Transform sky { get { return map.skyObjects; } }
		public Transform ground { get { return map.groundObjects; } }
		public BoundingBox box { get; private set; }
		public float visiblePosition { get; private set; }
		public bool isAllEnemiesKilled { get { return container.isEnemiesEmpty; } }

		public const float FLY_HEIGHT = 4;
		public const float SPAWN_OFFSET = 1.2f;
		public const int WORLD_BOX_LAYER = 31;

		public void Init(IGameWorld gameWorld)
		{
		}
		public void GameplayChange()
		{
			ship.GameplayChange();
			container.GameplayChange();
		}
		public void Add<T>(T obj) where T : WorldObject
		{
			if (obj.isWorldSet)
			{
				return;
			}

			container.Add(obj);
		}
		public void Remove<T>(T obj) where T : WorldObject
		{
			container.Remove(obj);
			if (obj.openAllowed) OpenObject(obj);
			if (obj.distmantleAllowed) Dismantle(obj);
			Destroy(obj.gameObject);
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
				m_deltaScale = Mathf.Abs(targetScale - Time.timeScale);
				m_lastModeType = isModeOn;
			}

			float step = Time.fixedDeltaTime / GameplayUI.SLOWMO_CHANGE_TIME * m_deltaScale;
			Time.timeScale = Mathf.MoveTowards(Time.timeScale, targetScale, step);
			Time.fixedDeltaTime = (Time.timeScale != 1) ? SLOWMO_DT : NORMAL_DT;
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

			float factor = (useShipMagnetic) ? ship.mind.magnetFactor : 1;
			float distanceFactor = ship.mind.magnetDistance / distance;
			float movement = factor * distanceFactor * MAGNETIC_SPEED * Time.fixedDeltaTime;
			body.position = Vector3.MoveTowards(body.position, ship.position, movement);
		}
		public void KillPlayer()
		{
			if (!ship)
			{
				return;
			}

			Dismantle(ship);
			Remove(ship);
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
		public void Cleanup()
		{
		}

		private Ship m_ship;
		private Player m_player;
		private bool m_lastModeType = false;
		private bool m_isDismantle = false;
		private float m_deltaScale = 1 - SLOW_TIMESCALE;
		[SerializeField]
		private Material m_garbageMaterial;

		private const float MAGNETIC_SPEED = 2;
		private const float SLOW_TIMESCALE = 0.2f;
		private const float DISMANTLE_FORCE = 300;
		private const float NORMAL_DT = 0.02f;
		private const float SLOWMO_DT = 0.01f;
		private const int GARBAGE_LAYER = 12;

		private void Awake()
		{
			box = GameData.mapBox;
			container = new WorldContainer();
			container.Init(this);
		}

		private void Start()
		{
			InitTempPlayer();
		}
		private void InitTempPlayer()
		{
			m_player = new Player(playerInterface, ship);
			m_player.onDemaged = () => { Debug.Log("Demaged"); };
			m_player.onLossEnemy = () => { Debug.Log("LOSS"); };
		}

		private void FixedUpdate()
		{
			//Debug.Log(container.ToString());
		}

		private void OnTriggerExit(Collider other)
		{
			WorldObject obj = other.GetComponent<WorldObject>();

			if (obj)
			{
				obj.ExitFromWorld();
				Remove(obj);
				return;
			}

			Destroy(other.gameObject);
		}
		private void OpenObject(WorldObject obj)
		{
			player.AddPoints(obj.points);

			obj.bonuses.ForEach(bonus =>
			{
				Utils.DoAnyTimes(bonus.value, () =>
				{
					Bonus newBonus = factory.GetBonus(bonus.key);
					newBonus.position = obj.position;
					newBonus.explosionStart = true;
				});
			});
		}
		private void Dismantle(WorldObject dismantleObject)
		{
			if (!dismantleObject)
			{
				return;
			}

			CreateExplosion(dismantleObject.explosion, dismantleObject.position);

			if (!m_isDismantle)
			{
				return;
			}

			List<Rigidbody> bodies = Utils.GetChilds<Rigidbody>(dismantleObject);
			bodies.ForEach(body =>
			{
				body.transform.SetParent(ground);
				body.useGravity = true;
				body.isKinematic = false;
				body.gameObject.layer = GARBAGE_LAYER;
				body.AddForce(Utils.RandomVect(-DISMANTLE_FORCE, DISMANTLE_FORCE));
				List<MeshRenderer> materials = Utils.GetAllComponents<MeshRenderer>(body.transform);
				materials.ForEach(element => element.material = m_garbageMaterial);
			});
		}
	}

	public interface IGameWorld
	{
		IFactory factory { get; }
		Player player { get; }
		Vector3 shipPosition { get; }
		Transform sky { get; }
		Transform ground { get; }
		BoundingBox box { get; }
		float visiblePosition { get; }

		void Add<T>(T obj) where T : WorldObject;
		void Remove<T>(T obj) where T : WorldObject;

		Vector3 GetNearestEnemy(Vector3 point);

		void CreateExplosion(ParticleSystem explosion, Vector3 position);
		void SubscribeToMove(WorldObject body);
		void MoveToShip(WorldObject body, bool useShipMagnetic = true);
	}

	public class WorldContainer : IWorldEntity
	{
		public bool isEnemiesEmpty { get { return m_enemies.Count == 0; } }

		public void Init(IGameWorld gameWorld)
		{
			world = gameWorld;
		}
		public void GameplayChange()
		{
			m_enemies.ForEach(element => element.GameplayChange());
			m_bonuses.ForEach(element => element.GameplayChange());
			m_other.ForEach(element => element.GameplayChange());
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
			else
			{
				AddOther(obj as WorldObject);
			}
		}
		public void Remove<T>(T obj) where T : WorldObject
		{
			if (obj is Enemy)
			{
				EraseEnemy(obj as Enemy);
			}
			else if (obj is Bonus)
			{
				EraseBonus(obj as Bonus);
			}
			else
			{
				EraseOther(obj as WorldObject);
			}
		}

		public override string ToString()
		{
			string result = "";

			result += "Enemies: " + m_enemies.Count + "\n";
			result += "Bonuses: " + m_bonuses.Count + "\n";
			result += "Other: " + m_other.Count + "\n";
			result += "Erase list: " + m_onErasing.Count;

			return result;
		}

		private List<Enemy> m_enemies = new List<Enemy>();
		private List<Bonus> m_bonuses = new List<Bonus>();
		private List<WorldObject> m_other = new List<WorldObject>();
		private List<object> m_onErasing = new List<object>();

		private IGameWorld world { get; set; }

		private void AddEnemy(Enemy enemy)
		{
			if (!enemy) return;
			AddObject(m_enemies, enemy);
		}
		private void AddBonus(Bonus bonus)
		{
			if (!bonus) return;
			AddObject(m_bonuses, bonus);
		}
		private void AddOther(WorldObject other)
		{
			if (!other) return;
			AddObject(m_other, other);
		}

		private void EraseEnemy(Enemy enemy)
		{
			EraseObject(m_enemies, enemy);
		}
		private void EraseBonus(Bonus bonus)
		{
			EraseObject(m_bonuses, bonus);
		}
		private void EraseOther(WorldObject other)
		{
			EraseObject(m_other, other);
		}

		private void AddObject<T>(List<T> list, T newObject) where T : WorldObject
		{
			if (list.Exists(obj => obj.Equals(newObject)))
			{
				return;
			}

			newObject.Init(world);
			list.Add(newObject);
		}
		private void EraseObject<T>(List<T> list, T eraseObject) where T : WorldObject
		{
			if (m_onErasing.Exists(element => element.Equals(eraseObject)))
			{
				return;
			}

			m_onErasing.Add(eraseObject);
			list.Remove(eraseObject);
			list.RemoveAll(element => element == null);
			m_onErasing.Remove(eraseObject);
		}
	}
}