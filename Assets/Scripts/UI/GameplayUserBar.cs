using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
	public class GameplayUserBar : MonoBehaviour
	{
		public Text m_points;
		public Text m_stars;
		public Text m_modify;

		public byte modifyCount { get; set; }
		public byte modify { get; set; }
		public uint points { get; set; }
		public uint stars { get; set; }

		private void FixedUpdate()
		{

		}
	}
}
