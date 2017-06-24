using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class Star : Body
	{
		private Vector3 rotationAngle { get; set; }
		private Vector3 endPosition { get; set; }
		private float moveDelta { get; set; }

		private const float DELTA = 4;
		private const float MOVE_DELTA = 0.95f;
		private const float START_MOVE_DELTA = 0.1f;
		private const float CRITICAL_MOVE_DELTA = 0.01f;

		private void Start()
		{
			InitStopPosition();
			InitRotation();
			moveDelta = START_MOVE_DELTA;
		}
		private void InitStopPosition()
		{
			float deltaX = Random.Range(-DELTA, DELTA);
			float deltaZ = Random.Range(-DELTA, DELTA);

			Vector3 newStopPosition = position;
			newStopPosition.x += deltaX;
			newStopPosition.y = 1;
			newStopPosition.z += deltaZ;
			endPosition = newStopPosition;
		}
		private void InitRotation()
		{
			float x = Random.Range(0.1f, 1.0f);
			float y = Random.Range(0.1f, 1.0f);
			float z = Random.Range(0.1f, 1.0f);

			rotationAngle = new Vector3(x, y, z);
		}
		private void FixedUpdate()
		{
			transform.Rotate(rotationAngle);

			if (position != endPosition)
			{
				position = Vector3.MoveTowards(position, endPosition, moveDelta);
				if (moveDelta * MOVE_DELTA > CRITICAL_MOVE_DELTA)
				{
					moveDelta *= MOVE_DELTA;
				}
			}
		}
	}
}
