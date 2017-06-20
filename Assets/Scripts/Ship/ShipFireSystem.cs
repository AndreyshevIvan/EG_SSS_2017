using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public abstract class ShipFireSystem : MonoBehaviour
	{
		public abstract void Init(IGunProperties properties);

		public bool isActiveReady
		{
			get { return Utils.IsColdownReady(m_activeTimer, m_activeColdown); }
		}

		protected abstract void FirstFire();
		protected abstract void SecondFire();
		protected virtual bool FirstFirePredicate() { return true; }
		protected abstract bool SecondFirePredicate();

		protected float m_firstColdown;
		protected float m_secondColdown;
		protected float m_activeColdown;

		private float m_firstTimer;
		private float m_secondTimer;
		private float m_activeTimer;

		private const float MIN_COLDOWN = 0.2f;

		private void FixedUpdate()
		{
			if (Utils.IsColdownReady(m_firstTimer, m_firstColdown) &&
				FirstFirePredicate())
			{
				FirstFire();
				m_firstTimer = 0;
			}
			if (Utils.IsColdownReady(m_secondColdown, m_secondTimer) &&
				SecondFirePredicate())
			{
				SecondFire();
				m_secondTimer = 0;
			}

			UpdateColdowns();
		}
		private void UpdateColdowns()
		{
			Utils.UpdateTimer(ref m_firstTimer, m_firstColdown);
			Utils.UpdateTimer(ref m_secondTimer, m_secondColdown);
			Utils.UpdateTimer(ref m_activeTimer, m_activeColdown);
		}
	}
}
