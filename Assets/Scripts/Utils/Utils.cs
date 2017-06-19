using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
