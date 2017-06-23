using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public abstract class MapPhysics : MonoBehaviour
	{
		public void AddStar(GameObject star)
		{
		}
		public void AddEnemy(GameObject enemy)
		{
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

		public void EraseEnemy(GameObject enemy)
		{
		}
		public void EraseStar(GameObject star)
		{
		}
		public void EraseEnemyBullet(GameObject bullet)
		{
		}
		public void ErasePlauerBullet(GameObject bullet)
		{
		}

		public abstract void AddPoints(ushort pointsCount);
		public void SpawnStars(byte starsCount)
		{
		}

		protected void FixedUpdate()
		{
		}
	}
}
