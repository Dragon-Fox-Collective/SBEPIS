using UnityEngine;

namespace SBEPIS.Utils
{
	public abstract class StateMachine
	{
		protected Animator state;
		public StateMachine(Animator state)
		{
			this.state = state;
		}
	}
}
