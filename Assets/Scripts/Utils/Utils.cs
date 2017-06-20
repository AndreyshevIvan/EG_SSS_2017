using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
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

		public static string ToMoney(uint value)
		{
			if (value < 1000)
			{
				return value.ToString();
			}

			uint kCount = value / 1000;
			uint mod = value % 1000;
			return kCount.ToString() + '.' + mod.ToString()[0] + " k";
		}

		public static void UpdateTimer(ref float timer, float coldown)
		{
			if (!IsColdownReady(timer, coldown))
			{
				timer += Time.deltaTime;
			}
		}
		public static bool IsColdownReady(float timer, float coldown)
		{
			return timer > coldown;
		}
	}
}
