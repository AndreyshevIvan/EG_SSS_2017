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
		public Transform m_garbageParent;
		public Transform m_starsParent;
		public int m_garbageLayer;

		public void AddEnemy(Enemy enemy)
		{
			enemy.gameMap = this;
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
			List<Rigidbody> bodies = new List<Rigidbody>(enemy.GetComponentsInChildren<Rigidbody>());

			bodies.Remove(enemy.GetComponent<Rigidbody>());

			foreach (Rigidbody body in bodies)
			{
				body.transform.SetParent(m_garbageParent);
				body.useGravity = true;
				body.gameObject.layer = m_garbageLayer;
				body.AddExplosionForce(400, enemy.position, 100);
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
		}

		public abstract void AddPoints(ushort pointsCount);
		public void SpawnStars(byte starsCount, Vector3 position)
		{
			position.y = 1;

			while (starsCount > 0)
			{
				Star newStar = Instantiate(m_star, m_starsParent);
				newStar.position = position;
				starsCount--;
			}
		}

		protected void FixedUpdate()
		{
		}

		private void CreateShipExplosion(Vector3 position)
		{
			GameObject explosion = Instantiate(m_shipExplosion);
			explosion.transform.position = position;
		}
	}
}
