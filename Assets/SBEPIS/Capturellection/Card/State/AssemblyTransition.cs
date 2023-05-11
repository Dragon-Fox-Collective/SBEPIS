using Arbor;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	[AddBehaviourMenu("Transition/AssemblyTransition")]
	public class AssemblyTransition : StateBehaviour
	{
		[Tooltip("If true, if the grabbable is already in a state, immediately transition instead of waiting for an event")]
		[SerializeField] private bool passIfValid = true;
		
		[SerializeField] private FlexibleDequeElement dequeElement = new();
		
		[SerializeField] private StateLink onStartAssembly = new();
		[SerializeField] private StateLink onStartDisassembly = new();
		[SerializeField] private StateLink onStopAssemblyAndDisassembly = new();
		
		private bool addedListeners;
		
		public override void OnStateBegin()
		{
			addedListeners = false;
			
			if (passIfValid)
				if (dequeElement.value.IsAssembling && Transition(onStartAssembly))
					return;
				else if (dequeElement.value.IsDisassembling && Transition(onStartDisassembly))
					return;
				else if (!dequeElement.value.IsAssembling && !dequeElement.value.IsDisassembling && Transition(onStopAssemblyAndDisassembly))
					return;
			
			addedListeners = true;
			
			AddListeners();
		}
		
		public override void OnStateEnd()
		{
			if (addedListeners)
				RemoveListeners();
		}
		
		private void AddListeners()
		{
			dequeElement.value.onStartAssembling.AddListener(OnStartAssembly);
			dequeElement.value.onStartDisassembling.AddListener(OnStartDisassembly);
			dequeElement.value.onStopAssemblingAndDisassembling.AddListener(OnStopAssemblyAndDisassembly);
		}
		
		private void RemoveListeners()
		{
			dequeElement.value.onStartAssembling.RemoveListener(OnStartAssembly);
			dequeElement.value.onStartDisassembling.RemoveListener(OnStartDisassembly);
			dequeElement.value.onStopAssemblingAndDisassembling.RemoveListener(OnStopAssemblyAndDisassembly);
		}
		
		private void OnStartAssembly() => Transition(onStartAssembly);
		private void OnStartDisassembly() => Transition(onStartDisassembly);
		private void OnStopAssemblyAndDisassembly() => Transition(onStopAssemblyAndDisassembly);
	}
}
