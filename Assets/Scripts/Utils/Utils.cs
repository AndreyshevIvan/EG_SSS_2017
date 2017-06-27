using System;
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
			if (!IsTimerReady(timer, coldown))
			{
				timer += Time.deltaTime;
			}
		}
		public static bool IsTimerReady(float timer, float coldown)
		{
			return timer > coldown;
		}
		public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
		{
			if (value.CompareTo(min) < 0)
			{
				return min;
			}
			else if (value.CompareTo(max) > 0)
			{
				return max;
			}

			return min;
		}
		public static bool IsContain<T>(T value, T min, T max) where T : IComparable<T>
		{
			return value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
		}
		public static byte GetValidLevel(byte level)
		{
			return Clamp(level, GameData.minModLevel, GameData.maxModLevel);
		}
		public static bool GetDemage(ref float demage, Collider other)
		{
			Body demageBody = other.GetComponent<Body>();
			if (demageBody == null || demageBody.touchDemage == 0)
			{
				return false;
			}

			demage = demageBody.touchDemage;
			demageBody.OnDemageTaked();
			return true;
		}
		public static Vector3 WorldToCanvas(Vector3 worldPosition)
		{
			return Camera.main.WorldToScreenPoint(worldPosition);
		}
		public static List<T> ToList<T>(T[] arr)
		{
			return new List<T>(arr);
		}
		public static Vector3 RandomVect(float min, float max)
		{
			float x = UnityEngine.Random.Range(min, max);
			float y = UnityEngine.Random.Range(min, max);
			float z = UnityEngine.Random.Range(min, max);

			return new Vector3(x, y, z);
		}
		public static float MoveTowards(float current, float target, float step)
		{
			float absoluteCurrent = Mathf.Abs(current);
			float absoluteTarget = Mathf.Abs(target);

			if (absoluteCurrent >= absoluteTarget)
			{
				return target;
			}

			float newValue = Mathf.Abs(current + step);
			if (newValue >= absoluteTarget)
			{
				return target;
			}

			return current + step;
		}
		public static List<T> GetChilds<T>(Component parent) where T : Component
		{
			List<T> list = ToList(parent.GetComponentsInChildren<T>());
			list.Remove(parent.GetComponent<T>());
			return list;
		}
	}
}
