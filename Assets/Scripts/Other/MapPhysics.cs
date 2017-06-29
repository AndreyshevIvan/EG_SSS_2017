using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FluffyUnderware.Curvy;

namespace MyGame.World
{
	public class MapPhysics : MonoBehaviour
	{
		public EventDelegate onPlayerDeath;

		public Material m_garbageMaterial;

		public Factories factories { get; set; }
		public Transform ground { get; set; }
		public Transform sky { get; set; }
		public Ship ship { get; set; }
		public ShipMind shipMind { get { return ship.mind; } }
		public Vector3 shipPosition
		{
			get { return ship.position; }
			set { ship.position = value; }
		}
		public float offset { get; set; }
		public bool isSleep { get; set; }
		public IPlayerBar playerBar { get; set; }

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
			ammo.transform.SetParent(sky);
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
				Bonus newBonus = Instantiate(bonus, ground);
				newBonus.explosionStart = true;
				newBonus.position = position;
				newBonus.Init(this);
				count--;
			}
		}

		public void EraseEnemyByKill(Enemy enemy)
		{
			CreateExplosion(enemy.deathExplosion, enemy.position);
			AddBonus(factories.bonuses.star, enemy.starsCount, enemy.position);
			AddBonus(enemy.bonus, 1, enemy.position);
			m_player.points += enemy.points;
			playerBar.points = m_player.points;
			Dismantle(enemy.transform);
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

			if (isModeOn != lastModeType)
			{
				deltaScale = Mathf.Abs(target - Time.timeScale);
				lastModeType = isModeOn;
			}

			float step = Time.deltaTime / GameplayUI.SLOWMO_CHANGE_TIME * deltaScale;
			Time.timeScale = Mathf.MoveTowards(Time.timeScale, target, step);
		}

		public void MoveToShip(Body body, bool useShipMagnetic = true)
		{
			float distance = Vector3.Distance(body.position, shipPosition);
			if (distance > shipMind.magnetDistance)
			{
				return;
			}

			float factor = (useShipMagnetic) ? shipMind.magnetic : 1;
			float distanceFactor = shipMind.magnetDistance / distance;
			float movement = factor * distanceFactor * MAGNETIC_SPEED * Time.fixedDeltaTime;
			body.position = Vector3.MoveTowards(
				body.position,
				shipPosition,
				movement);
		}
		public void SubscribeToMove(Body body)
		{
			body.transform.SetParent(ground);
		}
		public void OnTriggerExit(Collider other)
		{
			Body body = other.GetComponent<Body>();
			if (body == null)
			{
				return;
			}
			body.OnExitFromWorld();
			Destroy(body);
		}
		public void Cleanup()
		{
			List<Component> toDelete = new List<Component>();
			toDelete.AddRange(Utils.GetChilds<Component>(sky));
			toDelete.AddRange(Utils.GetChilds<Component>(ground));
			toDelete.ForEach(element => Destroy(element.gameObject));
		}

		protected void Awake()
		{
			m_gameBox = GameData.mapBox;
			isSleep = true;
			offset = 0;
		}
		protected void FixedUpdate()
		{
			if (isSleep)
			{
				return;
			}

			MoveGround();

			if (!ship.isLive)
			{
				isSleep = true;
				onPlayerDeath();
				Dismantle(ship.transform);
			}
		}

		private List<Body> m_toEarse = new List<Body>();
		private BoundingBox m_gameBox;
		private TempPlayer m_player;
		private bool lastModeType = false;
		private float deltaScale = 1 - SLOW_TIMESCALE;

		private const float MAGNETIC_SPEED = 2;
		private const float MAP_MOVE_SPEED = 1.7f;
		private const float SLOW_TIMESCALE = 0.2f;
		private const float DISMANTLE_FORCE = 300;
		private const int GARBAGE_LAYER = 12;

		private void CreateExplosion(ParticleSystem explosion, Vector3 position)
		{
			if (explosion == null)
			{
				return;
			}

			ParticleSystem explosionObject = Instantiate(explosion);
			explosionObject.transform.position = position;
			explosionObject.transform.SetParent(sky);
		}
		private void MoveGround()
		{
			float movement = MAP_MOVE_SPEED * Time.fixedDeltaTime;
			ground.transform.Translate(new Vector3(0, 0, -movement));
			offset += movement;
		}
		private void Dismantle(Transform subject)
		{
			List<Rigidbody> bodies = Utils.GetChilds<Rigidbody>(subject);

			foreach (Rigidbody body in bodies)
			{
				GameObject obj = body.gameObject;
				body.transform.SetParent(ground);
				body.useGravity = true;
				obj.layer = GARBAGE_LAYER;
				body.AddForce(Utils.RandomVect(-DISMANTLE_FORCE, DISMANTLE_FORCE));
				Renderer renderer = obj.GetComponentInChildren<Renderer>();
				if (renderer != null) renderer.material = m_garbageMaterial;
			}
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
}
