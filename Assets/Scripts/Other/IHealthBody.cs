using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public interface IHealthBody
	{
		float addDemage { set; }
		bool isLive { get; }
		int health { get; }
		float healthPart { get; }
	}
}