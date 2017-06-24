using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public interface IDemageBody
	{
		float touchDemage { get; }

		void OnDemageTaked();
	}
}