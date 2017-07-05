using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	[System.Serializable]
	public class User : ICloneable
	{
		public void AddNew(TempPlayer player)
		{

		}
		public object Clone()
		{
			return this.MemberwiseClone();
		}

		public string userName { get { return m_name; } set { m_name = value; } }
		public ShipType ship { get { return m_ship; } }
		public uint stars { get { return m_stars; } }
		public uint points { get; set; }
		public ushort level { get { return m_level; } }

		private string m_name = "noname";

		private uint m_stars = 0;
		private uint m_points = 0;
		private ushort m_level = 1;

		private ShipType m_ship;
	}
}
