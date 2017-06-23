using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	[System.Serializable]
	public struct BoundingBox
	{
		public BoundingBox(float xMin, float xMax, float zMin, float zMax)
		{
			this.xMin = xMin;
			this.xMax = xMax;
			this.zMin = zMin;
			this.zMax = zMax;
		}

		public float xMin;
		public float xMax;
		public float zMin;
		public float zMax;
	}
}