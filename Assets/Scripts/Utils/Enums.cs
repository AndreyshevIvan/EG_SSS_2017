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
		ANGLE_TURRET,
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
		PRE_START,
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
	public enum MapType
	{
		FIRST,
		SECOND,
		THIRD,
	}

	[System.Serializable]
	public enum AmmoType
	{
		PLAYER_BULLET,
		TARGET_TURRET,
		ANGLE_TURRET,
		COPTER_ROCKET,
		PLAYER_BOMB,
		PLAYER_LASER,
	}

	[System.Serializable]
	public enum ResType
	{
		LOCALE,
		LEVEL_PRICE,
	}

	[System.Serializable]
	public enum UpdType
	{
		FIXED,
		UI,
	}

	[System.Serializable]
	public enum Layer
	{
		UNTOUCH = 0,
		PLAYER_BULLET = 8,
	}
}
