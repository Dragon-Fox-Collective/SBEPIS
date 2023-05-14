//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.StateMachine.StateBehaviours
{
	using ArborEditor.Inspectors;

	internal abstract class CheckTagBehaviourBaseInspector : InspectorBase
	{
		protected override void OnRegisterElements()
		{
			RegisterProperty("_TagChecker");
		}
	}
}