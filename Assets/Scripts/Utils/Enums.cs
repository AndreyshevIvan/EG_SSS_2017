using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame
{
	[System.Serializable]
	public enum UnitType
	{
		BASE_ENEMY,
		ROCKET_COPTER,
		TARGET_TURRET,
		CIRCLE_TURRET,
	}

	[System.Serializable]
	public enum ShipType
	{
		STANDART,
		TANK,
		DEMAGER,
	}

	[System.Serializable]
	public enum RoadType
	{
		BIG,
		LEFT,
		RIGHT,
		DIFF,
		PLAYER,
	}

	[System.Serializable]
	public enum BonusType
	{
		STAR,
		HEALTH,
		AMMO_UP,
	}

	[System.Serializable]
	public enum BarType
	{
		PLAYER_HEALTH,
		ENEMY_HEALTH,
	}

	[System.Serializable]
	public enum Maps
	{
		FIRST,
		SECOND,
		THIRD,
	}
}
