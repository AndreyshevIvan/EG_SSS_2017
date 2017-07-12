using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MyGame.GameUtils;
using System.Collections;

namespace MyGame
{
	public class PlayerBomb : Body
	{
		public Transform parent { get; set; }

		protected override void OnAwakeEnd()
		{
			m_transform = GetComponent<Transform>();
			target = new Vector3(TARGET_RADIUS, TARGET_RADIUS, TARGET_RADIUS);
			isImmortal = true;
			touchDemage = DEMAGE;
			gameObject.layer = (int)Layer.UNTOUCH;
		}
		protected override void SmartPlayingUpdate()
		{
			if (!parent)
			{
				world.Remove(this);
				return;
			}

			position = parent.position;

			if (scale.x >= TARGET_RADIUS)
			{
				gameObject.layer = (int)Layer.PLAYER_BULLET;
				StartCoroutine(DestroyAfterBoom());
				return;
			}

			scale = new Vector3(radius, radius, radius);
			m_timer += Time.fixedDeltaTime;
		}

		private Transform m_transform;
		private float m_timer;

		private Vector3 scale
		{
			get
			{
				return m_transform.localScale;
			}
			set
			{
				m_transform.localScale = value;
			}
		}
		private Vector3 target { get; set; }
		private float radius { get { return TARGET_RADIUS * m_timer / CAST_DURATION; } }

		private const float CAST_DURATION = 0.8f;
		private const float TARGET_RADIUS = 20;
		private const int DEMAGE = 1;

		private IEnumerator DestroyAfterBoom()
		{
			yield return new WaitForFixedUpdate();
			distmantleAllowed = true;
			world.Remove(this);
		}
	}
}
