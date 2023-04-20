using System;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	[Serializable]
	public class DequeRulesetLayerState : DequeRulesetState
	{
		public List<DequeRulesetState> states;

		private Vector3 _direction;
		public override Vector3 Direction
		{
			get => _direction;
			set
			{
				_direction = value;
				foreach (DequeRulesetState state in states)
					state.Direction = value;
			}
		}
	}
}
