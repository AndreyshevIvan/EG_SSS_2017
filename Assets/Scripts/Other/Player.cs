using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MyGame.Hero;

namespace MyGame
{
	public class Player
	{
		public Player(IPlayerBar bar, Ship ship)
		{
			m_points = 0;
			m_ship = ship;
			m_bar = bar;
		}

		public EventDelegate onDemaged;
		public EventDelegate onLossEnemy;

		public bool isWin { get; set; }
		public bool isDemaged { get { return m_isDemaged; } }
		public bool isLossEnemy { get { return m_isLossEnemy; } }
		public bool isAllowedModify { get { return modifications < MODIFICATION_COUNT; } }
		public byte modifications { get { return m_modifications; } }

		public int stars { get { return m_stars; } }
		public int points { get { return m_points; } }
		public int bombPersents { get; set; }
		public int laserPercents { get; set; }

		public const int MODIFICATION_COUNT = 12;

		public void AddPoints(int pointsCount)
		{
			m_points = Mathf.Clamp(pointsCount + m_points, MIN_POINTS, MAX_POINTS);
			m_bar.points = m_points;
		}
		public void AddStars(int count)
		{
			m_stars += count;
		}
		public void Modify()
		{
			if (!isAllowedModify)
			{
				return;
			}

			m_modifications++;
			m_ship.mind.ModificateByOne();
			m_bar.modifications = modifications;
		}
		public bool Laser()
		{
			return true;
		}
		public bool Bomb()
		{
			return true;
		}
		public void Heal(int healthCount)
		{
			m_ship.Heal(healthCount);
		}
		public void KillEnemy(UnitType type)
		{
			if (!m_killings.ContainsKey(type))
			{
				m_killings.Add(type, 1);
				return;
			}

			uint killsCount;
			m_killings.TryGetValue(type, out killsCount);
			m_killings.Remove(type);
			m_killings.Add(type, killsCount + 1);
		}

		public void BeDemaged()
		{
			SetTrigger(ref m_isDemaged, onDemaged);
		}
		public void LossEnemy()
		{
			SetTrigger(ref m_isLossEnemy, onLossEnemy);
		}

		private int m_points;
		private int m_stars;
		private byte m_modifications = 0;
		private bool m_isDemaged = false;
		private bool m_isLossEnemy = false;

		private const int MIN_POINTS = 0;
		private const int MAX_POINTS = 999999999;

		private IPlayerBar m_bar;
		private Ship m_ship;

		private Dictionary<UnitType, uint> m_killings = new Dictionary<UnitType, uint>();

		private void SetTrigger(ref bool trigger, EventDelegate onTriggerEvent)
		{
			if (trigger)
			{
				return;
			}

			if (onTriggerEvent != null) onTriggerEvent();
			onTriggerEvent = null;
			trigger = true;
		}
	}
}
