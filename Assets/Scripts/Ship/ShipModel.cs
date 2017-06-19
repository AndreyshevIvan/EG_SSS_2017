using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class ShipModel : MonoBehaviour
	{
		public Transform m_leftGun;
		public Transform m_rightGun;

		public void SetBody(GameObject newBody)
		{
			ClearBody();
			m_body = Instantiate(newBody, transform);
		}

		private GameObject m_body;

		private void Awake()
		{
			ClearBody();
		}
		private void ClearBody()
		{
			if (m_body == null)
			{
				return;
			}

			Destroy(m_body);
		}
	}
}
