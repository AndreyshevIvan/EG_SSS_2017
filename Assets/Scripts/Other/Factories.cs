using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FluffyUnderware.Curvy;
using MyGame.Hero;

namespace MyGame
{
	public class Factories : MonoBehaviour
	{
		public MapsFactory maps;
		public EnemiesFactory enemies;
		public ShipsFactory ships;
		public RoadsFactory roads;
		public BonusesFactory bonuses;
		public IBarsFactory bars;
	}
}
