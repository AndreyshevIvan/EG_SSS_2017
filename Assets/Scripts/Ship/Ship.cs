using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame.GameUtils;

namespace MyGame.Hero
{
	public sealed class Ship : Body
	{
		public ShipMind mind { get; set; }
		public ParticleSystem engine { get { return m_engineParticles; } }
		public ShipProperties properties
		{
			set
			{
				mind.properties = value;
				health = maxHealth = 100;// value.health;
			}
		}

		public const float ENDING_CONTROLL_DURATION = 2;

		public void MoveTo(Vector3 newPosition)
		{
			Vector3 direction = (newPosition - position).normalized;
			m_smoothDir = Vector3.MoveTowards(m_smoothDir, direction, CONTROLL_SMOOTHING);
			direction = m_smoothDir;
			Vector3 movement = new Vector3(direction.x, 0, direction.z);
			velocity = movement * CONTROLL_SPEED;
			m_isMoved = true;
		}

		protected override void OnAwakeEnd()
		{
			mind = GetComponent<ShipMind>();
			ParticleSystem.MainModule engineMain = m_engineParticles.main;
			engineMain.simulationSpace = ParticleSystemSimulationSpace.Local;
			m_animator = GetComponent<Animator>();
			m_animator.Play(ROTATION_CLIP);
		}
		protected override void OnInitEnd()
		{
			healthBar = world.factory.GetBar(BarType.PLAYER_HEALTH);
			healthBar.SetValue(health);
			toDestroy.Add(healthBar.gameObject);
			touchDemage = int.MaxValue;
			m_isEraseOnDeath = false;
			distmantleAllowed = true;
		}

		protected override void OnPlaying()
		{
			ParticleSystem.MainModule engineMain = m_engineParticles.main;
			engineMain.simulationSpace = ParticleSystemSimulationSpace.World;
			m_animator.SetTrigger(ROTATION_TRIGGER);
			Utils.DoAfterTime(this, 1, () => { m_animator.enabled = false; });
		}
		protected override void OnEndGameplay()
		{
			gameObject.layer = (int)Layer.UNTOUCH;
			if (healthBar) healthBar.Close();

			Utils.DoAfterTime(this, ENDING_CONTROLL_DURATION, () =>
			{
				m_startEndAnimation = true;
				m_animator.enabled = true;
				m_animator.Play(ROTATION_CLIP);
				AddExtraListener(EndingAnimation);
			});
			Utils.DoAfterTime(this, ENDING_ANIM_DURATION, () =>
			{
				EraseExtraListener(EndingAnimation);
				m_animator.SetTrigger(ROTATION_TRIGGER);
			});
		}
		protected override void SmartPlayingUpdate()
		{
			if (m_startEndAnimation)
			{
				return;
			}

			position += velocity * Time.fixedDeltaTime;

			UpdatePositionOnField();
			UpdateRotation();
			UpdateMoveingSpeed();
		}

		protected override void DoAfterDemaged()
		{
			world.player.BeDemaged();
			healthBar.Fade(1, PlayerHealthBar.HP_BAR_FADE_DUR);
		}
		protected override void OnHealEnd()
		{
			if (isFull) healthBar.Fade(0, PlayerHealthBar.HP_BAR_FADE_DUR);
		}

		[SerializeField]
		ParticleSystem m_engineParticles;
		private Vector3 m_smoothDir;
		private bool m_isMoved = false;
		private bool m_startEndAnimation = false;
		Animator m_animator;

		private Vector3 velocity { get; set; }

		private const float MODEL_X_ANGLE = 180;

		private const float CONTROLL_SPEED = 80;
		private const float CONTROLL_SMOOTHING = 15;
		private const float CONTROLL_TILT = 1;
		private const float CONTROLL_MAX_ANGLE = 60;

		private const float ENDING_ANIM_DURATION = 5;
		private const float ENDING_ESCAPE_SPEED = 10;
		private const float ENDING_ANGLE_SPEED = 1;

		private const string ROTATION_TRIGGER = "EndRotate";
		private const string ROTATION_CLIP = "ShipStartRotation";

		private void UpdateRotation()
		{
			float zEuler = velocity.x * -CONTROLL_TILT;
			float zAngle = Mathf.Clamp(-zEuler, -CONTROLL_MAX_ANGLE, CONTROLL_MAX_ANGLE);
			physicsBody.rotation = Quaternion.Euler(0, MODEL_X_ANGLE, zAngle);
		}
		private void UpdateMoveingSpeed()
		{
			Vector3 currVelocity = velocity;
			velocity = (m_isMoved) ? currVelocity : Vector3.zero;
			m_isMoved = false;
		}
		private void EndingAnimation()
		{
			float step = Time.fixedDeltaTime * ENDING_ANGLE_SPEED;
			float zAngle = Mathf.MoveTowards(physicsBody.rotation.z, 0, step);
			physicsBody.rotation = Quaternion.Euler(0, MODEL_X_ANGLE, zAngle);
			position += new Vector3(0, 0, Time.fixedDeltaTime * ENDING_ESCAPE_SPEED);
		}
	}
}
