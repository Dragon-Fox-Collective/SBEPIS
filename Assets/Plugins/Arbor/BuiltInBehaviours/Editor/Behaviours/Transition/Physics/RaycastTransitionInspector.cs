//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

using Arbor.StateMachine.StateBehaviours;

namespace ArborEditor.StateMachine.StateBehaviours
{
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(RaycastTransition))]
	internal sealed class RaycastTransitionInspector : InspectorBase
	{
		protected override void OnRegisterElements()
		{
			RegisterProperty("_Origin");
			RegisterProperty("_Direction");
			RegisterProperty("_Distance");
			RegisterProperty("_LayerMask");
			RegisterProperty("_CheckUpdate");
			RegisterProperty("_IsCheckTag");
			RegisterProperty("_Tag");
			RegisterProperty("_RaycastHit");
		}
	}
}
