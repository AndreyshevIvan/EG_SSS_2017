using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame
{
	[System.Serializable]
	public enum EnemyType
	{
		EASY,
		NORMAL,
		HARD,
	}

	[System.Serializable]
	public enum ShipType
	{
		VOYAGER,
		DESTENY,
		SPLASH,
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
}
