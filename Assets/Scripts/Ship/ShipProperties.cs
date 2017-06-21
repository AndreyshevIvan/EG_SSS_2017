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
	public class ShipProperties : IShipProperties
	{
		public ShipProperties(ShipType type)
		{
			m_type = type;
		}

		public ShipType type { get { return m_type; } }
		public string shipName { get { return ToName(m_type); } }
		public byte baseGunLevel
		{
			get { return m_gunLevel; }
			set { SetLevel(ref m_gunLevel, value); }
		}
		public byte specificGunLevel
		{
			get { return m_rocketLevel; }
			set { SetLevel(ref m_rocketLevel, value); }
		}
		public byte activeSpellLevel
		{
			get { return m_spellLevel; }
			set { SetLevel(ref m_spellLevel, value); }
		}
		public byte passiveSpellLevel
		{
			get { return m_passiveLevel; }
			set { SetLevel(ref m_passiveLevel, value); }
		}

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
			property = Utils.Clamp(level, GameData.minModLevel, GameData.maxModLevel);
		}
	}

	public abstract class ShipProperty : MonoBehaviour, IModifiable
	{
		public byte level { get { return m_level; } }
		public byte maxLevel { get { return GameData.maxModLevel; } }
		public byte minLevel { get { return GameData.minModLevel; } }

		public void SetLevel(byte newLevel)
		{
			if (newLevel < minLevel || newLevel > maxLevel)
			{
				return;
			}

			OnChangeLevel();
		}
		public abstract void Modify();

		protected abstract void OnChangeLevel();

		protected byte m_level;

		protected float m_timer;
		protected float m_coldown;
	}

	public interface IShipProperties
	{
		byte baseGunLevel { get; }
		byte specificGunLevel { get; }
		byte activeSpellLevel { get; }
		byte passiveSpellLevel { get; }
	}
}
