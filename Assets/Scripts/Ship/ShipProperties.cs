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

	public abstract class ShipProperty : MonoBehaviour
	{
		public void Init(byte newLevel, IMapPhysics mapPhysics)
		{
			this.mapPhysics = mapPhysics;
			level = Utils.Clamp(newLevel, GameData.minModLevel, GameData.maxModLevel);
			OnInit();
		}
		public abstract void Modify();
		public void ResetTimer()
		{
			m_timer = 0;
		}

		protected float coldown { get; set; }
		protected bool isTimerWork { get; set; }
		protected byte level { get; set; }
		protected IMapPhysics mapPhysics { get; set; }

		protected bool isTimerReady
		{
			get { return Utils.IsTimerReady(m_timer, coldown); }
		}

		protected abstract void OnInit();

		private float m_timer;

		private void FixedUpdate()
		{
			if (isTimerWork)
			{
				Utils.UpdateTimer(ref m_timer, coldown);
			}
		}
	}

	public interface IShipProperties
	{
		byte baseGunLevel { get; }
		byte specificGunLevel { get; }
		byte activeSpellLevel { get; }
		byte passiveSpellLevel { get; }
	}
}
