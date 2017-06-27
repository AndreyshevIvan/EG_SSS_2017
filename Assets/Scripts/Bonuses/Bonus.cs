using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
	public abstract class Bonus : Body
	{
		public bool isMagnetic { get; set; }
		public bool isMoveWithGround { get; set; }

		public sealed override void OnDeleteByWorld()
		{
			world.EraseBonus(this);
		}

		protected override void OnAwake()
		{
			isMagnetic = false;
		}
		protected override void OnTrigger(Collider other)
		{
			if (other.gameObject.layer == MapPhysics.WORLD_BOX_LAYER)
			{
				return;
			}

			OnRealize();
			world.EraseBonus(this);
		}
		protected abstract void OnRealize();
	}
}
