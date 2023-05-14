using Arbor;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.State
{
	public abstract class EventTransitionBase : StateBehaviour
	{
		[Tooltip("If true, if the grabbable is already in a state, immediately transition instead of waiting for an event")]
		[SerializeField] private bool passIfValid = true;

		protected abstract bool InitialValue { get; }
		protected abstract StateLink TrueLink { get; }
		protected abstract StateLink FalseLink { get; }
		
		private bool addedListeners;
		
		public override void OnStateBegin()
		{
			addedListeners = false;
			
			if (passIfValid)
				if (InitialValue && Transition(TrueLink))
					return;
				else if (!InitialValue && Transition(FalseLink))
					return;
			
			addedListeners = true;
			
			AddListeners();
		}
		
		public override void OnStateEnd()
		{
			if (addedListeners)
				RemoveListeners();
		}
		
		protected abstract void AddListeners();
		protected abstract void RemoveListeners();
	}
	
	public abstract class EventTransition : EventTransitionBase
	{
		protected abstract UnityEvent TrueEvent { get; }
		protected abstract UnityEvent FalseEvent { get; }
		
		protected override void AddListeners()
		{
			TrueEvent?.AddListener(TrueListener);
			FalseEvent?.AddListener(FalseListener);
		}
		
		protected override void RemoveListeners()
		{
			TrueEvent?.RemoveListener(TrueListener);
			FalseEvent?.RemoveListener(FalseListener);
		}
		
		private void TrueListener() => Transition(TrueLink);
		private void FalseListener() => Transition(FalseLink);
	}
	
	public abstract class EventTransition<T1, T2> : EventTransitionBase
	{
		protected abstract UnityEvent<T1, T2> TrueEvent { get; }
		protected abstract UnityEvent<T1, T2> FalseEvent { get; }
		
		protected override void AddListeners()
		{
			TrueEvent?.AddListener(TrueListener);
			FalseEvent?.AddListener(FalseListener);
		}
		
		protected override void RemoveListeners()
		{
			TrueEvent?.RemoveListener(TrueListener);
			FalseEvent?.RemoveListener(FalseListener);
		}
		
		private void TrueListener(T1 t1, T2 t2) => Transition(TrueLink);
		private void FalseListener(T1 t1, T2 t2) => Transition(FalseLink);
	}
}
