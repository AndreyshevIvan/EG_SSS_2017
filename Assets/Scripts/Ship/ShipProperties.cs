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
	public class ShipProperties
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
		public byte firstGunLevel
		{
			get { return m_firstGunLevel; }
			set { SetLevel(ref m_firstGunLevel, value); }
		}
		public byte secondGunLevel
		{
			get { return m_secondGunLevel; }
			set { SetLevel(ref m_secondGunLevel, value); }
		}
		public byte activeLevel
		{
			get { return m_activeSpellLevel; }
			set { SetLevel(ref m_activeSpellLevel, value); }
		}
		public byte passiveLevel
		{
			get { return m_passiveSpellLevel; }
			set { SetLevel(ref m_passiveSpellLevel, value); }
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
		private byte m_firstGunLevel = 1;
		private byte m_secondGunLevel = 1;
		private byte m_activeSpellLevel = 1;
		private byte m_passiveSpellLevel = 1;

		private void SetLevel(ref byte property, byte level)
		{
			if (level < MIN_LEVEL || level > MAX_LEVEL)
			{
				return;
			}

			property = level;
		}
	}
}
