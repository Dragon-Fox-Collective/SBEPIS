//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

using Arbor.StateMachine.StateBehaviours;

namespace ArborEditor.StateMachine.StateBehaviours
{
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(LookAtGameObject))]
	internal sealed class LookAtGameObjectInspector : InspectorBase
	{
		protected override void OnRegisterElements()
		{
			RegisterProperty("_Transform");
			RegisterProperty("_Target");
			RegisterProperty("_UsePositionX");
			RegisterProperty("_UsePositionY");
			RegisterProperty("_UsePositionZ");
			RegisterProperty("_ApplyLateUpdate");
		}
	}
}