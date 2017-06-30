using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame.World;

namespace MyGame.World
{
	[System.Serializable]
	public class ShipProperties : IShipProperties
	{
		public ShipProperties(ShipType type)
		{
			m_type = type;
		}

		public ShipType type { get { return m_type; } }
		public string shipName { get { return ToName(m_type); } }
		public byte firstGunLevel
		{
			get { return m_gunLevel; }
			set { m_gunLevel = Utils.GetValidLevel(value); }
		}
		public byte secondGunLevel
		{
			get { return m_rocketLevel; }
			set { m_rocketLevel = Utils.GetValidLevel(value); }
		}
		public byte firstSpellLevel
		{
			get { return m_spellLevel; }
			set { m_spellLevel = Utils.GetValidLevel(value); }
		}
		public byte secondSpellLevel
		{
			get { return m_passiveLevel; }
			set { m_passiveLevel = Utils.GetValidLevel(value); }
		}

		public static string ToName(ShipType type)
		{
			return type.ToString().ToUpperInvariant();
		}

		private ShipType m_type;
		private byte m_gunLevel = 1;
		private byte m_rocketLevel = 1;
		private byte m_spellLevel = 1;
		private byte m_passiveLevel = 1;
	}

	public abstract class ShipProperty : MonoBehaviour
	{
		public void Init(byte newLevel, IMapPhysics map, Body target = null)
		{
			level = Utils.GetValidLevel(newLevel);
			world = map;
			isTimerWork = true;
			this.target = target;
			DoAfterInit();
		}
		public virtual void Modify() { }
		public void ResetTimer()
		{
			m_timer = 0;
		}

		protected float coldown { get; set; }
		protected byte level { get; set; }
		protected IMapPhysics world { get; set; }
		protected Body target { get; set; }
		protected bool isTimerReady
		{
			get { return Utils.IsTimerReady(m_timer, coldown); }
		}

		protected abstract void DoAfterInit();
		protected void FixedUpdate()
		{
			if (!world.gameplay.isPlaying || !isTimerWork)
			{
				return;
			}

			Utils.UpdateTimer(ref m_timer, coldown, Time.fixedDeltaTime);
		}

		private float m_timer = 0;

		internal bool isTimerWork { get; set; }
	}

	public interface IShipProperties
	{
		byte firstGunLevel { get; }
		byte secondGunLevel { get; }
		byte firstSpellLevel { get; }
		byte secondSpellLevel { get; }
	}
}
