using Arbor;
using UnityEngine;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("Transition/DequeElementAssemblyTransition")]
	public class DequeElementAssemblyTransition : StateBehaviour
	{
		[Tooltip("If true, if the grabbable is already in a state, immediately transition instead of waiting for an event")]
		[SerializeField] private bool passIfValid = true;
		
		[SerializeField] private FlexibleDequeElement dequeElement;
		
		[SerializeField] private StateLink onStartAssembly;
		[SerializeField] private StateLink onStartDisassembly;
		[SerializeField] private StateLink onStopAssemblyAndDisassembly;
		
		private bool addedListeners = false;
		
		public override void OnStateBegin()
		{
			Debug.Log($"{name} {node.name} {dequeElement.value.IsAssembling} {dequeElement.value.IsDisassembling}");
			if (passIfValid)
				if (dequeElement.value.IsAssembling && Transition(onStartAssembly))
					return;
				else if (dequeElement.value.IsDisassembling && Transition(onStartDisassembly))
					return;
				else if (!dequeElement.value.IsAssembling && !dequeElement.value.IsDisassembling && Transition(onStopAssemblyAndDisassembly))
					return;
			
			AddListeners();
		}
		
		public override void OnStateEnd()
		{
			if (addedListeners)
				RemoveListeners();
		}
		
		private void AddListeners()
		{
			addedListeners = true;
			dequeElement.value.onStartAssembling.AddListener(OnStartAssembly);
			dequeElement.value.onStartDisassembling.AddListener(OnStartDisassembly);
			dequeElement.value.onStopAssemblingAndDisassembling.AddListener(OnStopAssemblyAndDisassembly);
		}
		
		private void RemoveListeners()
		{
			addedListeners = false;
			dequeElement.value.onStartAssembling.RemoveListener(OnStartAssembly);
			dequeElement.value.onStartDisassembling.RemoveListener(OnStartDisassembly);
			dequeElement.value.onStopAssemblingAndDisassembling.RemoveListener(OnStopAssemblyAndDisassembly);
		}
		
		private void OnStartAssembly() => Transition(onStartAssembly);
		private void OnStartDisassembly() => Transition(onStartDisassembly);
		private void OnStopAssemblyAndDisassembly() => Transition(onStopAssemblyAndDisassembly);
	}
}
