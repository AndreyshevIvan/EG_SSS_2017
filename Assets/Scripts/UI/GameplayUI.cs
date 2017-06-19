using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
	public RectTransform m_shipArea;

	public float areaSize
	{
		set
		{
			float size = value * 2;
			Utils.SetSize(m_shipArea, size * AREA_SCALE_FACTOR);
		}
	}
	public Vector3 areaPosition
	{
		set
		{
			m_shipArea.position = value;
		}
	}

	private const float AREA_SCALE_FACTOR = 1.25f;
}
