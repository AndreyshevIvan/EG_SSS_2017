using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public class GameplayController : MonoBehaviour, IMapPhysics
	{
		public ShipModel m_shipModel;
		public ShipModelsManager m_modelsManager;
		public ShipController m_controller;
		public MatchShip m_ship;

		public void Pause(bool isPause)
		{
		}

		private User m_user;

		private void Start()
		{
			InitUser();
		}
		private void FixedUpdate()
		{

		}
		private void InitUser()
		{
			m_user = GameData.LoadUser();
			GameObject shipBody = m_modelsManager.Get(m_user.ship);
			m_shipModel.SetBody(shipBody);
		}
	}
}
