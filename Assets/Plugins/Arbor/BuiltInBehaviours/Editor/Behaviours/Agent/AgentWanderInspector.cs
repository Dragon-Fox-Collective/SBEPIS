//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

using Arbor.StateMachine.StateBehaviours;

namespace ArborEditor.StateMachine.StateBehaviours
{
	[CustomEditor(typeof(AgentWander))]
	internal sealed class AgentWanderInspector : AgentMoveBaseInspector
	{
		SerializedProperty _RadiusProperty;
		SerializedProperty _DistanceProperty;
		SerializedProperty _JitterProperty;
		SerializedProperty _StoppingDistanceProperty;

		protected override void OnRegisterElements()
		{
			base.OnRegisterElements();

			RegisterProperty("_Radius");
			RegisterProperty("_Distance");
			RegisterProperty("_Jitter");
			RegisterProperty("_StoppingDistance");
		}
	}
}