using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FluffyUnderware.Curvy;
using MyGame.Hero;

namespace MyGame
{
	public partial class MapPhysics : MonoBehaviour, IMapPhysics
	{
		public Material m_garbageMaterial;

		public Factories factories { get; set; }
		public Transform groundObjects { get; set; }
		public Transform skyObjects { get; set; }
		public Transform ground { get; set; }
		public Ship ship { get; set; }
		public ShipMind shipMind { get { return ship.mind; } }
		public bool isPlayerLive { get; private set; }

		public float offset { get; set; }

		public IPlayerBar playerBar { get; set; }
		public IGameplay gameplay { get; set; }

		public const float FLY_HEIGHT = 4;
		public const float SPAWN_OFFSET = 1.2f;
		public const int WORLD_BOX_LAYER = 31;
		public const int MODIFICATION_COUNT = 7;

		public void AddEnemy(Enemy enemy)
		{
			enemy.Init(this);
		}
		public void AddAmmo(Ammo ammo)
		{
			ammo.Init(this);
			ammo.transform.SetParent(skyObjects);
		}
		public void AddBonus(Bonus bonus, byte count, Vector3 position)
		{
			if (bonus == null)
			{
				return;
			}

			position.y = FLY_HEIGHT;

			while (count > 0)
			{
				Bonus newBonus = Instantiate(bonus, groundObjects);
				newBonus.explosionStart = true;
				newBonus.position = position;
				newBonus.Init(this);
				count--;
			}
		}

		public void EraseEnemyByKill(Enemy enemy)
		{
			AddBonus(factories.bonuses.star, enemy.starsCount, enemy.transform.position);
			AddBonus(enemy.bonus, 1, enemy.transform.position);
			m_player.points += enemy.points;
			playerBar.points = m_player.points;
			Dismantle(enemy);
			EraseBody(enemy);
		}
		public void EraseEnemy(Enemy enemy)
		{
			EraseBody(enemy);
		}
		public void EraseAmmo(Ammo ammo)
		{
			EraseBody(ammo);
		}
		public void EraseBonus(Bonus bonus)
		{
			EraseBody(bonus);
		}

		public Vector3 GetNearestEnemy(Vector3 point)
		{
			return Vector3.zero;
		}

		public void SetSlowMode(bool isModeOn)
		{
			float target = (isModeOn) ? SLOW_TIMESCALE : 1;

			if (isModeOn != m_lastModeType)
			{
				deltaScale = Mathf.Abs(target - Time.timeScale);
				m_lastModeType = isModeOn;
			}

			float step = Time.deltaTime / GameplayUI.SLOWMO_CHANGE_TIME * deltaScale;
			Time.timeScale = Mathf.MoveTowards(Time.timeScale, target, step);
		}

		public void SubscribeToMove(Body body)
		{
			body.transform.SetParent(groundObjects);
		}
		public void MoveToShip(Body body, bool useShipMagnetic = true)
		{
			float distance = Vector3.Distance(body.transform.position, ship.transform.position);
			if (distance > shipMind.magnetDistance)
			{
				return;
			}

			float factor = (useShipMagnetic) ? shipMind.magnetic : 1;
			float distanceFactor = shipMind.magnetDistance / distance;
			float movement = factor * distanceFactor * MAGNETIC_SPEED * Time.fixedDeltaTime;
			body.position = Vector3.MoveTowards(
				body.transform.position,
				ship.transform.position,
				movement);
		}
		public void KillPlayer()
		{
			Dismantle(ship);
			Destroy(ship);
		}

		public void OnTriggerExit(Collider other)
		{
			Body body = other.GetComponent<Body>();
			if (body == null) return;

			body.OnExitFromWorld();
			Destroy(body);
		}
		public void Cleanup()
		{
			List<Component> toDelete = new List<Component>();
			toDelete.AddRange(Utils.GetChilds<Component>(skyObjects));
			toDelete.AddRange(Utils.GetChilds<Component>(groundObjects));
			toDelete.ForEach(element => Destroy(element.gameObject));
		}

		private List<Body> m_toEarse = new List<Body>();
		private BoundingBox m_gameBox;
		private TempPlayer m_player;
		private bool m_lastModeType = false;
		private float deltaScale = 1 - SLOW_TIMESCALE;

		private const float MAGNETIC_SPEED = 2;
		private const float MAP_MOVE_SPEED = 1.7f;
		private const float SLOW_TIMESCALE = 0.2f;
		private const float DISMANTLE_FORCE = 300;
		private const int GARBAGE_LAYER = 12;

		private void Awake()
		{
			m_gameBox = GameData.mapBox;
			offset = 0;
		}
		private void FixedUpdate()
		{
			isPlayerLive = ship.isLive;

			if (!gameplay.isMapSleep && gameplay.isPlaying)
			{
				MoveGround();
			}
		}
		private void CreateExplosion(ParticleSystem explosion, Vector3 position)
		{
			if (explosion == null)
			{
				return;
			}

			ParticleSystem explosionObject = Instantiate(explosion);
			explosionObject.transform.position = position;
			explosionObject.transform.SetParent(skyObjects);
		}
		private void MoveGround()
		{
			float movement = MAP_MOVE_SPEED * Time.fixedDeltaTime;
			ground.transform.Translate(new Vector3(0, 0, -movement));
			offset += movement;
		}
		private void Dismantle(Body subject)
		{
			if (!subject)
			{
				return;
			}

			CreateExplosion(subject.deathExplosion, subject.transform.position);
			List<Rigidbody> bodies = Utils.GetChilds<Rigidbody>(subject.transform);
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
		}
		private void EraseBody(Body body)
		{
			if (m_toEarse.Find(erasible => erasible == body))
			{
				return;
			}

			m_toEarse.Add(body);
			body.OnErase();
			m_toEarse.Remove(body);
			Destroy(body.gameObject);
		}
	}

	public interface IMapPhysics
	{
		Factories factories { get; }
		Ship ship { get; }
		ShipMind shipMind { get; }
		IGameplay gameplay { get; }
		bool isPlayerLive { get; }

		void AddEnemy(Enemy enemy);
		void AddAmmo(Ammo ammo);
		void AddBonus(Bonus bonus, byte count, Vector3 position);

		Vector3 GetNearestEnemy(Vector3 point);

		void EraseEnemyByKill(Enemy enemy);
		void EraseEnemy(Enemy enemy);
		void EraseAmmo(Ammo ammo);
		void EraseBonus(Bonus bonus);

		void SubscribeToMove(Body body);
		void MoveToShip(Body body, bool useShipMagnetic = true);
	}
}