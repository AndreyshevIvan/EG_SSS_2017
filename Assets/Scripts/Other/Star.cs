using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public sealed class Star : Body
	{
		protected override void OnTrigger(Collider other)
		{
			world.EraseStar(this);
		}
		protected override void OnTouchDeleter()
		{
			world.EraseStar(this);
		}

		private float moveDelta { get; set; }

		private const float DELTA_FORCE = 400;
		private const float DELTA_ROTATION = 10;

		private void Start()
		{
			Vector3 force = Utils.RandomVect(-DELTA_FORCE, DELTA_FORCE);
			physicsBody.AddForce(force);

			Vector3 rotation = Utils.RandomVect(-DELTA_ROTATION, DELTA_ROTATION);
			physicsBody.AddTorque(rotation);
		}
		private void FixedUpdate()
		{
			world.MoveToShip(this);
		}
	}
}
