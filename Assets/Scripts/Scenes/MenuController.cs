using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class MenuController : MonoBehaviour
	{
		public MenuUI m_menuUI;

		private User m_user;

		private void Awake()
		{
			m_user = GameData.LoadUser();
			m_menuUI.stars = m_user.stars;
		}
	}
}
