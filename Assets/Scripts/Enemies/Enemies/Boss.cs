using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using FluffyUnderware.Curvy;
using FluffyUnderware.Curvy.Controllers;

namespace MyGame.Enemies
{
	using RoadAction = UnityAction<CurvySplineMoveEventArgs>;

	public class Boss : Enemy
	{
		protected override void OnAwakeEnd()
		{
			m_controller = m_bossBody.GetComponent<SplineController>();
		}
		protected override void InitProperties()
		{
		}
		protected override void Shoot()
		{

		}

		[SerializeField]
		private GameObject m_bossBody;

		[SerializeField]
		private BossUnit m_leftUnit;
		[SerializeField]
		private BossUnit m_rightUnit;

		[SerializeField]
		private CurvySpline m_mainSpline;

		private int m_readyUnits = 0;
		private SplineController m_controller;

		private const int UNITS_COUNT = 3;

		private void InitUnits()
		{
			world.Add(m_leftUnit);
			world.Add(m_rightUnit);

			RoadAction readyAction = (T) =>
			{
				m_readyUnits++;
				if (m_readyUnits == UNITS_COUNT) OnUnitsReady();
			};
			m_leftUnit.roadController.OnEndReached.AddListener(readyAction);
			m_rightUnit.roadController.OnEndReached.AddListener(readyAction);
			m_controller.OnEndReached.AddListener(readyAction);
		}
		private void OnUnitsReady()
		{
			Debug.Log("All ready");
		}
	}
}
