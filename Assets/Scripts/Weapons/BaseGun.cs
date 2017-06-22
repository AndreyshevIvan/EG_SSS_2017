using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public sealed class BaseGun : Gun
	{
		public SimpleBullet m_ammo;
		public Transform m_leftSpawn;
		public Transform m_rightSpawn;

		public override void Modify()
		{
		}

		protected float bulletsSpeed { get; set; }

		protected override void OnInit()
		{
			isTimerWork = true;
			coldown = 2;
			bulletsSpeed = 16;
		}
		protected override void OnShoot()
		{
			SpawnBullet(m_leftSpawn);
			SpawnBullet(m_rightSpawn);
		}

		private void SpawnBullet(Transform spawn)
		{
			SimpleBullet bullet = Instantiate(m_ammo);
			bullet.position = spawn.position;
			bullet.speed = bulletsSpeed;
			mapPhysics.AddPlayerBullet(bullet);
			bullet.Start();
		}
	}
}