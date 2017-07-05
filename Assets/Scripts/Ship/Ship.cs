using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Hero
{
	public sealed class Ship : Body
	{
		public ShipMind mind { get; set; }
		public ShipProperties properties
		{
			set
			{
				mind.properties = value;
				health = maxHealth = 100;//value.health;
			}
		}
		public IPlayerBar bar { get; set; }

		public void MoveTo(Vector3 newPosition)
		{
			Vector3 direction = (newPosition - position).normalized;
			m_smoothDir = Vector3.MoveTowards(m_smoothDir, direction, SMOOTHING);
			direction = m_smoothDir;
			Vector3 movement = new Vector3(direction.x, 0, direction.z);
			velocity = movement * SPEED;
			m_isMoved = true;
		}

		protected override void OnEnd()
		{
			if (healthBar) healthBar.Close();
		}
		protected override void OnAwakeEnd()
		{
			mind = GetComponent<ShipMind>();
		}
		protected override void OnInitEnd()
		{
			roadController.Spline = world.factory.GetRoad(RoadType.PLAYER);
			healthBar = world.factory.GetBar(BarType.PLAYER_HEALTH);
			healthBar.SetValue(healthPart);
			touchDemage = int.MaxValue;
			isEraseOnDeath = false;
			mind.bar = bar;
		}
		protected override void PlayingUpdate()
		{
			position += velocity * Time.fixedDeltaTime;

			UpdatePositionOnField();
			UpdateRotation();
			UpdateMoveingSpeed();

			healthBar.isFadable = maxHealth == health;
		}
		protected override void AfterMatchUpdate()
		{
			float step = Time.fixedDeltaTime * AFTER_GAME_ANGLE_SPEED;
			float zAngle = Mathf.MoveTowards(physicsBody.rotation.z, 0, step);
			physicsBody.rotation = Quaternion.Euler( 0, X_ANGLE, zAngle);
			position += new Vector3(0, 0, Time.fixedDeltaTime * ESCAPE_SPEED);
		}
		protected override void DoAfterDemaged()
		{
			healthBar.SetValue(healthPart);
			world.player.Demaged();
		}

		private Vector3 m_smoothDir;
		private bool m_isMoved = false;

		private Vector3 velocity { get; set; }

		private const float SPEED = 80;
		private const float ESCAPE_SPEED = 15;
		private const float AFTER_GAME_ANGLE_SPEED = 1;
		private const float SMOOTHING = 15;
		private const float TILT = 2;
		private const float X_ANGLE = 180;
		private const float MAX_VELOCITY_ANGLE = 80;

		private void UpdateRotation()
		{
			float zEuler = velocity.x * -TILT;
			float zAngle = Mathf.Clamp(-zEuler, -MAX_VELOCITY_ANGLE, MAX_VELOCITY_ANGLE);
			physicsBody.rotation = Quaternion.Euler(0, X_ANGLE, zAngle);
		}
		private void UpdateMoveingSpeed()
		{
			Vector3 currVelocity = velocity;
			velocity = (m_isMoved) ? currVelocity : Vector3.zero;
			m_isMoved = false;
		}
	}
}
