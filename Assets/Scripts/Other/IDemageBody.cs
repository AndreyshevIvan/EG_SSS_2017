using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public interface IDemageBody
	{
		float demage { get; }

		void OnDemageTaked();
	}
}