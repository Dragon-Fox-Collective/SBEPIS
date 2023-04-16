using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Utils
{
	[RequireComponent(typeof(Animator))]
	public abstract class StateMachine : MonoBehaviour
	{
		[SerializeField, HideInInspector, Self]
		private Animator state;
		protected Animator State => state;
	}
}
