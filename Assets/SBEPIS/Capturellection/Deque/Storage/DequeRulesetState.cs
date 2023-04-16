using System;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public abstract class DequeRulesetState
	{
		public virtual Vector3 direction { get; set; }
	}
	
	[Serializable]
	public class BaseState : DequeRulesetState { }
}
