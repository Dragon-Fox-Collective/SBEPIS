//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

using Arbor.StateMachine.StateBehaviours;

namespace ArborEditor.StateMachine.StateBehaviours
{
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(Raycast2DTransition))]
	internal sealed class Raycast2DTransitionInspector : InspectorBase
	{
		protected override void OnRegisterElements()
		{
			RegisterProperty("_Origin");
			RegisterProperty("_Direction");
			RegisterProperty("_Distance");
			RegisterProperty("_LayerMask");
			RegisterProperty("_MinDepth");
			RegisterProperty("_MaxDepth");
			RegisterProperty("_CheckUpdate");
			RegisterProperty("_TagChecker");
			RegisterProperty("_RaycastHit2D");
		}
	}
}