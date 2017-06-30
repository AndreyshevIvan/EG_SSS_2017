using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public sealed class PlayerBaseGun : Gun
	{
		public SimpleBullet m_ammo;

		protected override void DoAfterInit()
		{
			coldown = 0.3f;
		}
		protected override void Shoot()
		{
			SimpleBullet bullet = Instantiate(m_ammo);
			bullet.position = transform.position;
			Vector3 target = transform.position + Vector3.forward;
			bullet.Init(target, 20, 7);
			bullet.StartAmmo();
			world.AddAmmo(bullet);
		}
	}
}