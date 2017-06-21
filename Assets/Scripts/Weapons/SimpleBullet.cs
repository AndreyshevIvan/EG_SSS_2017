using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public sealed class SimpleBullet : Ammo
	{
		public override void Start()
		{
			velocity = new Vector3(0, 0, m_speed);
		}
		public override void Modify()
		{
		}

		protected override void OnAwake()
		{
		}
		protected override void OnUpdate()
		{
		}
		protected override void OnChangeLevel()
		{
		}

		private float m_speed = 10;
	}
}
