using System;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public abstract class DequeRulesetState
	{
		public Vector3 direction;
	}
	
	[Serializable]
	public class BaseState : DequeRulesetState { }
}
