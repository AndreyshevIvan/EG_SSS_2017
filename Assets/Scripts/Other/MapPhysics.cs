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
		public GameObject m_shipExplosion;
		public Star m_star;

		public Material m_garbage;
		public List<CurvySpline> m_splines;

		public Transform ground { get; set; }
		public Body shipBody { get; set; }
		public ShipMind shipMind { get; set; }
		public Vector3 shipPosition
		{
			get { return shipBody.position; }
			set { shipBody.position = value; }
		}

		public const float FLY_HEIGHT = 4;
		public const int DELETE_LAYER = 31;

		public void AddEnemy(Enemy enemy)
		{
			enemy.world = this;
			enemy.transform.SetParent(transform);
		}
		public void AddAmmo(Ammo ammo)
		{
			ammo.world = this;
		}

		public Vector3 GetNearestEnemy(Vector3 point)
		{
			return Vector3.zero;
		}

		public void EraseEnemyByKill(Enemy enemy)
		{
			/*
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
				//if (renderer != null) renderer.material = m_garbage;
			}
			*/
			CreateShipExplosion(enemy.position);
			SpawnStars(enemy.starsCount, enemy.position);
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

		public CurvySpline GetSpline()
		{
			int index = UnityEngine.Random.Range(0, m_splines.Count);
			return m_splines[index];
		}
		public void SpawnStars(byte starsCount, Vector3 position)
		{
			position.y = MapPhysics.FLY_HEIGHT;

			while (starsCount > 0)
			{
				Star newStar = Instantiate(m_star, ground);
				newStar.position = position;
				newStar.world = this;
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
