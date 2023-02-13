using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Utils
{
	public abstract class Transition : MonoBehaviour
	{
		public State fromState;
		public State toState;

		public abstract bool CanTransition();
	}
}
