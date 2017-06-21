using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame
{
	public interface IModifiable
	{
		byte level { get; }
		byte maxLevel { get; }
		byte minLevel { get; }

		void SetLevel(byte newLevel);
		void Modify();
	}
}
