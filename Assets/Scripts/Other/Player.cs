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
		public bool isDemaged { get; private set; }
		public bool isLossEnemy { get; private set; }
		public bool isAllowedModify { get { return modifications < MODIFICATION_COUNT; } }
		public byte modifications { get { return m_modifications; } }

		public const int MODIFICATION_COUNT = 12;

		public void AddPoints(int pointsCount)
		{
			m_points = Mathf.Clamp(pointsCount + m_points, MIN_POINTS, MAX_POINTS);
			m_bar.points = m_points;
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
			if (isDemaged)
			{
				return;
			}

			if (onDemaged != null) onDemaged();
			onDemaged = null;
			isDemaged = true;
		}
		public void LossEnemy()
		{
			if (isLossEnemy)
			{
				return;
			}

			if (onLossEnemy != null) onLossEnemy();
			onLossEnemy = null;
			isLossEnemy = true;
		}

		private int m_points;
		private byte m_modifications = 0;

		private const int MIN_POINTS = 0;
		private const int MAX_POINTS = 999999999;

		private IPlayerBar m_bar;
		private Ship m_ship;

		private Dictionary<UnitType, uint> m_killings = new Dictionary<UnitType, uint>();
	}
}
