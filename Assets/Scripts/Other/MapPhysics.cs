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

		public GameObject m_shipExplosion;
		public Star m_star;
		public List<CurvySpline> m_splines;

		public Transform ground { get; set; }
		public Transform sky { get; set; }
		public Body shipBody { get; set; }
		public ShipMind shipMind { get; set; }
		public Vector3 shipPosition
		{
			get { return shipBody.position; }
			set { shipBody.position = value; }
		}
		public float offset { get; set; }
		public bool isSleep { get; set; }

		public const float FLY_HEIGHT = 4;
		public const float SPAWN_OFFSET = 1.2f;
		public const int DELETE_LAYER = 31;

		public void AddEnemy(Enemy enemy)
		{
			enemy.world = this;
		}
		public void AddAmmo(Ammo ammo)
		{
			ammo.world = this;
			ammo.transform.SetParent(sky);
		}
		public void AddStars(byte starsCount, Vector3 position)
		{
			position.y = FLY_HEIGHT;

			while (starsCount > 0)
			{
				Star newStar = Instantiate(m_star, ground);
				newStar.position = position;
				newStar.world = this;
				starsCount--;
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
				body.AddForce(Utils.RandomVect(-300, 300));
			}

			CreateShipExplosion(enemy.position);
			AddStars(enemy.starsCount, enemy.position);
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
		public void EraseStar(Star star)
		{
			Destroy(star.gameObject);
		}

		public Vector3 GetNearestEnemy(Vector3 point)
		{
			return Vector3.zero;
		}
		public CurvySpline GetSpline()
		{
			int index = UnityEngine.Random.Range(0, m_splines.Count);
			return m_splines[index];
		}

		public void SetSlowMode(bool isModeOn)
		{
			Time.timeScale = (isModeOn) ? 1 : 0.5f;
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

			if (!shipBody.isLive)
			{
				onPlayerDeath();
				isSleep = true;
			}
		}

		private BoundingBox m_gameBox;

		private const float MAGNETIC_SPEED = 2;
		private const float MAP_MOVE_SPEED = 1.7f;
		private const int GARBAGE_LAYER = 12;

		private void CreateShipExplosion(Vector3 position)
		{
			GameObject explosion = Instantiate(m_shipExplosion);
			explosion.transform.position = position;
		}
		private void MoveGround()
		{
			float movement = MAP_MOVE_SPEED * Time.deltaTime * Time.timeScale;
			ground.transform.Translate(new Vector3(0, 0, -movement));
			offset += movement;
		}
	}
}
