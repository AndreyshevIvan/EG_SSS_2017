using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public abstract class Bonus : WorldObject
	{
		public bool isMagnetic { get; set; }
		public bool explosionStart { get; set; }

		protected void Start()
		{
			world.SubscribeToMove(this);
			SetRandomRotation();

			if (explosionStart)
			{
				SetExplosionForce();
			}
		}

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
			world.Remove(this, true);
		}
		protected sealed override void PlayingUpdate()
		{
			if (isMagnetic) world.MoveToShip(this);
		}
		protected abstract void OnRealize();

		protected const float DELTA_FORCE = 400;
		private const float DELTA_ROTATION = 10;

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
