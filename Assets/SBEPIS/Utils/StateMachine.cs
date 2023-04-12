using UnityEngine;

namespace SBEPIS.Utils
{
	[RequireComponent(typeof(Animator))]
	public abstract class StateMachine : MonoBehaviour
	{
		protected Animator State { get; private set; }
		
		protected virtual void Awake()
		{
			State = GetComponent<Animator>();
		}
	}
}
