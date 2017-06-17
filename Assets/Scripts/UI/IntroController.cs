﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
	public class IntroController : MonoBehaviour
	{
		public Text m_helper;
		public Text m_gameName;

		private void Awake()
		{
			m_gameName.text = String.Get(1);
			m_helper.text = String.Get(2);
		}
	}

}
