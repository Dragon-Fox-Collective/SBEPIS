using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Utils
{
	[RequireComponent(typeof(Animator))]
	public abstract class StateMachine : MonoBehaviour
	{
		[SerializeField, Self]
		private Animator state;
		protected Animator State => state;
	}
}
