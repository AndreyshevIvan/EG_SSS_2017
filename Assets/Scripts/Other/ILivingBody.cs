using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public interface ILivingBody
	{
		bool isLive { get; }
		bool isImmortal { get; }
		int health { get; }
		float healthPart { get; }
	}
}