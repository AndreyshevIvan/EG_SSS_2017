using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
	public class GameplayUI : MonoBehaviour
	{
		public RectTransform m_shipArea;

		public float areaSize
		{
			set { Utils.SetSize(m_shipArea, 2 * value * AREA_SCALE_FACTOR); }
		}
		public Vector3 areaPosition
		{
			set { m_shipArea.position = value; }
		}

		private const float AREA_SCALE_FACTOR = 1.25f;
	}
}
