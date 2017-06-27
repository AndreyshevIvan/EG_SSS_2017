using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyGame
{
	public sealed class Star : Bonus
	{
		protected override void OnRealize()
		{
		}
		protected override void OnUpdate()
		{
			world.MoveToShip(this);
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

			world.SubscribeToMove(this);
		}
	}
}
