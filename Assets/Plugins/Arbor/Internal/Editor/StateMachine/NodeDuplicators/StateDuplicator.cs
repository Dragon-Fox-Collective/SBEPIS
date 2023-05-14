//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections.Generic;

using Arbor;

namespace ArborEditor
{
	[CustomNodeDuplicator(typeof(State),typeof(ArborFSMInternal))]
	internal sealed class StateDuplicator : NodeDuplicator
	{
		protected override Node OnDuplicate()
		{
			State sourceState = sourceNode as State;
			ArborFSMInternal stateMachine = targetGraph as ArborFSMInternal;
			State state = null;
			if (isClip)
			{
				state = stateMachine.CreateState(sourceState.nodeID, sourceState.resident);
			}
			else
			{
				state = stateMachine.CreateState(sourceState.resident);
			}

			return state;
		}

		protected override void OnAfterDuplicate(List<NodeDuplicator> duplicators)
		{
			State sourceState = sourceNode as State;
			State state = destNode as State;

			int behaviourCount = sourceState.behaviourCount;
			for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
			{
				StateBehaviour sourceBehaviour = sourceState.GetBehaviourFromIndex(behaviourIndex);
				if (sourceBehaviour != null)
				{
					StateBehaviour behaviour = NodeBehaviour.CreateNodeBehaviour(state, sourceBehaviour.GetType(), true) as StateBehaviour;

					if (behaviour != null)
					{
						state.AddBehaviour(behaviour);

						StateBehaviourClipboardProcessor.CopyBehaviour(sourceBehaviour, behaviour, !isClip, duplicators);
						RegisterBehaviour(sourceBehaviour, behaviour);
					}
				}
			}
		}
	}
}