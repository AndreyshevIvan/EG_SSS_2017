using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
	public class Results : MonoBehaviour
	{
		public void Open(User oldUser, User newUser, Player player, bool isWin)
		{
			this.oldUser = oldUser;
			this.newUser = newUser;
			this.player = player;
			string title = StrManager.Get((uint)((isWin) ? 6 : 7));
			m_title.text = title;
			StartCoroutine(Animate());
		}

		private void Awake()
		{
			graphics = Utils.GetAllComponents<Graphic>(this);
			graphics.ForEach(element => element.CrossFadeAlpha(0, 0, true));
		}
		private IEnumerator Animate()
		{
			graphics.ForEach(element => element.CrossFadeAlpha(1, FADE_TIME, true));
			yield return new WaitForSeconds(FADE_TIME);
		}

		[SerializeField]
		private Animator m_animator;
		[SerializeField]
		private Text m_title;

		private User oldUser { get; set; }
		private User newUser { get; set; }
		private Player player { get; set; }
		private List<Graphic> graphics { get; set; }

		private const float FADE_TIME = 0.4f;
	}
}
