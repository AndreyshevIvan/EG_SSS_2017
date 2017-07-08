using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyGame.GameUtils;

namespace MyGame
{
	public class ModificationBar : UIBar
	{
		public GameObject m_plank;
		public Color m_inactive;
		public Color m_active;

		protected override void OnAwakeEnd()
		{
			CreateNewPlanks();
			ResetFadeElements();
		}
		protected override void OnSetNewValue()
		{
			m_planks.ForEach(plank =>
			{
				plank.color = m_inactive;

				if (m_planks.IndexOf(plank) <= value)
				{
					plank.color = m_active;
				}
			});
		}

		private List<Image> m_planks = new List<Image>();

		private void CreateNewPlanks()
		{
			List<Component> oldPlanks = Utils.GetChilds<Component>(transform);
			m_planks.ForEach(element => Destroy(element));
			m_planks.Clear();

			Utils.DoAnyTimes(GameWorld.MODIFICATION_COUNT, () =>
			{
				GameObject plank = Instantiate(m_plank, transform);
				Image image = plank.GetComponentInChildren<Image>();
				m_planks.Add(image);
				image.color = m_inactive;
			});
		}
	}
}
