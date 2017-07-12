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
		public Vector3 shipPosition { get { return ship.position; } }

		public IGameplay gameplay { get; set; }
		public bool isMapStart { get { return gameplay.isMapStart; } }
		public bool isPaused { get { return gameplay.isPaused; } }
		public bool isGameEnd { get { return gameplay.isGameEnd; } }
		public bool isWin { get { return gameplay.isWin; } }
		public bool isPlaying { get { return gameplay.isPlaying; } }
		public Player player { get { return gameplay.player; } }

		public Transform sky { get { return map.skyObjects; } }
		public Transform ground { get { return map.groundObjects; } }
		public BoundingBox box { get; private set; }
		public float visiblePosition { get; private set; }
		public float time { get { return map.time; } }
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
			m_targetTimeScale = (isModeOn) ? SLOW_TIMESCALE : 1;

			if (isModeOn != m_lastModeType)
			{
				m_deltaScale = Mathf.Abs(m_targetTimeScale - Time.timeScale);
				m_lastModeType = isModeOn;
			}
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

			Vector3 deathPos = ship.position;
			List<Pair<BonusType, int>> playerStars = new List<Pair<BonusType, int>>();
			int stars = player.stars > DEATH_STARS_COUNT ? DEATH_STARS_COUNT : player.stars;
			playerStars.Add(Pair<BonusType, int>.Create(BonusType.STAR, stars));
			Remove(ship);
			Destroy(ship.gameObject);
			OpenBonuses(playerStars, deathPos);
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
		private Camera m_camera;
		private bool m_lastModeType = false;
		private bool m_isDismantle = true;
		private float m_deltaScale = 1 - SLOW_TIMESCALE;
		private float m_targetTimeScale = 1;
		[SerializeField]
		private Material m_garbageMaterial;

		private const float MAGNETIC_SPEED = 2;
		private const float SLOW_TIMESCALE = 0.2f;
		private const float DISMANTLE_FORCE = 200;
		private const float NORMAL_TIME_SCALE = 1;
		private const float NORMAL_DT = 0.02f;
		private const float SLOWMO_DT = 0.01f;
		private const int GARBAGE_LAYER = 12;
		private const int DEATH_STARS_COUNT = 25;

		private void Awake()
		{
			box = GameData.box;
			container = new WorldContainer();
			m_camera = Camera.main;
			container.Init(this);
		}

		private void Start()
		{
			InitTempPlayer();
			InitVisibleArea();
		}
		private void InitTempPlayer()
		{
			player.onDemaged = () => { Debug.Log("Demaged"); };
			player.onLossEnemy = () => { Debug.Log("LOSS"); };
		}
		private void InitVisibleArea()
		{
		}

		private void FixedUpdate()
		{
			UpdateSlowmode();
		}
		private void UpdateSlowmode()
		{
			float step = Time.fixedDeltaTime / GameplayUI.SLOWMO_OPEN_DUR * m_deltaScale;
			Time.timeScale = Mathf.MoveTowards(Time.timeScale, m_targetTimeScale, step);
			Time.fixedDeltaTime = (Time.timeScale != NORMAL_TIME_SCALE) ? SLOWMO_DT : NORMAL_DT;
		} 

		private void OnTriggerExit(Collider other)
		{
			WorldObject obj = other.GetComponent<WorldObject>();

			if (obj && obj.exitAllowed)
			{
				obj.ExitFromWorld();
				Remove(obj);
				return;
			}

			Destroy(other.gameObject);
		}
		private void OpenObject(WorldObject obj)
		{
			if (!obj)
			{
				return;
			}

			player.AddPoints(obj.points);
			OpenBonuses(obj.bonuses, obj.position);
		}
		private void OpenBonuses(List<Pair<BonusType, int>> list, Vector3 spawn)
		{
			if (list == null)
			{
				return;
			}

			list.ForEach(bonus =>
			{
				Utils.DoAnyTimes(bonus.value, () =>
				{
					Bonus newBonus = factory.GetBonus(bonus.key);
					newBonus.position = spawn;
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
		Ship ship { get; }
		IFactory factory { get; }
		Player player { get; }
		Vector3 shipPosition { get; }
		Transform sky { get; }
		Transform ground { get; }
		BoundingBox box { get; }
		float visiblePosition { get; }
		float time { get; }

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