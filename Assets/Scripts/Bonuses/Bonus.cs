using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame.GameUtils;

namespace MyGame
{
	public abstract class Bonus : WorldObject
	{
		public bool explosionStart { get; set; }
		public bool rotateOnStart { get; set; }
		public bool isMagnetic { get; protected set; }

		protected sealed override void OnAwakeEnd()
		{
			explosionStart = true;
			rotateOnStart = true;
			isMagnetic = false;
		}
		protected void Start()
		{
			OnStart();
			exitAllowed = true;
			world.SubscribeToMove(this);

			if (explosionStart) SetExplosionForce();
			if (rotateOnStart) SetRandomRotation();
		}
		protected virtual void OnStart() { }
		protected sealed override void OnExitFromWorld()
		{
		}
		protected sealed override void OnTrigger(Collider other)
		{
			if (other.gameObject.layer == GameWorld.WORLD_BOX_LAYER)
			{
				return;
			}

			OnRealize();
			Exit();
		}
		protected sealed override void PlayingUpdate()
		{
			if (isMagnetic) world.MoveToShip(this);
			UpdatePositionOnField();
		}
		protected abstract void OnRealize();

		private const float DELTA_FORCE = 200;
		private const float DELTA_ROTATION = 100;

		private void SetExplosionForce()
		{
			Vector3 force = Utils.RandomVect(-DELTA_FORCE, DELTA_FORCE);
			physicsBody.AddForce(force);
		}
		private void SetRandomRotation()
		{
			Vector3 rotation = Utils.RandomVect(-DELTA_ROTATION, DELTA_ROTATION);
			physicsBody.AddTorque(rotation);
		}
	}
}
