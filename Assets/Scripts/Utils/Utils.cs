using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	[System.Serializable]
	public struct Boundary
	{
		public float xMin;
		public float xMax;
		public float yMin;
		public float yMax;
	}

	public static class Utils
	{
		public static void SetWidth(RectTransform rect, float width)
		{
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
		}
		public static void SetHeight(RectTransform rect, float height)
		{
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
		}
		public static void SetSize(RectTransform rect, float size)
		{
			SetWidth(rect, size);
			SetHeight(rect, size);
		}
	}
}
