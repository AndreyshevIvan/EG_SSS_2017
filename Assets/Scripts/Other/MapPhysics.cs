using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public abstract class MapPhysics : MonoBehaviour
	{
		public GameObject m_shipExplosion;
		public Star m_star;
		public Material m_garbageMaterial;

		public Transform m_moveParent;
		public Transform m_bulletsParent;

		public int m_garbageLayer;

		public Vector3 shipPosition { get { return shipBody.position; } }
		public Body shipBody { get; protected set; }

		public void AddEnemy(Enemy enemy)
		{
			enemy.gameMap = this;
		}
		public void AddPlayerBullet(GameObject bullet)
		{
		}
		public void AddEnemyBullet(GameObject bullet)
		{
			bullet.transform.SetParent(m_bulletsParent);
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
				body.transform.SetParent(m_moveParent);
				body.useGravity = true;
				obj.layer = m_garbageLayer;
				body.AddForce(Utils.RandomVect(-300, 300));
				MeshRenderer renderer = obj.GetComponentInChildren<MeshRenderer>();
				renderer.material = m_garbageMaterial;
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
			AddPoints(1);
			Destroy(star.gameObject);
		}

		public abstract void AddPoints(ushort pointsCount);
		public void SpawnStars(byte starsCount, Vector3 position)
		{
			position.y = 1;

			while (starsCount > 0)
			{
				Star newStar = Instantiate(m_star, m_moveParent);
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
			body.transform.SetParent(m_moveParent);
		}

		protected ShipMind shipMind { get; set; }
			
		protected void FixedUpdate()
		{
			Vector3 movement = new Vector3(0, 0, -MAP_MOVE_SPEED) * Time.deltaTime;
			m_moveParent.position = m_moveParent.position + movement;
		}

		private const float MAGNETIC_SPEED = 2;
		private const float MAP_MOVE_SPEED = 1.6f;

		private void CreateShipExplosion(Vector3 position)
		{
			GameObject explosion = Instantiate(m_shipExplosion);
			explosion.transform.position = position;
		}
	}
}
