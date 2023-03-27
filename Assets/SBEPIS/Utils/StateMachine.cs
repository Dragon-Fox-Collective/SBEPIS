using UnityEngine;

namespace SBEPIS.Utils
{
	[RequireComponent(typeof(Animator))]
	public abstract class StateMachine : MonoBehaviour
	{
		protected Animator state;
		
		private void Awake()
		{
			state = GetComponent<Animator>();
		}
	}
}
