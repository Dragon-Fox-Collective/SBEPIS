//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

using Arbor.StateMachine.StateBehaviours;

namespace ArborEditor.StateMachine.StateBehaviours
{
	[CustomEditor(typeof(OnControllerColliderHitTransition))]
	internal sealed class OnControllerColliderHitTransitionInspector : CheckTagBehaviourBaseInspector
	{
		protected override void OnRegisterElements()
		{
			base.OnRegisterElements();

			RegisterProperty("_Hit");
		}
	}
}
