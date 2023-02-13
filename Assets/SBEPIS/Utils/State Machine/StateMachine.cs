using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Utils
{
	public class StateMachine : MonoBehaviour
	{
		public List<State> states = new();
		public List<Transition> transitions = new();
	}
}
