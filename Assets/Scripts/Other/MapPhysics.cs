using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public class MapPhysics : MonoBehaviour
	{
		public GameObject m_shipExplosion;
		public Star m_star;

		public Transform ground { get; set; }
		public Body shipBody { get; set; }
		public ShipMind shipMind { get; set; }
		public Vector3 shipPosition
		{
			get { return shipBody.position; }
			set { shipBody.position = value; }
		}

		public const float FLY_HEIGHT = 1;

		public void AddEnemy(Enemy enemy)
		{
			enemy.gameMap = this;
			Vector3 position = enemy.transform.position;
			position.z += 10;
			position.y = FLY_HEIGHT;
			enemy.transform.position = position;
			enemy.transform.SetParent(transform);
		}
		public void AddPlayerBullet(GameObject bullet)
		{
		}
		public void AddEnemyBullet(GameObject bullet)
		{
		}

		public Vector3 GetNearestEnemy(Vector3 point)
		{
			return Vector3.zero;
		}

		public void EraseEnemy(Enemy enemy)
		{
			List<Rigidbody> bodies = null;
			bodies = Utils.ToList(enemy.GetComponentsInChildren<Rigidbody>());
			bodies.Remove(enemy.GetComponent<Rigidbody>());

			foreach (Rigidbody body in bodies)
			{
				GameObject obj = body.gameObject;
				body.transform.SetParent(ground);
				body.useGravity = true;
				obj.layer = GARBAGE_LAYER;
				body.AddForce(Utils.RandomVect(-300, 300));
				//MeshRenderer renderer = obj.GetComponentInChildren<MeshRenderer>();
				//renderer.material = m_garbageMaterial;
			}

			CreateShipExplosion(enemy.position);
			SpawnStars(enemy.starsCount, enemy.position);
			Destroy(enemy);
		}
		public void EraseEnemyBullet(GameObject bullet)
		{
		}
		public void ErasePlayerBullet(GameObject bullet)
		{
		}
		public void EraseStar(Star star)
		{
			Destroy(star.gameObject);
		}

		public void SpawnStars(byte starsCount, Vector3 position)
		{
			position.y = 1;

			while (starsCount > 0)
			{
				Star newStar = Instantiate(m_star, ground);
				newStar.position = position;
				newStar.gameMap = this;
				starsCount--;
			}
		}
		public void MoveToShip(Body body, bool useShipMagnetic = true)
		{
			float distance = Vector3.Distance(body.position, shipPosition);
			if (distance > shipMind.magneticDistance)
			{
				return;
			}

			float factor = (useShipMagnetic) ? shipMind.magnetic : 1;
			float distanceFactor = shipMind.magneticDistance / distance;
			body.position = Vector3.MoveTowards(
				body.position,
				shipPosition,
				factor * distanceFactor * MAGNETIC_SPEED * Time.deltaTime);
		}
		public void MoveWithMap(Body body)
		{
			body.transform.SetParent(ground);
		}

		protected void FixedUpdate()
		{
			Vector3 movement = new Vector3(0, 0, -MAP_MOVE_SPEED) * Time.deltaTime;
			ground.position = ground.position + movement;
		}

		private const float MAGNETIC_SPEED = 2;
		private const float MAP_MOVE_SPEED = 1.6f;
		private const int GARBAGE_LAYER = 12;

		private void CreateShipExplosion(Vector3 position)
		{
			GameObject explosion = Instantiate(m_shipExplosion);
			explosion.transform.position = position;
		}
	}
}
