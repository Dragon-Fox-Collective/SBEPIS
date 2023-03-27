using System;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public abstract class DequeRulesetState
	{
		public virtual Vector3 direction { get; set; }
	}
	
	[Serializable]
	public class BaseState : DequeRulesetState { }
}
