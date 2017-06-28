using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FluffyUnderware.Curvy;

namespace MyGame
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
		public TempPlayer player { get; protected set; }

		public const float FLY_HEIGHT = 4;
		public const float SPAWN_OFFSET = 1.2f;
		public const int WORLD_BOX_LAYER = 31;

		public void AddEnemy(Enemy enemy)
		{
			enemy.world = this;
		}
		public void AddAmmo(Ammo ammo)
		{
			ammo.world = this;
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
				newBonus.world = this;
				count--;
			}
		}

		public void EraseEnemyByKill(Enemy enemy)
		{
			List<Rigidbody> bodies = Utils.GetChilds<Rigidbody>(enemy);

			foreach (Rigidbody body in bodies)
			{
				GameObject obj = body.gameObject;
				body.transform.SetParent(ground);
				body.useGravity = true;
				obj.layer = GARBAGE_LAYER;
				body.AddForce(Utils.RandomVect(-EXPLOSION_FORCE, EXPLOSION_FORCE));
				Renderer renderer = obj.GetComponentInChildren<Renderer>();
				if (renderer != null) renderer.material = m_garbageMaterial;
			}

			CreateExplosion(enemy);
			AddBonus(factories.bonuses.star, enemy.starsCount, enemy.position);
			AddBonus(enemy.bonus, 1, enemy.position);
			Destroy(enemy.gameObject);
		}
		public void EraseEnemy(Enemy enemy)
		{
			Destroy(enemy.gameObject);
		}
		public void EraseAmmo(Ammo ammo)
		{
			Destroy(ammo.gameObject);
		}
		public void EraseBonus(Bonus bonus)
		{
			Destroy(bonus.gameObject);
		}

		public Vector3 GetNearestEnemy(Vector3 point)
		{
			return Vector3.zero;
		}

		public void SetSlowMode(bool isModeOn)
		{
			float dt = Time.fixedDeltaTime;
			float target = (isModeOn) ? SLOW_TIMESCALE : 1;
			float step = dt / GameplayUI.SLOW_TIME * (1 - SLOW_TIMESCALE);

			Time.timeScale = Mathf.MoveTowards(Time.timeScale, target, dt);
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
			float movement = factor * distanceFactor * MAGNETIC_SPEED * Time.deltaTime;
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
			if (body != null)
			{
				body.OnDeleteByWorld();
				return;
			}
			Destroy(other.gameObject);
		}
		public void Cleanup()
		{
			List<Component> toDeleteObjects = new List<Component>();
			toDeleteObjects.AddRange(Utils.GetChilds<Component>(sky));
			toDeleteObjects.AddRange(Utils.GetChilds<Component>(ground));
			toDeleteObjects.ForEach(element => Destroy(element.gameObject));
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
			}
		}

		private BoundingBox m_gameBox;
		private List<Enemy> m_enemies;

		private const float MAGNETIC_SPEED = 2;
		private const float MAP_MOVE_SPEED = 1.7f;
		private const float SLOW_TIMESCALE = 0.2f;
		private const float EXPLOSION_FORCE = 300;
		private const int GARBAGE_LAYER = 12;

		private void CreateExplosion(Body body)
		{
			if (body.deathExplosion == null)
			{
				return;
			}

			GameObject explosion = Instantiate(body.deathExplosion.gameObject, sky);
			explosion.transform.position = body.position;
		}
		private void MoveGround()
		{
			float movement = MAP_MOVE_SPEED * Time.deltaTime * Time.timeScale;
			ground.transform.Translate(new Vector3(0, 0, -movement));
			offset += movement;
		}
	}
}
