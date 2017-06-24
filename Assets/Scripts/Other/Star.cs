using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class Star : Body
	{
		private Vector3 rotationAngle { get; set; }
		private Vector3 stopPosition { get; set; }

		private const float DELTA = 4;

		private void Start()
		{
			InitStopPosition();
			InitRotation();
		}
		private void InitStopPosition()
		{
			float deltaX = Random.Range(-DELTA, DELTA);
			float deltaZ = Random.Range(-DELTA, DELTA);

			Vector3 newStopPosition = position;
			newStopPosition.x += deltaX;
			newStopPosition.y = 1;
			newStopPosition.z += deltaZ;
			stopPosition = newStopPosition;
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

			if (position != stopPosition)
			{
				Vector3 newPos = Vector3.MoveTowards(position, stopPosition, 0.1f);
				position = newPos;
			}
		}
	}
}
