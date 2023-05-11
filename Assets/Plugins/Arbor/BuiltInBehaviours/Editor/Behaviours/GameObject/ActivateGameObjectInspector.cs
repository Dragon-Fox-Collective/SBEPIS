//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

using Arbor.StateMachine.StateBehaviours;

namespace ArborEditor.StateMachine.StateBehaviours
{
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(ActivateGameObject))]
	internal sealed class ActivateGameObjectInspector : InspectorBase
	{
		protected override void OnRegisterElements()
		{
			RegisterProperty("_Target");
			RegisterProperty("_BeginActive");
			RegisterProperty("_EndActive");
		}
	}
}