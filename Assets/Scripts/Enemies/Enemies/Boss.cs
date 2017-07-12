using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FluffyUnderware.Curvy;
using FluffyUnderware.Curvy.Controllers;

namespace MyGame.Enemies
{
	public class Boss : Enemy
	{
		protected override void InitProperties()
		{
		}
		protected override void Shoot()
		{

		}

		[SerializeField]
		private BossUnit m_leftUnit;
		[SerializeField]
		private BossUnit m_rightUnit;

		[SerializeField]
		private CurvySpline m_mainSpline;
		[SerializeField]
		private CurvySpline m_leftSpline;
		[SerializeField]
		private CurvySpline m_rightSpline;

		private void InitUnits()
		{
			world.Add(m_leftUnit);
			world.Add(m_rightUnit);

			m_leftUnit.roadController.Spline = m_leftSpline;
			m_rightUnit.roadController.Spline = m_rightSpline;
		}
	}
}
