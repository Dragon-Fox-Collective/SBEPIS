using System;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[Serializable]
	public class DequeRulesetLayerState : DequeRulesetState
	{
		public List<DequeRulesetState> states;

		private Vector3 _direction;
		public override Vector3 direction
		{
			get => _direction;
			set
			{
				_direction = value;
				foreach (DequeRulesetState state in states)
					state.direction = value;
			}
		}
	}
}
