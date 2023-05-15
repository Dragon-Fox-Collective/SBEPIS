using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Utils.State
{
	public class InvokeTransitionReference : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere] private InvokeTransition transition;

		public void Invoke() => transition.Invoke();
	}
}
