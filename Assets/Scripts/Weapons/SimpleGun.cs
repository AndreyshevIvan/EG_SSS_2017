using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public sealed class SimpleGun : Gun
	{
		public Transform m_leftSpawn;
		public Transform m_rightSpawn;

		protected override void OnModificateGun()
		{
		}
		protected override void OnChangeGunLevel()
		{
		}
		protected override void OnShoot()
		{
			SpawnBullet(m_leftSpawn);
			SpawnBullet(m_rightSpawn);
		}

		private void SpawnBullet(Transform spawn)
		{
			Ammo bullet = Instantiate(m_ammo, spawn);
			bullet.transform.position = spawn.position;
			bullet.Start();
		}
	}
}
