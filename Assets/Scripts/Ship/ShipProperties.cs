using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	[System.Serializable]
	public enum ShipType
	{
		VOYAGER,
		DESTENY,
		SPLASH,
	}

	[System.Serializable]
	public class ShipProperties : IGunProperties
	{
		public ShipProperties(ShipType type)
		{
			m_type = type;
		}

		public ShipType type
		{
			get { return m_type; }
		}
		public string shipName
		{
			get { return ToName(m_type); }
		}
		public byte gunLevel
		{
			get { return m_gunLevel; }
			set { SetLevel(ref m_gunLevel, value); }
		}
		public byte specialGunLevel
		{
			get { return m_rocketLevel; }
			set { SetLevel(ref m_rocketLevel, value); }
		}
		public byte spellLevel
		{
			get { return m_spellLevel; }
			set { SetLevel(ref m_spellLevel, value); }
		}
		public byte passiveLevel
		{
			get { return m_passiveLevel; }
			set { SetLevel(ref m_passiveLevel, value); }
		}

		public const byte MIN_LEVEL = 1;
		public const byte MAX_LEVEL = 5;
		public const float MIN_HEALTH = 100;
		public const float MIN_MAGNETIC = 0.05f;

		public static string ToName(ShipType type)
		{
			return type.ToString().ToUpperInvariant();
		}

		private ShipType m_type;
		private string m_name;
		private byte m_gunLevel = 1;
		private byte m_rocketLevel = 1;
		private byte m_spellLevel = 1;
		private byte m_passiveLevel = 1;

		private void SetLevel(ref byte property, byte level)
		{
			if (level < MIN_LEVEL || level > MAX_LEVEL)
			{
				return;
			}

			property = level;
		}
	}

	public interface IGunProperties
	{
		byte gunLevel { get; }
		byte specialGunLevel { get; }
		byte spellLevel { get; }
		byte passiveLevel { get; }
	}
}
